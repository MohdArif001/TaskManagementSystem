using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TaskDashboard.Models;
using TaskDashboard.Utility;

namespace TaskDashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            LoginResponse loginResponse = new LoginResponse();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                string url = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.Login;
                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
                }
            }
            if (loginResponse.Message == TaskDashboardStaticValues.LoginMessage)
            {
                string token = loginResponse.Token;
                HttpContext.Session.SetString("_token", token);
                HttpContext.Session.SetString("_Email", model.Email);
                HttpContext.Session.SetInt32("_UserId", loginResponse.UserDetail.UserId);
                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, Convert.ToString(loginResponse.UserDetail.FirstName + " " + loginResponse.UserDetail.LastName)),
                };
                //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                var principal = new ClaimsPrincipal(identity);
                //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                });
                return RedirectToAction(TaskDashboardStaticValues.TaskList, TaskDashboardStaticValues.Account);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials");
                return View(model);
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    CreateTaskResponse createTaskResponse = new CreateTaskResponse();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    string url = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.SignUp;
                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        createTaskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(apiResponse);
                        if (createTaskResponse.Message == TaskDashboardStaticValues.SignUpMessage)
                        {
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            return View();
                        }
                    }
                }

            }
            else
            {
                return View();
            }

        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(TaskDashboardStaticValues.Login);
        }
        [Authorize]
        public async Task<IActionResult> TaskList()
        {
            List<TaskListModel> tasksList = new List<TaskListModel>();
            TasksListResponseModel tasksListResponse = new TasksListResponseModel();
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.GetAllTasks))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        tasksListResponse = JsonConvert.DeserializeObject<TasksListResponseModel>(apiResponse);
                        if (tasksListResponse != null && tasksListResponse.Message == TaskDashboardStaticValues.GetAllTasksMessage && tasksListResponse.ResponseData.Count() > 0)
                        {
                            return View(tasksListResponse.ResponseData);
                        }
                        else
                        {
                            tasksListResponse.ResponseData = tasksList;
                            return View(tasksListResponse.ResponseData);
                        }
                    }
                    else
                        tasksListResponse.ResponseData = tasksList;
                    return View(tasksListResponse.ResponseData);
                }
            }

        }
        [HttpPost]
        public async Task<ActionResult> CreateTask(AddTask addTask)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            addTask.CreatedBy = userId ?? 0;
            using (var httpClient = new HttpClient())
            {
                CreateTaskResponse createTaskResponse = new CreateTaskResponse();
                StringContent content = new StringContent(JsonConvert.SerializeObject(addTask), Encoding.UTF8, "application/json");

                string CreateTaskUrl = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.CreateTask;
                string SendNotificationUrl = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.SentNotification;
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync(CreateTaskUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    createTaskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(apiResponse);
                    if (createTaskResponse.Message == TaskDashboardStaticValues.CreateTaskMessageSuccess)
                    {
                        CreateTaskResponse taskResponse = new CreateTaskResponse();
                        NotificationRequest notificationRequest = new NotificationRequest();
                        notificationRequest.TaskId = createTaskResponse.TaskId;
                        StringContent notificationContent = new StringContent(JsonConvert.SerializeObject(notificationRequest), Encoding.UTF8, "application/json");
                        using (var notificationResponse = await httpClient.PostAsync(SendNotificationUrl, notificationContent))
                        {
                            string notificationApiResponse = await notificationResponse.Content.ReadAsStringAsync();
                            taskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(notificationApiResponse);
                            if (taskResponse.Message == TaskDashboardStaticValues.NotificationMessageSuccess)
                            {

                                return Json(TaskDashboardStaticValues.NotificationAndCreateTask);
                            }
                            else
                            {
                                return Json(TaskDashboardStaticValues.CreateTaskMessageSuccess);
                            }
                        }
                    }
                    else
                    {
                        return Json(TaskDashboardStaticValues.CreateTaskMessageFailure);
                    }
                }
            }

        }
        public async Task<JsonResult> GetDropDownList()
        {
            string apiResponse = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.GetDropDownLists))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        var dropDownResponse = JsonConvert.DeserializeObject<CreateTaskDropDownLists>(apiResponse);
                        return Json(dropDownResponse);
                    }
                    else
                        return Json(apiResponse);
                }
            }

        }
        public async Task<IActionResult> GetTasksById(int id)
        {
            TaskDetailResponse taskDetailResponse = new TaskDetailResponse();
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.GetTasksById + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        taskDetailResponse = JsonConvert.DeserializeObject<TaskDetailResponse>(apiResponse);
                        if (taskDetailResponse.Message == TaskDashboardStaticValues.GetAllTasksMessage)
                        {

                            return Json(JsonConvert.SerializeObject(taskDetailResponse));
                        }
                        else
                        {
                            return Json(TaskDashboardStaticValues.TasksMessageFailure);
                        }
                    }
                    else
                    {
                        return Json(TaskDashboardStaticValues.TasksMessageFailure);
                    }
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTask(AddTask addTask)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            addTask.ModifiedBy = userId ?? 0;
            using (var httpClient = new HttpClient())
            {
                CreateTaskResponse createTaskResponse = new CreateTaskResponse();
                StringContent content = new StringContent(JsonConvert.SerializeObject(addTask), Encoding.UTF8, "application/json");
                string url = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.UpdateTask;
                string SendNotificationUrl = _configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.SentNotification;
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    createTaskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(apiResponse);
                    if (createTaskResponse.Message == TaskDashboardStaticValues.UpdateTaskMessageSuccess)
                    {
                        CreateTaskResponse taskResponse = new CreateTaskResponse();
                        NotificationRequest notificationRequest = new NotificationRequest();
                        notificationRequest.TaskId = createTaskResponse.TaskId;
                        StringContent notificationContent = new StringContent(JsonConvert.SerializeObject(notificationRequest), Encoding.UTF8, "application/json");
                        using (var notificationResponse = await httpClient.PostAsync(SendNotificationUrl, notificationContent))
                        {
                            string notificationApiResponse = await notificationResponse.Content.ReadAsStringAsync();
                            taskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(notificationApiResponse);
                            if (taskResponse.Message == TaskDashboardStaticValues.NotificationMessageSuccess)
                            {

                                return Json(TaskDashboardStaticValues.NotificationAndUpdateTask);
                            }
                            else
                            {
                                return Json(TaskDashboardStaticValues.UpdateTaskMessageSuccess);
                            }
                        }
                    }
                    else
                    {
                        return Json(TaskDashboardStaticValues.UpdateTaskMessageFailure);
                    }
                }
            }

        }
        public async Task<IActionResult> DeleteTasksById(int id)
        {
            CreateTaskResponse createTaskResponse = new CreateTaskResponse();
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Session.GetString("_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (var response = await httpClient.DeleteAsync(_configuration["Url:UserManagementServiceUrl"] + TaskDashboardStaticValues.DeleteTasksById + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        createTaskResponse = JsonConvert.DeserializeObject<CreateTaskResponse>(apiResponse);
                        if (createTaskResponse.Message == TaskDashboardStaticValues.DeleteTasksMessage)
                        {

                            return Json(JsonConvert.SerializeObject(createTaskResponse));
                        }
                        else
                        {
                            return Json(TaskDashboardStaticValues.DeleteTaskMessageFailure);
                        }
                    }
                    else
                    {
                        return Json(TaskDashboardStaticValues.DeleteTaskMessageFailure);
                    }
                }
            }
        }
    }
}

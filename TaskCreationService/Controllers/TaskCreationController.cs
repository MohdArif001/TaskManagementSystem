using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskCreationService.Models;
using TaskCreationService.Repository.Interfaces;
using UserManagementService.Model;

namespace TaskCreationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCreationController : ControllerBase
    {
        private readonly ITaskCreation _taskCreation;
       
        public TaskCreationController(ITaskCreation taskCreation)
        {
            _taskCreation = taskCreation;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateTask")]
        public ActionResult CreateTask(TaskDetail taskDetail)
        {
            var taskData = _taskCreation.CreateTask(taskDetail);
            if (taskData!=null)
            {
                return Ok(new
                {
                    Message = "Task Created Successfully",
                    TaskId = taskData.Id
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Task Not Created",
                    TaskId = 0
                });
            }

        }

        [Authorize]
        [HttpPost]
        [Route("UpdateTask")]
        public ActionResult UpdateTask(TaskDetail taskDetail)
        {
            var taskData = _taskCreation.UpdateTask(taskDetail);
            if (taskData!=null)
            {
                return Ok(new
                {
                    Message = "Task Updated Successfully",
                    TaskId = taskData.Id
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Task Not Updated",
                    TaskId = taskData.Id
                });
            }

        }

        [Authorize]
        [HttpGet]
        [Route("GetTasksById/{id}")]
        public ActionResult GetTasksById(int id)
        {
            var getTasksById = _taskCreation.GetTasksById(id);
            if (getTasksById != null)
            {
                return Ok(new
                {
                    Message = "Tasks Fetch Successfully",
                    ResponseData = getTasksById
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "No Task found"
                });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteTasksById/{id}")]
        public ActionResult DeleteTasksById(int id)
        {
            var getTasksById = _taskCreation.DeleteTasksById(id);
            if (getTasksById)
            {
                return Ok(new
                {
                    Message = "Tasks Deleted Successfully",
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Task not deleted"
                });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetAllTasks")]
        public ActionResult GetAllTasks()
        {
            var allTasks = _taskCreation.GetAllTasks();
            if (allTasks != null)
            {
                return Ok(new
                {
                    Message = "Tasks Fetch Successfully",
                    ResponseData = allTasks
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "No Task found"
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetDropDownLists")]
        public ActionResult GetCreateTaskDropDownLists()
        {
            var allDropDownLists = _taskCreation.GetCreateTaskDropDownLists();
            if (allDropDownLists != null)
            {
                return Ok(new
                {
                    Message = "Data Fetch Successfully",
                    ResponseData = allDropDownLists
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "No Data found"
                });
            }
        }
    }
}

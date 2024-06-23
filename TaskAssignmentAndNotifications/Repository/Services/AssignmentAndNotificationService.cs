using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.Xml;
using TaskAssignmentAndNotificationService.Models;
using TaskAssignmentAndNotificationService.Repository.Interfaces;

namespace TaskAssignmentAndNotificationService.Repository.Services
{
    public class AssignmentAndNotificationService : IAssignmentAndNotification
    {
        private readonly AssignmentAndNotificationDbContext _assignmentAndNotificationDb;
        private readonly IConfiguration _configuration;
        public AssignmentAndNotificationService(AssignmentAndNotificationDbContext assignmentAndNotificationDb, IConfiguration configuration)
        {
            _assignmentAndNotificationDb = assignmentAndNotificationDb;
            _configuration = configuration;
        }
        public bool SentNotification(TaskAssign taskAssign)
        {
            try
            {
                var taskData = _assignmentAndNotificationDb.TaskDetails.Where(x => x.Id == taskAssign.TaskId).FirstOrDefault();
                if (taskData != null)
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    EmailRequest emailRequest = new EmailRequest();
                    emailRequest = SetEmailConfiguration(emailRequest);
                    emailRequest.ToEmailId = taskData.EmailId;
                    emailRequest.Subject = "[TaskId - "+taskData.Id+"]"+ " "+taskData.TaskName; 
                    
                    stringBuilder.Append(" [Project Name]:  " + taskData.ProjectName + "\n");
                    stringBuilder.Append(" [Task Name]:  " + taskData.TaskName + "\n");
                    stringBuilder.Append(" [Task Description]:  " + taskData.Description + "\n");
                    stringBuilder.Append(" [Priority]:  " + taskData.PriorityName + "\n");
                    stringBuilder.Append(" [Issue Type]:  " + taskData.IssueTypeName + "\n");
                    stringBuilder.Append(" [Status]:  " + taskData.StatusName + "\n" + "\n" + "\n");
                    stringBuilder.Append("Thanks & Regards,");
                    stringBuilder.Append("\n" + "Chetu Jira");
                    stringBuilder.Append("\n" + "Chetu Inc. - Delivering world class IT services");
                    stringBuilder.Append("\n" + "For more information , Please visit : www.chetu.com");
                    emailRequest.Body = stringBuilder.ToString();
                    var status = SendEmail(emailRequest);
                    return status;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
           
        }
        public bool SendEmail(EmailRequest emailRequest)
        {
            try
            {
                // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                var smtp = new SmtpClient
                {

                    Host = emailRequest.Host,
                    Port = 587,
                    EnableSsl = emailRequest.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = emailRequest.UseDefaultCredentials,
                    Credentials = new NetworkCredential(emailRequest.FromUserName, emailRequest.FromPassword)
                };
                using (var message = new MailMessage()
                {

                    Subject = emailRequest.Subject,
                    Body = emailRequest.Body,

                })
                {

                    message.From = new MailAddress(emailRequest.FromEmailAddress, _configuration["EmailCred:DisplayName"]);
                    message.To.Add(emailRequest.ToEmailId);
                    smtp.Send(message);
                    message.Dispose();
                }
                smtp.Dispose();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
         
        }

        public EmailRequest SetEmailConfiguration(EmailRequest email)
        {
            email.Host = _configuration["EmailCred:Host"];//"email-smtp.us-west-2.amazonaws.com";
            email.Port = Convert.ToInt32(_configuration["EmailCred:Port"]);//Convert.ToInt32("587");
            email.EnableSsl = true;
            email.FromUserName = _configuration["EmailCred:FromUserName"]; //"AKIAWDXI2D5Q4XSSNZXZ";
            email.FromEmailAddress = _configuration["EmailCred:FromEmailAddress"];//"noreply@banktechpr.com";
            email.FromPassword = _configuration["EmailCred:FromPassword"];//"BA0JbQCMWiUFs7rGDs/f6KWWHSwARxuRUbm3izNHhm5I";
            return email;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TaskCreationService.Models;
using TaskCreationService.Repository.Interfaces;

namespace TaskCreationService.Repository.Services
{
    public class TaskCreationServices : ITaskCreation
    {
        private readonly TaskCreationDbContext _taskCreationDb;
        public TaskCreationServices(TaskCreationDbContext taskCreationDb)
        {

            _taskCreationDb = taskCreationDb;
        }
        public TaskDetail CreateTask(TaskDetail task)
        {
            try
            {
                task.CreatedDate = DateTime.UtcNow;
                task.ModifiedDate = DateTime.UtcNow;
                task.IsDeleted = false;
                _taskCreationDb.TaskDetails.Add(task);
                _taskCreationDb.SaveChanges();

                return task;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<VTaskDetail> GetAllTasks()
        {
            try
            {
                var tasks = _taskCreationDb.VTaskDetails.ToList();
                return tasks;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public TaskDetail GetTasksById(int id)
        {
            try
            {
                var task = _taskCreationDb.TaskDetails.Where(x => x.Id == id).FirstOrDefault();
                return task;
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public bool DeleteTasksById(int id)
        {
            try
            {
                var task = _taskCreationDb.TaskDetails.Where(x => x.Id == id).FirstOrDefault();
                if (task != null)
                {
                    task.IsDeleted = true;
                }
                var isdeleted = _taskCreationDb.SaveChanges();
                if (isdeleted > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        public CreateTaskDropDownLists GetCreateTaskDropDownLists()
        {
            try
            {
                CreateTaskDropDownLists createTaskDropDownLists = new CreateTaskDropDownLists();
                var project = _taskCreationDb.Projects.ToList();
                var priority = _taskCreationDb.Priority.ToList();
                var status = _taskCreationDb.Status.ToList();
                var issueType = _taskCreationDb.IssueType.ToList();
                var assignee = _taskCreationDb.VUserName.ToList();
                createTaskDropDownLists.Projects = project;
                createTaskDropDownLists.Priorities = priority;
                createTaskDropDownLists.Statuses = status;
                createTaskDropDownLists.IssueTypes = issueType;
                createTaskDropDownLists.Assignees = assignee;

                return createTaskDropDownLists;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public TaskDetail UpdateTask(TaskDetail task)
        {
            try
            {
                var taskDetail = _taskCreationDb.TaskDetails.Where(x => x.Id == task.Id).FirstOrDefault();
                if (taskDetail != null)
                {
                    taskDetail.PriorityId = task.PriorityId;
                    taskDetail.IssueTypeId = task.IssueTypeId;
                    taskDetail.ProjectId = task.ProjectId;
                    taskDetail.StatusId = task.StatusId;
                    taskDetail.AssignedTo = task.AssignedTo;
                    taskDetail.Description = task.Description;
                    taskDetail.TaskName = task.TaskName;
                    taskDetail.ModifiedBy = task.ModifiedBy;
                    taskDetail.ModifiedDate = DateTime.UtcNow;
                }
                _taskCreationDb.SaveChanges();
                return taskDetail;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}

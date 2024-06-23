using TaskCreationService.Models;

namespace TaskCreationService.Repository.Interfaces
{
    public interface ITaskCreation
    {
        public TaskDetail CreateTask(TaskDetail task);
        public TaskDetail UpdateTask(TaskDetail task);
        public List<VTaskDetail> GetAllTasks();
        public TaskDetail GetTasksById( int id);
        public bool DeleteTasksById( int id);
        public CreateTaskDropDownLists GetCreateTaskDropDownLists();
    }
}

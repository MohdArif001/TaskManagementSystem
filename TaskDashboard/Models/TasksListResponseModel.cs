namespace TaskDashboard.Models
{
    public class TasksListResponseModel
    {
        public string Message { get; set; }
        public List<TaskListModel> ResponseData { get; set; }
    }
}

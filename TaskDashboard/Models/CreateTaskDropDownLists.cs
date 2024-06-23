namespace TaskDashboard.Models
{
    public class CreateTaskDropDownLists
    {
        public string message { get; set; }
        public ResponseData responseData { get; set; }
    }
    public class IssueType
    {
        public int id { get; set; }
        public string issueTypeName { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string priorityName { get; set; }
    }

    public class Project
    {
        public int id { get; set; }
        public string projectName { get; set; }
    }

    public class ResponseData
    {
        public List<Project> projects { get; set; }
        public List<Priority> priorities { get; set; }
        public List<Status> statuses { get; set; }
        public List<IssueType> issueTypes { get; set; }
        public List<VUserName> assignees { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string statusName { get; set; }
    }
    public class VUserName
    {
        public int Id { get; set; }
        public string userName { get; set; }
    }
}

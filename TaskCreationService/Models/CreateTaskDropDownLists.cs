namespace TaskCreationService.Models
{
    public class CreateTaskDropDownLists
    {
        public List<Project> Projects { get; set; }
        public List<Priority> Priorities { get; set; }
        public List<Status> Statuses { get; set; }
        public List<IssueType> IssueTypes { get; set; }
        public List<VUserName> Assignees { get; set; }
        
    }
}

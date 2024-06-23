

namespace TaskCreationService.Models
{
    public class VTaskDetail
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public string IssueTypeName { get; set; }
        public string ProjectName { get; set; }
        public string PriorityName { get; set; }
        public string AssignedTo { get; set; }
        public string Reporter { get; set; }
        public string EmailId { get; set; }
    }
}

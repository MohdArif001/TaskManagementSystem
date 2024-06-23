using System.ComponentModel.DataAnnotations;

namespace TaskCreationService.Models
{
    public class TaskDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int StatusId { get; set; }
        public int IssueTypeId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityId { get; set; }
        public int AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

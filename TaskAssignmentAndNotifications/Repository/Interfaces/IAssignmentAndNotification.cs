using TaskAssignmentAndNotificationService.Models;

namespace TaskAssignmentAndNotificationService.Repository.Interfaces
{
    public interface IAssignmentAndNotification
    {
        public bool SentNotification(TaskAssign taskAssign);
    }
}

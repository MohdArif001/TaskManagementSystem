namespace TaskAssignmentAndNotificationService.Models
{
    public class EmailRequest
    {
        /// <summary>
        /// get or set FromEmailId value
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// get or set ToEmailId value
        /// </summary>
        public string ToEmailId { get; set; }

        /// <summary>
        /// get or set ToEmailId value
        /// </summary>
        public string ToCCEmailId { get; set; }

        /// <summary>
        /// get or set ToEmailId value
        /// </summary>
        public string ToBCCEmailId { get; set; }

        /// <summary>
        /// get or set FromPassword value
        /// </summary>
        public string FromPassword { get; set; }

        /// <summary>
        /// get or set Subject value
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// get or set Body value
        /// </summary>
        public string Body { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; } = false;
        public string FromEmailAddress { get; set; }
        public string Attachments { get; set; }
    }
}

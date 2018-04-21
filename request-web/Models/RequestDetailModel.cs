using System;
using request_web.WebService;

namespace request_web.Models
{
    public class RequestDetailModel
    {
        public int RequestId { get; set; }
        public string Address { get; set; }
        public string Creator { get; set; }
        public string Worker { get; set; }
        public string Executer { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? WorkDate { get; set; }
        public string WorkPeriod { get; set; }
        public string ParentService { get; set; }
        public string Service { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Contacts { get; set; }
        public string Rating { get; set; }
        public string RatingDescription { get; set; }
        public WebCallsDto[] CallList { get; set; }
        public AttachmentDto[] Attachments { get; set; }
        public NoteDto[] Notes { get; set; }
        public bool Garanty { get; set; }
    }
}
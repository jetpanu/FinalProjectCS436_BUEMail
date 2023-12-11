namespace FinalProject.Areas.Identity.Data
{
    public class EmailInbox
    {
        public int Id { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailMessage { get; set; }
        public DateTime? EmailDate { get; set; }
        public bool? EmailIsRead { get; set; }
        public string? EmailSender { get; set; } // my email
        public string? EmailReceiver { get; set; } // ematil to
        public bool? EmailIsDeleted { get; set; }
    }
}

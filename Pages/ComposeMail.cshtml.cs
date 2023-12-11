using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class ComposeMailModel : PageModel
    {
        [BindProperty]
        public string Recipient { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Body { get; set; }

        private readonly IEmailSender _emailSender;
        private readonly ILogger<ComposeMailModel> _logger;

        public ComposeMailModel(IEmailSender emailSender, ILogger<ComposeMailModel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var emailContent = $"Subject: {Subject}\n\n{Body}";
                var emailSent = SendEmail(Recipient, Subject, emailContent);

                if (emailSent)
                {
                    SaveEmailToDatabase(Recipient, Subject, Body);

                    _logger.LogInformation($"Email sent to {Recipient} with subject: {Subject}");
                    TempData["Message"] = "Email sent successfully";
                    return RedirectToPage("/Index"); 
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to send email. Please try again.");
                }
            }

            return Page();
        }

        private bool SendEmail(string recipient, string subject, string content)
        {
            try
            {
                _emailSender.SendEmailAsync(recipient, subject, content).Wait();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                return false;
            }
        }


        private void SaveEmailToDatabase(string recipient, string subject, string body)
        {
            try
            {
                String connectionString = "Server=tcp:buemail436.database.windows.net,1433;Initial Catalog=BUEMAIL;Persist Security Info=False;User ID=admindeklnw;Password=Deklnwadmin-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string senderName = User.Identity.IsAuthenticated ? User.Identity.Name  : "Anonymous";

                    String sql = "INSERT INTO EMAILINBOX (EmailSubject, EmailMessage, EmailDate, EmailIsRead, EmailSender, EmailReceiver) VALUES (@Subject, @Body, GETDATE(), 0, @Sender, @Recipient)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Subject", subject);
                        command.Parameters.AddWithValue("@Body", body);
                        command.Parameters.AddWithValue("@Sender", senderName);
                        command.Parameters.AddWithValue("@Recipient", recipient);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving email to database: {ex.Message}");
            }
        }

    }


}

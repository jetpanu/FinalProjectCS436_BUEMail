using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {

        public List<EmailInbox> listEmails = new List<EmailInbox>();

        private readonly ILogger<IndexModel> _logger;
        public UserManager<FinalProjectUser> _userManager;
        public IndexModel(ILogger<IndexModel> logger, 
            UserManager<FinalProjectUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async void OnGet()
        {
            try
            {
                if (TempData.ContainsKey("Message"))
                {
                    ViewData["Message"] = TempData["Message"].ToString();
                }

                if (User.Identity.IsAuthenticated)
                {
                    String connectionString = "Server=tcp:buemail436.database.windows.net,1433;Initial Catalog=BUEMAIL;Persist Security Info=False;User ID=admindeklnw;Password=Deklnwadmin-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string username = "";
                        if (User.Identity.Name == null)
                        {
                            username = "";

                        }
                        else
                        {
                            username = User.Identity.Name;
                        }

                        String sql = "SELECT * FROM EMAILINBOX WHERE emailreceiver = '"+username+"' AND EmailIsDeleted IS NULL";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    EmailInbox emailInfo = new EmailInbox();

                                    emailInfo.Id = reader.GetInt32(0);
                                    emailInfo.EmailSubject = reader.GetString(1);
                                    emailInfo.EmailMessage = reader.GetString(2);
                                    emailInfo.EmailDate = reader.GetDateTime(3);
                                    emailInfo.EmailIsRead = reader.GetBoolean("EmailIsRead") == true;
                                   // emailInfo.EmailIsDeleted = reader.GetValue("EmailIsDeleted").ToString() == null;
                                    emailInfo.EmailSender = reader.GetString(6) ?? null;
                                    emailInfo.EmailReceiver = reader.GetString(7) ?? null;

                                    listEmails.Add(emailInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        
    }
    public class EmailInfo
    {
        public String EmailID;
        public String EmailSubject;
        public String EmailMessage;
        public String EmailDate;
        public String EmailIsRead;
        public String EmailSender;
        public String EmailReceiver;
    }



}

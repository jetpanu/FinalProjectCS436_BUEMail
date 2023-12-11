using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

public class DeleteEmailModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int EmailID { get; set; }

    public IActionResult OnGet()
    {
        try
        {
            string connectionString = "Server=tcp:buemail436.database.windows.net,1433;Initial Catalog=BUEMAIL;Persist Security Info=False;User ID=admindeklnw;Password=Deklnwadmin-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE EMAILINBOX SET EmailIsDeleted = 1 WHERE id = @emailid";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@emailid", EmailID);

                    command.ExecuteNonQuery();
                }
            }
            TempData["Message"] = "Delete email successfully";
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            return RedirectToPage("/Index");
        }


        return Redirect("Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            string connectionString = "Server=tcp:buemail436.database.windows.net,1433;Initial Catalog=BUEMAIL;Persist Security Info=False;User ID=admindeklnw;Password=Deklnwadmin-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string sql = "DELETE FROM Emails WHERE EmailID = @emailid";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@emailid", EmailID);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            return Page();
        }
    }
}

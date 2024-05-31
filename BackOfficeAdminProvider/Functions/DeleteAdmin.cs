using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackOfficeAdminProvider.Functions
{
    public class DeleteAdmin(ILogger<DeleteAdmin> logger, DataContext context)
    {
        private readonly ILogger<DeleteAdmin> _logger = logger;
        private readonly DataContext _context = context;

        [Function("DeleteAdmin")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete", "post", Route = "silicon/admins/delete/{email}" )] HttpRequest req, string email)
        {
            _logger.LogInformation("DeleteUser function processed a request.");

            try
            {
                var admin = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                if (admin == null)
                {
                    return new NotFoundResult();
                }

                _context.Users.Remove(admin);
                await _context.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error deleting user: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

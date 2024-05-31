using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BackOfficeAdminProvider.Functions
{
    public class UpdateAdmin(ILogger<UpdateAdmin> logger, DataContext context)
    {
        private readonly ILogger<UpdateAdmin> _logger = logger;
        private readonly DataContext _context = context;

        [Function("UpdateAdmin")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", "post", Route = "silicon/admin/update/{email}")] HttpRequest req, string email)
        {
            _logger.LogInformation("UpdateUser function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdateUserModel updateUserModel;

            try
            {
                updateUserModel = JsonSerializer.Deserialize<UpdateUserModel>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult("Invalid JSON data.");
            }

            if (updateUserModel == null || updateUserModel.Email == null)
            {
                return new BadRequestObjectResult("Invalid user data or email mismatch.");
            }

            var admin = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (admin == null)
            {
                return new NotFoundResult();
            }

            admin.Email = updateUserModel.Email;
            admin.Role = updateUserModel.Role;

            try
            {
                _context.Users.Update(admin);
                await _context.SaveChangesAsync();
                return new OkObjectResult(admin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public class UpdateUserModel
        {
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
    


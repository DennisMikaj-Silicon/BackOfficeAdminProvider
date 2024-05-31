using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BackOfficeAdminProvider.Functions
{
    public class GetAdmins(ILogger<GetAdmins> logger, DataContext context)
    {
        private readonly ILogger<GetAdmins> _logger = logger;
        private readonly DataContext _context = context;

        [Function("GetAdmins")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,"get", "post")] HttpRequest req)
        {
            var admins = await _context.Users.ToListAsync();
            return new OkObjectResult(admins);
        }
    }
}

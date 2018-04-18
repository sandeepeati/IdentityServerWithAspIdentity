using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerWithAspIdentity.Data;
using IdentityServerWithAspIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerWithAspIdentity.Controllers
{
    [Authorize]
    [Route("usermanagement")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            var result = new List<ApplicationUser>();

            foreach (var appuser in users)
            {
                var user = new ApplicationUser
                {
                    Id = appuser.Id,
                    UserName = appuser.UserName,
                    Email = appuser.Email,
                    TenantId = appuser.TenantId
                };
                result.Add(user);
            }

            return Ok(result);
        }
    }
}
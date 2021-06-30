using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using secure_approve.Models.localdb;

namespace secure_approve.Controllers
{
    public class AccountController : Controller
    {
        private readonly secureApproveDBContext _context;

        public AccountController(secureApproveDBContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {           
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var curUser = _context.AppUsers.Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim()).Include(e => e.UserRole).SingleOrDefault();
                if (curUser != null)
                {
                    var curPassHash = ComputeSha256Hash(password.Trim());
                    ClaimsIdentity identity = null;
                    bool isAuthenticated = false;

                    if (curPassHash == curUser.Userpass)
                    {
                        identity = new ClaimsIdentity(
                            new[] { new Claim(ClaimTypes.Name, curUser.Username), new Claim(ClaimTypes.Role, curUser.UserRole.Rolename) }
                            , CookieAuthenticationDefaults.AuthenticationScheme
                            );
                        isAuthenticated = true;
                    }

                    if (isAuthenticated)
                    {
                        var curPrincipal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, curPrincipal);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account", new { errorMessage = "Login Failed"});
                    }
                }
            }
            return View();
        }


        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

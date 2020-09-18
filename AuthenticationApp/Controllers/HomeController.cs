using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }


        [Authorize(Policy = "Claim.Dob")]
        public IActionResult SecretDateOfBirth()
        {
            return View("Secret");
        }

        //[Authorize(Roles = "professor")]
        //[Authorize(Roles = "Administrator")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Professor")]
        public IActionResult ProfessorView()
        {
            return View();
        }

        [Authorize(Roles = "student")]
        [Authorize(Roles = "Administrator")]
        public IActionResult StudentView()
        {
            return View();
        }

        public IActionResult Authenticate()
        {

            var professorClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Sergio Pastor Ontiveros Pérez"),
                new Claim(ClaimTypes.Email, "sergio@mail.com"),
                new Claim(ClaimTypes.DateOfBirth, "27/12/1987"),
                new Claim(ClaimTypes.Role, "Professor")
            };

            var studentClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Cosme Fulanito"),
                new Claim(ClaimTypes.Email, "cosme@mail.com"),
                new Claim(ClaimTypes.Role, "student")
            };

            var adminClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Sergio Alberto Ontiveros Soto"),
                new Claim(ClaimTypes.Email, "saos@mail.com"),
                new Claim(ClaimTypes.Role, "Administrator")
            };


            var professorIdentity = new ClaimsIdentity(professorClaim, "Professor Identity");
            var studentIdentity = new ClaimsIdentity(studentClaim, "Student Identity");
            var adminIdentity = new ClaimsIdentity(adminClaim, "Administrator Identity");

            var usersPrincipal = new ClaimsPrincipal(new[] { professorIdentity, studentIdentity, adminIdentity });

            

            HttpContext.SignInAsync(usersPrincipal);

            return RedirectToAction("Index");
        }
    }
}

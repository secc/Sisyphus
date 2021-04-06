using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sisyphus.Utilities;
using Sisyphus.ViewModels;

namespace Sisyphus.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        // POST: Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login( UserLogin userLogin )
        {
            try
            {
                var claims = RockAuthentication.AuthenticateUser( userLogin );
                if (claims != null && claims.Any() )
                {
                    await HttpContext.SignInAsync( new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", "user", "role" ) ) );
                }

                return Redirect( "/" );
            }
            catch
            {
                return View();
            }
        }



    }
}

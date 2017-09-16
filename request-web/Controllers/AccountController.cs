using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Highsoft.Web.Mvc.Charts;
using Newtonsoft.Json;
using request_web.Models;
using request_web.WebService;

namespace request_web.Controllers
{
    public class AccountController : Controller
    {
        private readonly RequestWebServiceClient _requestService;

        public AccountController()
        {
            _requestService = new RequestWebServiceClient();
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var roles = _requestService.Login(model.UserName, model.Password);
                if (roles != null)
                {
                    var userInfo = JsonConvert.SerializeObject(roles);
                    FormsAuthentication.SetAuthCookie(userInfo, false);

                    var t = FormsAuthentication.GetAuthCookie(userInfo, false);
                   
                    return Redirect(returnUrl??Url.Action( "List", "Requests"));
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Redirect(Url.Action("Login", "Account"));
        }
    }
}
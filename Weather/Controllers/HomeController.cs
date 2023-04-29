using Microsoft.AspNetCore.Mvc;
using Weather.Web.Controllers.Base;

namespace AdminPanel.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using SMB.Models.Autentification;
using SMB.Models.DataBases;
using System.Web;
using System.Web.Mvc;

namespace SMB.Controllers
{
    public class HomeController : Controller
    {
        private SMBContext _db = new SMBContext();

        public ActionResult Index()
        {
            var cookie = Request.Cookies.Get("SMB_AU");
            if (cookie != null)
                ViewBag.HasCookie = true;
            else
                ViewBag.HasCookie = false;
            return View();
        }

        public ActionResult Autorisation(string login, string password)
        {
            IProfileManager profileManager = new HashedProfileManager(_db);
            IProfile profile = profileManager.ReturnProfileFromDB(login, password);
            if(profile.Rank != ProfileRank.Null)
            {
                var cookie = new HttpCookie("SMB_AU");
                Response.Cookies.Add(cookie);
            }
            return new RedirectResult("~/Home/Index");
        }
    }
}
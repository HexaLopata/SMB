using SMB.Models.Autentification;
using SMB.Models.DataBases;
using System.Web;
using System.Web.Mvc;

namespace SMB.Controllers
{
    public class HomeController : Controller
    {
        private readonly SMBContext _db = new SMBContext();
        private readonly ICookieManager _cookieManager = new CookieManager();

        public ActionResult Index()
        {
            ViewBag.HasCookie = _cookieManager.CheckForAutorisationCookie(Request);
            return View();
        }

        public ActionResult Autorisation(string login, string password)
        {
            IProfileManager profileManager = new HashedProfileManager(_db);
            IProfile profile = profileManager.ReturnProfileFromDB(login, password);
            if(profile.Rank != ProfileRank.Null)
            {
                _cookieManager.CreateAndAddAutorisationCookie(Response);
            }
            return new RedirectResult("~/Home/Index");
        }
    }
}
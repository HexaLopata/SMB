using SMB.Models.Links;
using System.Web.Mvc;

namespace SMB.Controllers
{
    public class LinksController : Controller
    {
        private LinkContext linkDB = new LinkContext();

        public ActionResult Index()
        {
            var subjects = linkDB.Subjects;
            return View(subjects);
        }

        public ActionResult Explore(int id)
        {
            var subject = linkDB.Subjects.Find(id);
            var topics = subject.Topics;
            return View(topics);
        }
    }
}
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
            ViewBag.SubjectId = subject.Id;
            return View(topics);
        }

        [HttpPost]
        public RedirectResult AddTopic(int id, string topicName)
        {
            linkDB.Topics.Add(new Topic() { SubjectId = id, Name = topicName });
            linkDB.SaveChanges();

            return RedirectPermanent("~/Links/Explore/" + id);
        }

        [HttpPost]
        public RedirectResult AddLink(int id, string linkName, string linkContent)
        {
            linkDB.Links.Add(new Link() { TopicId = id, Name = linkName, Content = linkContent });
            linkDB.SaveChanges();

            var topic = linkDB.Topics.Find(id);

            return RedirectPermanent("~/Links/Explore/" + topic.SubjectId);
        }
    }
}
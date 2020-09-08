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
            if (subject != null)
            {
                var topics = subject.Topics;
                ViewBag.SubjectId = subject.Id;
                return View(topics);
            }
            else
                return RedirectPermanent("~/Links/Index/");
        }

        [HttpPost]
        public RedirectResult AddTopic(int id, string topicName)
        {
            var subject = linkDB.Subjects.Find(id);
            if (subject != null)
            {
                if (topicName.Trim() != string.Empty)
                {
                    subject.Topics.Add(new Topic() { SubjectId = id, Name = topicName });
                    linkDB.SaveChanges();
                }

                return RedirectPermanent("~/Links/Explore/" + id);
            }

            return RedirectPermanent("~/Links/Index/");
        }

        [HttpPost]
        public RedirectResult AddLink(int id, string linkName, string linkContent)
        {
            var topic = linkDB.Topics.Find(id);
            if (topic != null)
            {
                if (linkName.Trim() != string.Empty)
                {
                    topic.Links.Add(new Link() { TopicId = id, Name = linkName, Content = linkContent });
                    linkDB.SaveChanges();
                }
                return RedirectPermanent("~/Links/Explore/" + topic.SubjectId);
            }

            return RedirectPermanent("~/Links/Index/");
        }

        [HttpPost]
        public RedirectResult AddSubject(string subjectName)
        {
            if (subjectName != null)
            {
                if (subjectName.Trim() != string.Empty)
                {
                    linkDB.Subjects.Add(new Subject() { Name = subjectName });
                    linkDB.SaveChanges();
                }
            }

            return RedirectPermanent("~/Links/Index/");
        }

        [HttpPost]
        public RedirectResult DeleteLink(int linkId)
        {
            var link = linkDB.Links.Find(linkId);
            if (link != null)
            {
                var subjectId = link.Topic.SubjectId;
                linkDB.Links.Remove(link);
                linkDB.SaveChanges();

                return RedirectPermanent("~/Links/Explore/" + subjectId);
            }
            else
            {
                return RedirectPermanent("~/Links/Index/");
            }
        }

        [HttpPost]
        public RedirectResult DeleteTopic(int topicId)
        {
            var topic = linkDB.Topics.Find(topicId);
            if (topic != null)
            {
                var subjectId = topic.SubjectId;
                linkDB.Links.RemoveRange(topic.Links);
                linkDB.Topics.Remove(topic);
                linkDB.SaveChanges();

                return RedirectPermanent("~/Links/Explore/" + subjectId);
            }
            else
            {
                return RedirectPermanent("~/Links/Index/");
            }
        }

        [HttpPost]
        public RedirectResult DeleteSubject(int subjectId)
        {
            var subject = linkDB.Subjects.Find(subjectId);
            if (subject != null)
            {
                var topics = subject.Topics;
                foreach(Topic topic in topics)
                {
                    linkDB.Links.RemoveRange(topic.Links);
                }
                linkDB.Topics.RemoveRange(topics);
                linkDB.Subjects.Remove(subject);
                linkDB.SaveChanges();
            }
                return RedirectPermanent("~/Links/Index/");
        }
    }
}
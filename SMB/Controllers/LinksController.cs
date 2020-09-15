using SMB.Models.Links;
using System.Data.Entity;
using System.Web.Mvc;

namespace SMB.Controllers
{
    public class LinksController : Controller
    {
        private LinkContext linkDB = new LinkContext();
        private readonly IDataBaseManager linkDataBaseManager = new LinkDataBaseManager();

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
            return AddObjectToDataBaseAndRedirect(new Topic() { SubjectId = id, Name = topicName }, linkDB.Topics,
                                                              topicName, subject);
        }

        [HttpPost]
        public RedirectResult AddLink(int id, string linkName, string linkContent)
        {
            var topic = linkDB.Topics.Find(id);

            return AddObjectToDataBaseAndRedirect(new Link() { TopicId = id, Name = linkName, Content = linkContent }, linkDB.Links,
                                                             linkName, topic);
        }

        [HttpPost]
        public RedirectResult AddSubject(string subjectName)
        {
            return AddObjectToDataBaseAndRedirect(new Subject() { Name = subjectName }, linkDB.Subjects, subjectName, subjectName);
        }

        [HttpPost]
        public RedirectResult DeleteLink(int linkId)
        {
            return DeleteModelAndRedirect(linkId, linkDB.Links, linkDB);
        }

        [HttpPost]
        public RedirectResult DeleteTopic(int topicId)
        {
            return DeleteModelAndRedirect(topicId, linkDB.Topics, linkDB);
        }

        [HttpPost]
        public RedirectResult DeleteSubject(int subjectId)
        {
            return DeleteModelAndRedirect(subjectId, linkDB.Subjects, linkDB);
        }

        private RedirectResult DeleteModelAndRedirect<T>(int id, DbSet<T> modelType, DbContext db) where T : class
        {
            var result = linkDataBaseManager.DeleteObjectById(id, modelType, db);
            if (result)
                return RedirectPermanent(HttpContext.Request.ServerVariables["HTTP_REFERER"]);
            return RedirectPermanent("~/Links/Index/");
        }

        private RedirectResult AddObjectToDataBaseAndRedirect<T>(T obj, DbSet<T> modelSet, string nameForCheck, object objForCheckOnNull) where T : class
        {
            if (objForCheckOnNull != null)
            {
                if (nameForCheck.Trim() != string.Empty)
                {
                    linkDataBaseManager.AddObjectInDataBase(obj, modelSet, linkDB);
                }
                return RedirectPermanent(HttpContext.Request.ServerVariables["HTTP_REFERER"]);
            }

            return RedirectPermanent("~/Links/Index/");
        }
    }
}
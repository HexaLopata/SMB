﻿using SMB.Models.Autentification;
using SMB.Models.DataBases;
using SMB.Models.Links;
using System.Data.Entity;
using System.Web.Mvc;

namespace SMB.Controllers
{
    public class LinksController : Controller
    {
        private readonly SMBContext _db = new SMBContext();
        private readonly ICookieManager _cookieManager = new CookieManager(); 
        private readonly ILinkDataBaseManager linkDataBaseManager = new LinkDataBaseManager();

        public ActionResult Index()
        {
            ViewBag.HasCookie = _cookieManager.CheckForAutorisationCookie(Request);

            var subjects = _db.Subjects;
            return View(subjects);
        }

        public ActionResult Explore(int id)
        {
            ViewBag.HasCookie = _cookieManager.CheckForAutorisationCookie(Request);

            var subject = _db.Subjects.Find(id);
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
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                var subject = _db.Subjects.Find(id);
                return AddObjectToDataBaseAndRedirect(new Topic() { SubjectId = id, Name = topicName }, _db.Topics,
                                                                  topicName, subject);
            }
            return null;
        }

        [HttpPost]
        public RedirectResult AddLink(int id, string linkName, string linkContent)
        {
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                var topic = _db.Topics.Find(id);

                return AddObjectToDataBaseAndRedirect(new Link() { TopicId = id, Name = linkName, Content = linkContent }, _db.Links,
                                                                 linkName, topic);
            }
            return null;
        }

        [HttpPost]
        public RedirectResult AddSubject(string subjectName)
        {
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                return AddObjectToDataBaseAndRedirect(new Subject() { Name = subjectName }, _db.Subjects, subjectName, subjectName);
            }
            return null;
        }

        [HttpPost]
        public RedirectResult DeleteLink(int linkId)
        {
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                return DeleteModelAndRedirect(linkId, _db.Links, _db);
            }
            return null;
        }

        [HttpPost]
        public RedirectResult DeleteTopic(int topicId)
        {
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                return DeleteModelAndRedirect(topicId, _db.Topics, _db);
            }
            return null;
        }

        [HttpPost]
        public RedirectResult DeleteSubject(int subjectId)
        {
            var cookie = Request.Cookies.Get(_cookieManager.AutorizationCookieName);
            if (cookie != null)
            {
                return DeleteModelAndRedirect(subjectId, _db.Subjects, _db);
            }
            return null;
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
                    linkDataBaseManager.AddObjectInDataBase(obj, modelSet, _db);
                }
                return RedirectPermanent(HttpContext.Request.ServerVariables["HTTP_REFERER"]);
            }

            return RedirectPermanent("~/Links/Index/");
        }
    }
}
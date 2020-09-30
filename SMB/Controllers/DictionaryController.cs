using SMB.Models.DataBases;
using SMB.Models.Dictionary;
using System.Web.Mvc;
using System;
using System.Linq;

namespace SMB.Controllers
{
    public class DictionaryController : Controller
    {
        private IWordManager _wordManager;
        private SMBContext _db;

        private const int _wordsOnPage = 20;

        public DictionaryController()
        {
            _db = new SMBContext();
            _wordManager = new WordManager(_db);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddWord(string firstLanguage, string secondLanguage, string word, string translation)
        {
            if (firstLanguage != null && secondLanguage != null)
            {
                if (!(string.IsNullOrEmpty(word.Trim()) || string.IsNullOrEmpty(translation.Trim())))
                {
                    if (word.Trim() != translation.Trim() && firstLanguage != secondLanguage)
                    {
                        if (Enum.IsDefined(typeof(Language), firstLanguage) &&
                            Enum.IsDefined(typeof(Language), secondLanguage))
                        {
                            _wordManager.AddWordInDB
                            (
                                new Word() { Language = firstLanguage, Value = word },
                                new Word() { Language = secondLanguage, Value = translation }
                            );
                        }
                    }
                }
            }

            return RedirectPermanent("~/Dictionary/Index");
        }

        public ActionResult Show(int pageNumber = 1)
        {
            IQueryable<Word> words = _db.Words;
            // Формула количества страниц учитывая количество элементов
            ViewBag.PageCount = words.Count() / _wordsOnPage + (words.Count() % _wordsOnPage == 0 ? 0 : 1);
            words = words.OrderByDescending(w => w.Id)
                         .Skip((pageNumber - 1) * _wordsOnPage)
                         .Take(_wordsOnPage);

            return View(words.ToList());
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult WordDetail(int id)
        {
            return View(_db.Words.Find(id));
        }
    }
}
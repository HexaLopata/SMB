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

        public ActionResult Show(int pageNumber = 1, string language = "", string contains = "")
        {
            IQueryable<Word> words = _db.Words;
            // Формула количества страниц учитывая количество элементов
            if (!string.IsNullOrEmpty(language) && Enum.IsDefined(typeof(Language), language))
            {
                words = words.Where(w => w.Language.ToString() == language);
            }
            if(!string.IsNullOrEmpty(contains))
            {
                words = words.Where(w => w.Value.Contains(contains));
            }

            ViewBag.PageCount = words.Count() / _wordsOnPage + (words.Count() % _wordsOnPage == 0 ? 0 : 1);
            words = words.OrderByDescending(w => w.Id)
                         .Skip((pageNumber - 1) * _wordsOnPage)
                         .Take(_wordsOnPage);
            ViewBag.Language = language;
            ViewBag.Contains = contains;

            return View(words.ToList());
        }

        [HttpPost]
        public ActionResult DeleteWord(int id)
        {
            var word = _db.Words.Find(id);
            _db.Words.Remove(word);
            _db.SaveChanges();
            return RedirectPermanent("~/Dictionary/Show/");
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
using SMB.Models.DataBases;
using SMB.Models.Dictionary;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Text;

namespace SMB.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IWordManager _wordManager;
        private readonly SMBContext _db;

        private const int _wordsOnPage = 20;

        public DictionaryController()
        {
            _db = new SMBContext();
            _wordManager = new WordManager(_db);
        }

        public ActionResult AddWordMenu(string firstLanguage = "Russian", string secondLanguage = "English")
        {
            ViewBag.FirstLanguage = firstLanguage;
            ViewBag.SecondLanguage = secondLanguage;
            var cookie = Request.Cookies.Get("SMB_AU");
            if (cookie != null)
                return View();
            else
                return new RedirectResult($"~/Dictionary/Test");
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

            return RedirectPermanent($"~/Dictionary/AddWordMenu?firstLanguage={firstLanguage}&secondLanguage={secondLanguage}");
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

            ViewBag.PageCount = words.Count() / _wordsOnPage +
                                (words.Count() % _wordsOnPage == 0 ? 0 : 1);
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

        [HttpPost]
        public ActionResult DeleteWordConnection(int wordId, int translationId, int meaningId)
        {
            var word = _db.Words.Find(wordId);
            var translation = _db.Words.Find(translationId);
            var meaning = _db.Meanings.Find(meaningId);
            _wordManager.DeleteWordConnectionFromDB(word, translation, meaning);
            _db.SaveChanges();
            return RedirectPermanent(HttpContext.Request.ServerVariables["HTTP_REFERER"]);
        }

        private string GetRandomWord(string language, string secondLanguage)
        {
            var wordArray = _db.Words.Where(w => w.Language == language)
                                     .Where(w => w.Meanings.SelectMany(m => m.Words)
                                                            .Where(t => t.Language == secondLanguage)
                                                            .Count() > 0).ToArray();
            if (wordArray.Length != 0)
            {
                return wordArray[new Random().Next(0, wordArray.Length)].Value;
            }
            else
                return string.Empty;
        }

        private string GetTranslation(string word, string firstLanguage, string secondLanguage)
        {
            var currentWord = _db.Words.FirstOrDefault(w => w.Value == word.Trim() && w.Language == firstLanguage);
            var sb = new StringBuilder();
            var translations = currentWord.Meanings.SelectMany(m => m.Words)
                                                   .Where(w => w.Language == secondLanguage);
            foreach(Word word1 in translations)
            {
                sb.Append(word1.Value + " ");
            }
            return sb.ToString().Trim();     
        }

        public ActionResult Test(string firstLanguage = "Russian", string secondLanguage = "English", string word = "")
        {
            ViewBag.FirstLanguage = firstLanguage;
            ViewBag.SecondLanguage = secondLanguage;
            ViewBag.PreviousWord = word;
            ViewBag.RandomWord = GetRandomWord(firstLanguage, secondLanguage);

            if (word != string.Empty)
            {
                ViewBag.Translation = GetTranslation(word, firstLanguage, secondLanguage);
            }
            else ViewBag.Translation = string.Empty;

            return View();
        }

        public ActionResult WordDetail(int id)
        {
            return View(_db.Words.Find(id));
        }
    }
}
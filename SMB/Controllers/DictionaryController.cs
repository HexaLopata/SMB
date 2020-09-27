using SMB.Models.DataBases;
using SMB.Models.Dictionary;
using System.Web.Mvc;
using System;

namespace SMB.Controllers
{
    public class DictionaryController : Controller
    {
        private IWordManager _wordManager = new WordManager(new SMBContext());

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

            return RedirectPermanent("~/Dictionary/Index");
        }
    }
}
using SMB.Models.DataBases;
using System.Collections.Generic;
using System.Linq;

namespace SMB.Models.Dictionary
{
    public class WordManager : IWordManager
    {
        private SMBContext _db;

        public WordManager(SMBContext db)
        {
            _db = db;
        }

        public void AddWordInDB(Word firstWord, Word translation)
        {
            // Получаем из двух данных слов набор уже существующих с таким же значением
            IQueryable<Word> words = _db.Words;
            words = words.Where(w => (w.Value == firstWord.Value && w.Language == firstWord.Language) ||
                                     (w.Value == translation.Value && w.Language == translation.Language));

            switch (words.Count())
            {
                // Если ни одного из данных слов не существует
                case 0:
                    _db.Meanings.Add(new Meaning() { Words = new List<Word>() { firstWord, translation } });
                    break;
                case 1:
                    var newMeaning = new Meaning();
                    if (words.First().Value == firstWord.Value && words.First().Language == firstWord.Language)
                    {
                        newMeaning.Words.Add(translation);
                    }
                    else
                    {
                        newMeaning.Words.Add(firstWord);
                    }

                    words.First().Meanings.Add(newMeaning);
                    break;
                // Если оба данных слова уже существуют
                case 2:
                    // Выбираем из первого слова все связные слова
                    var allFirstWordTranslations = words.First().Meanings.Select(m => (m.Words, m));

                    // Переводим эти слова в нужный формат
                    List<(Word, Meaning)> allFirstWordTranslationsInCorrectFormat = new List<(Word, Meaning)>();
                    foreach (var tuple in allFirstWordTranslations)
                    {
                        foreach (var word in tuple.Words)
                        {
                            allFirstWordTranslationsInCorrectFormat.Add((word, tuple.m));
                        }
                    }
                    // Проворачиваем то же самое и со вторым словом
                    var allSecondWordTranslations = words.OrderByDescending(e => e.Id).First().Meanings.Select(m => (m.Words, m));

                    List<(Word, Meaning)> allSecondWordTranslationsInCorrectFormat = new List<(Word, Meaning)>();
                    foreach (var tuple in allSecondWordTranslations)
                    {
                        foreach (var word in tuple.Words)
                        {
                            allSecondWordTranslationsInCorrectFormat.Add((word, tuple.m));
                        }
                    }

                    // Получаем связанные слова, которые являются общими для данных в качестве
                    // аргумента слов
                    List<(Word, Meaning)> wordsWithOneMeaning = new List<(Word, Meaning)>();
                    foreach (var word in allFirstWordTranslationsInCorrectFormat)
                    {
                        foreach (var wordTranslation in allSecondWordTranslationsInCorrectFormat)
                        {
                            if (word.Item1.Value == wordTranslation.Item1.Value &&
                                word.Item1.Language == wordTranslation.Item1.Language)
                                wordsWithOneMeaning.Add(word);
                        }
                    }

                    foreach (var word in allSecondWordTranslationsInCorrectFormat)
                    {
                        foreach (var wordTranslation in allFirstWordTranslationsInCorrectFormat)
                        {
                            if (word.Item1.Value == wordTranslation.Item1.Value &&
                                word.Item1.Language == wordTranslation.Item1.Language)                            
                                wordsWithOneMeaning.Add(word);
                        }
                    }

                    foreach (var word in allFirstWordTranslationsInCorrectFormat)
                    {
                        if (wordsWithOneMeaning.Where(i => i.Item2 == word.Item2).Count() > 0)
                            wordsWithOneMeaning.Add(word);
                    }

                    foreach (var word in allSecondWordTranslationsInCorrectFormat)
                    {
                        if(wordsWithOneMeaning.Where(i => i.Item2 == word.Item2).Count() > 0)
                            wordsWithOneMeaning.Add(word);
                    }

                    var meaning = new Meaning();
                    // Удаляем у полученных слов предыдущее значение, связанное со словами-аргументами
                    // Добавляем новое значение, которое будет общим для всех собранных слов
                    foreach (var wordInOneMeaning in wordsWithOneMeaning)
                    {
                        words.First().Meanings.Remove(wordInOneMeaning.Item2);
                        words.OrderByDescending(e => e.Id).First().Meanings.Remove(wordInOneMeaning.Item2);
                        words.First().Meanings.Add(meaning);
                        words.OrderByDescending(e => e.Id).First().Meanings.Add(meaning);
                        wordInOneMeaning.Item1.Meanings.Remove(wordInOneMeaning.Item2);
                        wordInOneMeaning.Item1.Meanings.Add(meaning);
                    }

                    foreach (var wordInOneMeaning in wordsWithOneMeaning)
                    {
                        _db.Meanings.Remove(wordInOneMeaning.Item2);
                    }
                    break;
            }
            _db.SaveChanges();
        }
    }
}
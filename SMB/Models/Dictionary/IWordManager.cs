namespace SMB.Models.Dictionary
{
    interface IWordManager
    {
        void AddWordInDB(Word word, Word translation);
        void DeleteWordConnectionFromDB(Word word, Word translation, Meaning meaning);
    }
}

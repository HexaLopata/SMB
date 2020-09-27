namespace SMB.Models.Dictionary
{
    interface IWordManager
    {
        void AddWordInDB(Word word, Word translation);
    }
}

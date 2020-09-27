using System.Collections.Generic;

namespace SMB.Models.Dictionary
{
    public class Word
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }

        public virtual ICollection<Meaning> Meanings { get; set; }

        public Word()
        {
            Meanings = new List<Meaning>();
        }
    }
}
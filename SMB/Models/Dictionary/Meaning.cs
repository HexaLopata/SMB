using System.Collections.Generic;

namespace SMB.Models.Dictionary
{
    public class Meaning
    {
        public long Id { get; set; }

        public virtual ICollection<Word> Words { get; set; }

        public Meaning()
        {
            Words = new List<Word>();
        }
    }
}
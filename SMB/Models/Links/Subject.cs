using System.Collections.Generic;

namespace SMB.Models.Links
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }

        public Subject()
        {
            Topics = new List<Topic>();
        }
    }
}
using System.Collections.Generic;

namespace SMB.Models.Links
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public Topic()
        {
            Links = new List<Link>();
        }
    }
}
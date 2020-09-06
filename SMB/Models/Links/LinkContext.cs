using System.Data.Entity;

namespace SMB.Models.Links
{
    public class LinkContext : DbContext
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }

    public class LinkContextInitializer : DropCreateDatabaseAlways<LinkContext>
    {
        protected override void Seed(LinkContext context)
        {
            var algebra = new Subject() { Name = "Алгебра" };
            algebra.Topics.Add(new Topic() { Name = "Тригонометрия" });
            var physics = new Subject() { Name = "Физика" };
            context.Subjects.Add(algebra);
            context.Subjects.Add(physics);

            base.Seed(context);
        }
    }
}
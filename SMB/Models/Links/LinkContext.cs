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
            var trigonometry = new Topic() { Name = "Тригонометрия" };
            trigonometry.Links.Add(new Link() { Name = "Формулы приведения и как их запомнить", Content = "https://matemonline.com/dh/тригонометрия/formuly-privedenija/" });
            trigonometry.Links.Add(new Link() { Name = "Формулы суммы/разности тригонометрических функций", Content = "https://zaochnik.com/spravochnik/matematika/trigonometrija/summa-i-raznost-sinusov-i-kosinusov/" });
            trigonometry.Links.Add(new Link() { Name = "Все основные формулы в удобном формате", Content = "https://mnogoformul.ru/vse-formuly-po-trigonometrii" });
            algebra.Topics.Add(trigonometry);
            var physics = new Subject() { Name = "Физика" };
            context.Subjects.Add(algebra);
            context.Subjects.Add(physics);

            base.Seed(context);
        }
    }
}
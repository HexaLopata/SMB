﻿using SMB.Models.Autentification;
using SMB.Models.Dictionary;
using SMB.Models.Links;
using System.Data.Entity;

namespace SMB.Models.DataBases
{
    public class SMBContext : DbContext
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<RealProfile> Profiles { get; set; }
        public DbSet<Meaning> Meanings { get; set; }
        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meaning>().HasMany(w => w.Words)
                .WithMany(m => m.Meanings)
                .Map(t => t.MapLeftKey("MeaningId")
                .MapRightKey("WordId")
                .ToTable("MeaningWords"));    
        }
    }

    public class LinkContextInitializer : DropCreateDatabaseAlways<SMBContext>
    {
        protected override void Seed(SMBContext context)
        {
            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.ReturnHashedPasswordAsString("password");
            var admin = new RealProfile() { Name = "DoubleGrabli", Password = hashedPassword, Rank = ProfileRank.Admin };
            context.Profiles.Add(admin);
            var algebra = new Subject() { Name = "Алгебра" };
            var trigonometry = new Topic() { Name = "Тригонометрия" };
            trigonometry.Links.Add(new Link() { Name = "Формулы приведения и как их запомнить", Content = "https://matemonline.com/dh/тригонометрия/formuly-privedenija/" });
            trigonometry.Links.Add(new Link() { Name = "Формулы суммы/разности тригонометрических функций", Content = "https://zaochnik.com/spravochnik/matematika/trigonometrija/summa-i-raznost-sinusov-i-kosinusov/" });
            trigonometry.Links.Add(new Link() { Name = "Все основные формулы в удобном формате", Content = "https://mnogoformul.ru/vse-formuly-po-trigonometrii" });
            trigonometry.Links.Add(new Link() { Name = "Простейшие тригонометрические уравнения", Content = "https://www.resolventa.ru/spr/trig/equation.htm" });
            trigonometry.Links.Add(new Link() { Name = "Обратные тригонометрические функции.", Content = "https://www.calc.ru/Obratnyye-Trigonometricheskiye-Funktsii-A.html" });
            var logorifms = new Topic() { Name = "Логорифмы" };
            logorifms.Links.Add(new Link() { Name = "Основные формулы", Content = "https://zen.yandex.ru/media/studystudent/svoistva-logarifmov-shpargalka-s-formulami-5e99c14f7c1cd903b767fdab" });
            algebra.Topics.Add(trigonometry);
            algebra.Topics.Add(logorifms);
            var physics = new Subject() { Name = "Физика" };
            context.Subjects.Add(algebra);
            context.Subjects.Add(physics);

            base.Seed(context);
        }
    }
}
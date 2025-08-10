using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Data.Models.HomePage;
using DAL.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<AdviceRequest> AdviceRequests { get; set; }
        public DbSet<AdvisorAvailability> AdvisorAvailabilities { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<ServiceOffering> ServiceOfferings { get; set; }
        public DbSet<ServiceOfferingItem> ServiceOfferingItems { get; set; }

        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<HelpType> HelpTypes { get; set; }
        public DbSet<HelpRequest> HelpRequests { get; set; }
        public DbSet<ReconcileRequest> ReconcileRequests { get; set; }
        public DbSet<Mediation> Mediations { get; set; }
        public DbSet<ImagesLibrary> ImagesLibrary { get; set; }
        public DbSet<VideosLibrary> VideosLibraries { get; set; }
        public DbSet<HeroSection> HeroSections { get; set; }
        public DbSet<HomeVideoSection> HomeVideoSections { get; set; }
        public DbSet<TrendSection> TrendSections { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<DynamicPage> DynamicPages { get; set; }
        public DbSet<DynamicPageItem> DynamicPageItems { get; set; }
        public DbSet<NavItems> NavigationItems { get; set; }
        public DbSet<Pages> Pages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Advisor)
                .WithOne(a => a.User)
                .HasForeignKey<Advisor>(a => a.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Mediation)
                .WithOne(m => m.User)
                .HasForeignKey<Mediation>(m => m.UserId);


            builder.Entity<Advisor>()
                .HasOne(a => a.Consultation)
                .WithMany(c => c.Advisors)
                .HasForeignKey(a => a.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.Advisor)
                .WithMany(a => a.AdviceRequests)
                .HasForeignKey(ar => ar.AdvisorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.User)
                .WithMany(u => u.AdviceRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.Consultation)
                .WithMany(c => c.AdviceRequests)
                .HasForeignKey(ar => ar.ConsultationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ServiceOffering>()
            .HasMany(s => s.ServiceItem)
            .WithOne(i => i.ServiceOffering)
            .HasForeignKey(i => i.ServiceOfferingId)
            .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ServiceOffering>().HasData(new ServiceOffering
            {
                Id = 1,
                Title = "Default Title",
                Description = "Default Description"
            });



            builder.Entity<NavItems>()
       .HasMany(n => n.pages)
       .WithOne(p => p.NavItems)
       .HasForeignKey(p => p.NavItemsId)
       .OnDelete(DeleteBehavior.Cascade);

            // ======= NavItems seed =======
            builder.Entity<NavItems>().HasData(
                new NavItems { Id = 1, label = "الرئيسية", href = "/home" },
                new NavItems { Id = 2, label = "عن الجمعية", href = "/about-layout/about" },
                new NavItems { Id = 3, label = "الحوكمة", href = "/governance/regulations" },
                new NavItems { Id = 4, label = "طلبات المساعدة", href = "/help-layout/we-offer" },
                new NavItems { Id = 5, label = "المشاركة في الدعم", href = "/support-layout/bank-accounts" },
                new NavItems { Id = 6, label = "التنمية وإصلاح ذات البين", href = "/blank-page" },
                new NavItems { Id = 7, label = "النشاط الاستثماري", href = "/investment-layout/real-state" },
                new NavItems { Id = 8, label = "الخدمات الطبية", href = "/medcineLayout/GeneralDefinition" },
                new NavItems { Id = 9, label = "التطوع", href = "/volunteer-layout/unit" },
                new NavItems { Id = 10, label = "التواصل والشكاوى", href = "/about-layout/contact-phone" }
                //new NavItems { Id = 11, label = "صفحات إضافية", href = "#" } // empty placeholder
            );

            // ======= Pages seed (IDs must be unique) =======
            builder.Entity<Pages>().HasData(
                // عن الجمعية (Id=2)
                new Pages { Id = 1001, NavItemsId = 2, subTilte = " الرسالة والرؤية", subLink = "/about-layout/vision-mission" },
                new Pages { Id = 1002, NavItemsId = 2, subTilte = "نبذة عن الجمعية", subLink = "/about-layout/about-summary" },
                new Pages { Id = 1003, NavItemsId = 2, subTilte = "مهام الجمعية", subLink = "/about-layout/tasks-authorities" },
                new Pages { Id = 1004, NavItemsId = 2, subTilte = "الجمعية العمومية", subLink = "/about-layout/general-assembly" },
                new Pages { Id = 1005, NavItemsId = 2, subTilte = "مجلس الأدارة", subLink = "/about-layout/board-members" },
                new Pages { Id = 1006, NavItemsId = 2, subTilte = "الهيكل النظيمى", subLink = "/about-layout/organizational-structure" },
                new Pages { Id = 1007, NavItemsId = 2, subTilte = "مواقع العمل", subLink = "/about-layout/service-locations" },
                new Pages { Id = 1008, NavItemsId = 2, subTilte = "مكتبة الصور والفيديوهات", subLink = "/about-layout/ImageLibrary" },

                // الحوكمة (Id=3)
                new Pages { Id = 1101, NavItemsId = 3, subTilte = "الأنظمة واللوائح ", subLink = "/governance/regulations" },
                new Pages { Id = 1102, NavItemsId = 3, subTilte = "السياسات", subLink = "/governance/policies" },
                new Pages { Id = 1103, NavItemsId = 3, subTilte = "التقارير السنوية", subLink = "/blank-page" },
                new Pages { Id = 1104, NavItemsId = 3, subTilte = "التقارير الربعية", subLink = "/governance/quarterly-reports" },
                new Pages { Id = 1105, NavItemsId = 3, subTilte = "القوائم المالية", subLink = "/governance/financial-reports" },
                new Pages { Id = 1106, NavItemsId = 3, subTilte = "معايير الحوكمة", subLink = "/blank-page" },
                new Pages { Id = 1107, NavItemsId = 3, subTilte = "نتائج تقييم الحوكمة", subLink = "/governance/governance-evaluation" },
                new Pages { Id = 1108, NavItemsId = 3, subTilte = "الخطة الاستراتيجية", subLink = "/governance/strategic-plans" },
                new Pages { Id = 1109, NavItemsId = 3, subTilte = "الخطة التشغيلية", subLink = "/governance/operational-plan" },
                new Pages { Id = 1110, NavItemsId = 3, subTilte = "الأهداف", subLink = "/governance/Goals" },

                // طلبات المساعدة (Id=4)
                new Pages { Id = 1201, NavItemsId = 4, subTilte = "ماذا نقدم؟", subLink = "/help-layout/we-offer" },
                new Pages { Id = 1202, NavItemsId = 4, subTilte = "الفئات المستحقة", subLink = "/help-layout/eligible" },
                new Pages { Id = 1203, NavItemsId = 4, subTilte = "المستندات المطلوبة", subLink = "/help-layout/requirements" },
                new Pages { Id = 1204, NavItemsId = 4, subTilte = "الحصول على الخدمة", subLink = "/HelpPeopole" },
                new Pages { Id = 1205, NavItemsId = 4, subTilte = "إحصاءات", subLink = "/blank-page" },

                // المشاركة في الدعم (Id=5)
                new Pages { Id = 1301, NavItemsId = 5, subTilte = "نشاطات الجمعية", subLink = "/blank-page" },
                new Pages { Id = 1302, NavItemsId = 5, subTilte = "حسابات الجمعية", subLink = "/support-layout/bank-accounts" },
                new Pages { Id = 1303, NavItemsId = 5, subTilte = "الحصول على الخدمة", subLink = "https://jkmm.org.sa/ElectronicServices/Donate#" },
                new Pages { Id = 1304, NavItemsId = 5, subTilte = "إحصاءات", subLink = "/blank-page" },

                // التنمية وإصلاح ذات البين (Id=6)
                new Pages { Id = 1401, NavItemsId = 6, subTilte = "تعريف عام بالنشاط", subLink = "/blank-page" },
                new Pages { Id = 1402, NavItemsId = 6, subTilte = "طلب استشارة أون لاين", subLink = "/all-consultants" },
                new Pages { Id = 1403, NavItemsId = 6, subTilte = "طلب إصلاح ذات البين", subLink = "/RequesrRepair" },
                new Pages { Id = 1404, NavItemsId = 6, subTilte = "تقديم شكوى", subLink = "/complaints" },
                new Pages { Id = 1405, NavItemsId = 6, subTilte = "محاضرات توعوية", subLink = "/awarness-lecture" },
                new Pages { Id = 1406, NavItemsId = 6, subTilte = "إحصاءات", subLink = "/blank-page" },


                // النشاط الاستثماري (Id=7)
                new Pages { Id = 1501, NavItemsId = 7, subTilte = "تعريف عام بالنشاط", subLink = "/blank-page" },
                new Pages { Id = 1502, NavItemsId = 7, subTilte = "الاستثمارات العقارية", subLink = "/investment-layout/real-state" },
                new Pages { Id = 1503, NavItemsId = 7, subTilte = "المشروعات الاستثمارية", subLink = "/investment-layout/invest-project" },
                new Pages { Id = 1504, NavItemsId = 7, subTilte = "مشروعات التنمية المستدامة", subLink = "/blank-page" },
                new Pages { Id = 1505, NavItemsId = 7, subTilte = "إحصاءات", subLink = "/blank-page" },

                // الخدمات الطبية (Id=8)
                new Pages { Id = 1601, NavItemsId = 8, subTilte = "تعريف عام بالنشاط", subLink = "/medcineLayout/GeneralDefinition" },
                new Pages { Id = 1602, NavItemsId = 8, subTilte = "مركز غسيل الكلى", subLink = "/medcineLayout/MedicalCenter" },
                new Pages { Id = 1603, NavItemsId = 8, subTilte = "مركز السمع والنطق", subLink = "/medcineLayout/HairingCenter" },
                new Pages { Id = 1604, NavItemsId = 8, subTilte = "الخدمات الطبية من خارج الجمعية", subLink = "/blank-page" },
                new Pages { Id = 1605, NavItemsId = 8, subTilte = "إحصاءات", subLink = "/blank-page" },

                // التطوع (Id=9)
                new Pages { Id = 1701, NavItemsId = 9, subTilte = "تعريف بالنشاط", subLink = "/volunteer-layout/unit" },
                new Pages { Id = 1702, NavItemsId = 9, subTilte = "ميثاق التطوع", subLink = "/blank-page" },
                new Pages { Id = 1703, NavItemsId = 9, subTilte = "مجالات التطوع", subLink = "/volunteer-layout/volunteer-medical" },
                new Pages { Id = 1704, NavItemsId = 9, subTilte = "الحصول على الخدمة", subLink = "/Voulenteer" },
                new Pages { Id = 1705, NavItemsId = 9, subTilte = "إحصاءات التطوع", subLink = "/blank-page" },

                // التواصل والشكاوى (Id=10)
                new Pages { Id = 1801, NavItemsId = 10, subTilte = "تقديم مقترح", subLink = "/blank-page" },
                new Pages { Id = 1802, NavItemsId = 10, subTilte = "تقديم شكوى", subLink = "/complaints" },
                new Pages { Id = 1803, NavItemsId = 10, subTilte = "قياسات الرضا", subLink = "/Satisfaction" });

            //builder.Entity<Lecture>()
            //    .HasOne(l => l.Consultation)
            //    .WithMany(c => c.Lectures)
            //    .HasForeignKey(l => l.ConsultationId)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<Lecture>()
            //    .HasOne(l => l.CreatedByUser)
            //    .WithMany()
            //    .HasForeignKey(l => l.CreatedBy)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Seed HeroSection
            builder.Entity<HeroSection>().HasData(
                new HeroSection
                {
                    Id = 1,
                    BackgroundImageUrl = "/images/hero-bg.jpg",
                    MainTitle = "مرحباً بكم في موقعنا",
                    Stats1Label = "عدد المستفيدين",
                    Stats1Value = 1500,
                    Stats2Label = "المشاريع المكتملة",
                    Stats2Value = 120,
                    Stats3Label = "المتطوعين",
                    Stats3Value = 300,
                    Stats4Label = "سنوات الخبرة",
                    Stats4Value = 10
                }
            );

            // Seed HomeVideoSection
            builder.Entity<HomeVideoSection>().HasData(
                new HomeVideoSection
                {
                    Id = 1,
                    VideoUrl = "https://www.youtube.com/watch?v=abcd1234",
                    Title = "تعرف على خدماتنا",
                    Description = "فيديو قصير يوضح أهم الأنشطة والخدمات التي نقدمها."
                }
            );

            // Seed TrendSection
            builder.Entity<TrendSection>().HasData(
                new TrendSection
                {
                    Id = 1,
                    Title = "أحدث مبادراتنا",
                    Description = "تعرف على آخر المبادرات والبرامج التي أطلقناها لخدمة المجتمع.",
                    ImageUrl = "/images/trend.jpg",
                    ButtonText = "المزيد",
                    ButtonUrl = "/initiatives"
                }
            );

            // DynamicPage Configuration


            builder.Entity<DynamicPage>()
                .HasMany(dp => dp.Items)
                .WithOne(item => item.DynamicPage)
                .HasForeignKey(item => item.DynamicPageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DynamicPageItem>()
                .Property(item => item.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<DynamicPageItem>()
                .Property(item => item.Content)
                .IsRequired();

            builder.Entity<DynamicPageItem>()
                .Property(item => item.ImageUrl)
                .HasMaxLength(500);

            builder.Entity<DynamicPageItem>()
                .Property(item => item.FileUrl)
                .HasMaxLength(500);

            builder.Entity<DynamicPageItem>()
                .Property(item => item.FileName)
                .HasMaxLength(255);

            builder.Entity<AdvisorAvailability>()
                .HasOne(a => a.AdviceRequest)
                .WithOne()
                .HasForeignKey<AdvisorAvailability>(a => a.AdviceRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne<AdvisorAvailability>()
                .WithMany()
                .HasForeignKey(a => a.AdvisorAvailabilityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<NewsImage>()
                .HasOne(ni => ni.NewsItem)
                .WithMany(n => n.Images)
                .HasForeignKey(ni => ni.NewsItemId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}

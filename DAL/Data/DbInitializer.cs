using DAL.Data.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed Dynamic Pages if they don't exist
            if (!context.DynamicPages.Any())
            {
                var samplePages = new List<DynamicPage>
                {
                    new DynamicPage
                    {
                        PageName = "صفحة الترحيب",
                        Description = "صفحة ترحيبية بالزوار",
                        Slug = "welcome",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "system",
                        Items = new List<DynamicPageItem>
                        {
                            new DynamicPageItem
                            {
                                Type = "text",
                                Content = "مرحباً بكم في موقع الجمعية الخيرية. نحن نعمل على تقديم المساعدة للمحتاجين.",
                                Order = 1,
                                CreatedAt = DateTime.UtcNow
                            },
                            new DynamicPageItem
                            {
                                Type = "image_text",
                                Content = "نقدم خدمات متنوعة تشمل المساعدة الطبية والتعليمية والاجتماعية.",
                                ImageUrl = "/Images/general/1.jpg",
                                Order = 2,
                                CreatedAt = DateTime.UtcNow
                            }
                        }
                    },
                    new DynamicPage
                    {
                        PageName = "خدماتنا",
                        Description = "صفحة تعرض خدمات الجمعية",
                        Slug = "services",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "system",
                        Items = new List<DynamicPageItem>
                        {
                            new DynamicPageItem
                            {
                                Type = "text",
                                Content = "تقدم الجمعية الخيرية مجموعة متنوعة من الخدمات للمجتمع المحلي.",
                                Order = 1,
                                CreatedAt = DateTime.UtcNow
                            },
                            new DynamicPageItem
                            {
                                Type = "file",
                                Content = "دليل الخدمات المتاحة",
                                FileUrl = "/Pdf/forms/2021-Annual-Report.pdf",
                                FileName = "دليل الخدمات.pdf",
                                Order = 2,
                                CreatedAt = DateTime.UtcNow
                            }
                        }
                    }
                };

                context.DynamicPages.AddRange(samplePages);
                context.SaveChanges();
            }
        }
    }
} 
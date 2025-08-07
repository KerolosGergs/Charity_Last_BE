using DAL.Data;
using DAL.Data.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.DataSeed
{
    public static class DynamicPageSeed
    {
        public static async Task SeedDynamicPagesAsync(ApplicationDbContext context)
        {
            if (await context.DynamicPages.AnyAsync())
                return;

            var dynamicPages = new List<DynamicPage>
            {
                new DynamicPage
                {
                    PageName = "صفحة رضا العملاء",
                    Description = "صفحة مخصصة لرضا العملاء والخدمات المقدمة",
                    Slug = "customer-satisfaction",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<DynamicPageItem>
                    {
                        new DynamicPageItem
                        {
                            Type = "text",
                            Content = "نحن نعتز بخدمة عملائنا الكرام ونحرص على تلبية احتياجاتهم بأفضل الطرق الممكنة. نسعى دائماً لتقديم خدمات عالية الجودة تلبي توقعات عملائنا.",
                            Order = 0,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "image_text",
                            Content = "صورة توضيحية لمركز خدمة العملاء",
                            ImageUrl = "/assets/images/customer-service.jpg",
                            Order = 1,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "file",
                            Content = "تقرير رضا العملاء للعام 2023",
                            FileUrl = "/assets/pdfs/customer-satisfaction-2023.pdf",
                            FileName = "customer-satisfaction-2023.pdf",
                            Order = 2,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                },
                new DynamicPage
                {
                    PageName = "الخدمات الطبية",
                    Description = "صفحة الخدمات الطبية المتخصصة",
                    Slug = "medical-services",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<DynamicPageItem>
                    {
                        new DynamicPageItem
                        {
                            Type = "text",
                            Content = "نقدم مجموعة شاملة من الخدمات الطبية المتخصصة لجميع أفراد المجتمع. فريقنا الطبي المؤهل يحرص على تقديم أفضل رعاية صحية ممكنة.",
                            Order = 0,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "image_text",
                            Content = "مركز الخدمات الطبية",
                            ImageUrl = "/assets/images/medical-center.jpg",
                            Order = 1,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "text",
                            Content = "تشمل خدماتنا: الفحوصات الطبية، الاستشارات الطبية، العلاج الطبيعي، والرعاية الوقائية.",
                            Order = 2,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "file",
                            Content = "دليل الخدمات الطبية",
                            FileUrl = "/assets/pdfs/medical-services-guide.pdf",
                            FileName = "medical-services-guide.pdf",
                            Order = 3,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                },
                new DynamicPage
                {
                    PageName = "البرامج التعليمية",
                    Description = "صفحة البرامج التعليمية والتدريبية",
                    Slug = "educational-programs",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<DynamicPageItem>
                    {
                        new DynamicPageItem
                        {
                            Type = "text",
                            Content = "نقدم برامج تعليمية متنوعة تهدف إلى تطوير مهارات الأفراد وزيادة معرفتهم في مختلف المجالات.",
                            Order = 0,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "image_text",
                            Content = "فصول تعليمية مجهزة بأحدث التقنيات",
                            ImageUrl = "/assets/images/education-center.jpg",
                            Order = 1,
                            CreatedAt = DateTime.UtcNow
                        },
                        new DynamicPageItem
                        {
                            Type = "file",
                            Content = "البرامج التعليمية المتاحة",
                            FileUrl = "/assets/pdfs/educational-programs.pdf",
                            FileName = "educational-programs.pdf",
                            Order = 2,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                }
            };

            await context.DynamicPages.AddRangeAsync(dynamicPages);
            await context.SaveChangesAsync();
        }
    }
} 
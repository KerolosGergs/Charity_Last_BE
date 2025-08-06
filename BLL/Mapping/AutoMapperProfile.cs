using AutoMapper;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.AdminDTOs;
using Shared.DTOS.AdvisorDTOs;
using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.ComplaintDTOs;
using Shared.DTOS.ConsultationDTOs;
using Shared.DTOS.LectureDTOs;
using Shared.DTOS.NewsDTOs;
using Shared.DTOS.ServiceOfferingDTOs;
using Shared.DTOS.UserDTO;
using Shared.DTOS.VolunteerDTOs;
using Shared.DTOS.NotificationDTOs;
using Shared.DTOS.MediationDTOs;
using Shared.DTOS.HelpDTOs;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.ImageLibraryDTOs;
using Shared.DTOS.VideosLibraryDTOs;
using DAL.Data.Models.HomePage;
using Shared.DTOS.HomePageDTOS;

namespace BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Admin Mappings
            CreateMap<Admin, AdminDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<CreateAdminDTO, Admin>();
            CreateMap<UpdateAdminDTO, Admin>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Advisor Mappings
            CreateMap<Advisor, AdvisorDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation != null ? src.Consultation.ConsultationName : ""))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            CreateMap<CreateAdvisorDTO, Advisor>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            CreateMap<UpdateAdvisorDTO, Advisor>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            // Advisor Availability Mappings
            CreateMap<AdvisorAvailability, AdvisorAvailabilityDTO>()
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => src.Advisor.FullName));
            CreateMap<CreateAvailabilityDTO, AdvisorAvailability>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));
            CreateMap<UpdateAvailabilityDTO, AdvisorAvailability>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            // Advice Request Mappings
            CreateMap<AdviceRequest, AdviceRequestDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => src.Advisor.FullName))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation.ConsultationName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
                
            CreateMap<CreateAdviceRequestDTO, AdviceRequest>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));
            CreateMap<UpdateAdviceRequestDTO, AdviceRequest>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AdviceRequest,AdvisorRequestDTO>();
            // Complaint Mappings
           
            CreateMap<CreateComplaintDTO, Complaint>();
            CreateMap<UpdateComplaintDTO, Complaint>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Complaint, ComplaintDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.User.Email))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
            // Consultation Mappings
            CreateMap<Consultation, ConsultationDTO>();
            CreateMap<CreateConsultationDTO, Consultation>();
            CreateMap<UpdateConsultationDTO, Consultation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Lecture Mappings
            CreateMap<Lecture, LectureDTO>();
            //.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            //.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => DeserializeTags(src.Tags)));
            CreateMap<CreateLectureDTO, Lecture>();
            //.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            //.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => SerializeTags(src.Tags)));
            CreateMap<LectureDTO, Lecture>();
                //.ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<LectureType>(src.Type)))
                //.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => SerializeTags(src.Tags)));
            CreateMap<UpdateLectureDTO, Lecture>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<NewsItem, NewsItemDTO>()
                            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                                src.Images != null ? src.Images.Select(img => img.ImageUrl).ToList() : new List<string>()));


            CreateMap<CreateNewsItemDTO, NewsItem>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                 .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                 .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                 .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                 .ForMember(dest => dest.Images, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());


            CreateMap<UpdateNewsItemDTO, NewsItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<NewsImage, NewsImageDTO>();

            CreateMap<NewsImageDTO, NewsImage>()
                .ForMember(dest => dest.NewsItem, opt => opt.Ignore());

            // Service Offering Mappings
            CreateMap<ServiceOffering, ServiceOfferingDTO>();
            CreateMap<CreateServiceOfferingDTO, ServiceOffering>();
            CreateMap<UpdateServiceOfferingDTO, ServiceOffering>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // User Mappings
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<UpdateUserDTO, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Volunteer Application Mappings
            CreateMap<VolunteerApplication, VolunteerApplicationDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CreateVolunteerApplicationDTO, VolunteerApplication>();
            CreateMap<UpdateVolunteerApplicationDTO, VolunteerApplication>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Auth Mappings
            CreateMap<ApplicationUser, CurrentUserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<RegisterDTO, ApplicationUser>();
            CreateMap<RegisterAdminDTO, ApplicationUser>();
            CreateMap<RegisterAdvisorDTO, ApplicationUser>();

            // Notification Mappings
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationCreateDTO, Notification>();

            // Mediation Mappings
            CreateMap<Mediation, Shared.DTOS.MediationDTOs.MediationDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<Shared.DTOS.MediationDTOs.CreateMediationDTO, Mediation>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<Shared.DTOS.MediationDTOs.UpdateMediationDTO, Mediation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<HelpRequest, HelpRequestDTO>()
                .ForMember(dest => dest.HelpTypeName, opt => opt.MapFrom(src => src.HelpType.Name));
            CreateMap<CreateHelpRequestDTO, HelpRequest>();

            CreateMap<HelpType, HelpTypeDTO>();
            CreateMap<CreateHelpTypeDTO, HelpType>();

            CreateMap<ReconcileRequest, ReconcileRequestDTO>();
            CreateMap<ReconcileRequestDTO, ReconcileRequest>();

            CreateMap<CreateReconcileRequestDTO, ReconcileRequest>();

            CreateMap<AdviceRequest, GetAdvisorRequestDTO>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.AdvisorFullName, opt => opt.MapFrom(src => src.Advisor.FullName))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation.ConsultationName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                    src.Advisor.Availabilities.FirstOrDefault(a => a.Id == src.AdvisorAvailabilityId)!.Date))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src =>
                    src.Advisor.Availabilities.FirstOrDefault(a => a.Id == src.AdvisorAvailabilityId)!.Time))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src =>
                    src.Advisor.Availabilities.FirstOrDefault(a => a.Id == src.AdvisorAvailabilityId)!.Duration))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src =>
                    src.Advisor.Availabilities.FirstOrDefault(a => a.Id == src.AdvisorAvailabilityId)!.Notes));

            //ImageLibrary
            CreateMap<ImagesLibrary, ImageLibraryDTO>();
            CreateMap<CreateImageLibraryDTO, ImagesLibrary>();
            CreateMap<UpdateImageLibraryDTO, ImagesLibrary>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            //VideoLibrary
            CreateMap<VideosLibrary, VideosLibraryDTO>();
            CreateMap<CreateVideosLibraryDTO, VideosLibrary>();
            CreateMap<UpdateVideosLibraryDTO, VideosLibrary>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // HeroSection Mapping
            CreateMap<HeroSection, HeroSectionDTOs>();
            CreateMap<UpdateHeroSectionDTO, HeroSection>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // HomeVideoSection Mapping
            CreateMap<HomeVideoSection, HomeVideoSectionDTO>();
            CreateMap<UpdateHomeVideoSectionDTO, HomeVideoSection>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            //TrendSection Mapping
            CreateMap<TrendSection, TrendSectionDTO>();
            CreateMap<UpdateTrendSectionDTO, TrendSection>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        private static List<string> DeserializeTags(string tags)
        {
            return string.IsNullOrEmpty(tags)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(tags);
        }

        private static string SerializeTags(List<string> tags)
        {
            return tags != null
                ? System.Text.Json.JsonSerializer.Serialize(tags)
                : null;
        }
    }
} 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using Shared.DTOS.AdminDTOs;

namespace BLL.Service
{
    public class AdminDashboardCount : IAdminDashboardCount
    {
        private readonly IComplaintService _complaintService;
        private readonly IReconcileRequestService _reconcileRequestService;
        private readonly IVolunteerService _volunteerService;
        private readonly IAdvisorService _advisorService;
        private readonly ILectureService _lectureService;
        private readonly IAdviceRequestService _adviceRequestService;
        private readonly IServiceOfferingService _serviceOfferingService;

        public AdminDashboardCount(
        IComplaintService complaintService,
        IReconcileRequestService reconcileRequestService,
        IVolunteerService volunteerService,
        IAdvisorService advisorService,
        ILectureService lectureService,
        IAdviceRequestService adviceRequestService,
        IServiceOfferingService serviceOfferingService)
        {
            _complaintService = complaintService;
            _reconcileRequestService = reconcileRequestService;
            _volunteerService = volunteerService;
            _advisorService = advisorService;
            _lectureService = lectureService;
            _adviceRequestService = adviceRequestService;
            _serviceOfferingService = serviceOfferingService;
        }

       public async Task<DashboardStatisticsDTO> Count()
        {
            return new DashboardStatisticsDTO
            {
                ComplaintCount = await _complaintService.GetTotalComplaintsCountAsync(),
                ReconcileRequestCount = await _reconcileRequestService.GetAllRequestsCount(),
                VolunteerCount = await _volunteerService.GetTotalApplicationsCountAsync(),
                AdvisorCount = await _advisorService.GetAdvisorsCountAsync(),
                LectureCount = await _lectureService.GetTotalLecturesCountAsync(),
                AdviceRequestCount = await _adviceRequestService.GetTotalRequestsCountAsync(),
                ServiceOfferingCount = await _serviceOfferingService.GetTotalServicesCountAsync()
            };
        }

        //Task<DashboardStatisticsDTO> IAdminDashboardCount.Count()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
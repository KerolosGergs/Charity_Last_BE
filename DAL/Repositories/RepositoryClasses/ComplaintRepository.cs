using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.ComplaintDTOs;

namespace DAL.Repositories.RepositoryClasses
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _context.Complaints
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalComplaintsCountAsync()
        {
            return await _context.Complaints.CountAsync();
        }

        public async Task<int> GetComplaintsCountByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints.CountAsync(c => c.Status == status);
        }

        public async Task<List<object>> GetComplaintsByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.Complaints
                .Where(c => c.CreatedAt >= startDate)
                .GroupBy(c => new { c.CreatedAt.Year, c.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<double> GetAverageResponseTimeAsync()
        {
            var complaints = await _context.Complaints
                .Where(c => c.ResolvedAt.HasValue)
                .Select(c => (c.ResolvedAt.Value - c.CreatedAt).TotalHours)
                .ToListAsync();

            return complaints.Any() ? complaints.Average() : 0;
        }

        public async Task<List<Complaint>> GetRecentComplaintsAsync(int count)
        {
            return await _context.Complaints
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints
                .Where(c => c.Status == status)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetByCategoryAsync(ComplaintCategory category)
        {
            return await _context.Complaints
                .Where(c => c.Category == category)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetPendingComplaintsAsync()
        {
            return await _context.Complaints
                .Where(c => c.Status == ComplaintStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetResolvedComplaintsAsync()
        {
            return await _context.Complaints
                .Where(c => c.Status == ComplaintStatus.Resolved)
                .ToListAsync();
        }

        public async Task<int> GetComplaintCountByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints
                .CountAsync(c => c.Status == status);
        }
    }

}
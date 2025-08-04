using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class HelpRequestRepository : IHelpRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public HelpRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HelpRequest>> GetAllAsync()
        {
            return await _context.HelpRequests.Include(h => h.HelpType).ToListAsync();
        }

        public async Task<HelpRequest> AddAsync(HelpRequest entity)
        {
            _context.HelpRequests.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.HelpRequests.FindAsync(id);
            if (entity == null) return false;

            _context.HelpRequests.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
} 
using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class ServiceOfferingRepository : GenericRepository<ServiceOffering>, IServiceOfferingRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceOfferingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Get the one and only service offering record (Id = 1)
        public async Task<ServiceOffering> GetSingleAsync()
        {
            return await _context.ServiceOfferings
                .Include(s => s.ServiceItem)
                .FirstOrDefaultAsync(s => s.Id == 1);
        }
        public async Task<ServiceOffering> GetSingleAvialblyAsync()
        {
            return await _context.ServiceOfferings
                .Include(s => s.ServiceItem.Where(i=>i.IsActive==true))
                .FirstOrDefaultAsync(s => s.Id == 1);
        }


        public async Task<bool> UpdateTitleAndDescriptionAsync(string title, string description)
        {
            var service = await GetSingleAsync();
            if (service == null) return false;

            service.Title = title;
            service.Description = description;

            _context.ServiceOfferings.Update(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ServiceOfferingItem>> GetServiceItemsAsync()
        {
            return await _context.ServiceOfferingItems
                .Where(i => i.ServiceOfferingId == 1&&i.IsActive==true)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceOfferingItem> GetItemByIdAsync(int id)
        {
            return await _context.ServiceOfferingItems
                .FirstOrDefaultAsync(i => i.Id == id && i.ServiceOfferingId == 1);
        }

        public async Task AddServiceItemAsync(ServiceOfferingItem item)
        {
            _context.ServiceOfferingItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceItemAsync(ServiceOfferingItem item)
        {
            _context.ServiceOfferingItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceItemAsync(ServiceOfferingItem item)
        {
            _context.ServiceOfferingItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}

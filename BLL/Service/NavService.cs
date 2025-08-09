using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.NavItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class NavItemService : INavItemService
    {
        private readonly INavItemRepository _repo;
        public NavItemService(INavItemRepository repo) => _repo = repo;

        // NavItems
        public Task<List<NavItems>> GetAllNavItemsAsync() => _repo.GetAllNavItemsAsync();

        public Task<NavItemDto?> GetNavItemByIdAsync(int id) => _repo.GetNavItemByIdAsync(id);

        public Task AddNavItemAsync(NavItems navItem) => _repo.AddNavItemAsync(navItem);

        public async Task<bool> UpdateNavItemAsync(int id, NavItems navItem)
        {
            var exists = await _repo.FindNavItemEntityAsync(id);
            if (exists is null) return false;
            navItem.Id = id;
            await _repo.UpdateNavItemAsync(navItem);
            return true;
        }

        public async Task<bool> DeleteNavItemAsync(int id)
        {
            var exists = await _repo.FindNavItemEntityAsync(id);
            if (exists is null) return false;
            await _repo.DeleteNavItemAsync(id);
            return true;
        }

        // Pages
        public Task<List<Pages>> GetPagesAsync(int navItemId) => _repo.GetPagesByNavItemIdAsync(navItemId);

        public async Task<bool> AddPageAsync(int navItemId, PageDto page)
        {
            var nav = await _repo.FindNavItemEntityAsync(navItemId);
            if (nav is null) return false;

            var AddPAge = new Pages
            {
                NavItemsId = nav.Id,
                subLink = page.subLink,
                subTilte = page.subTilte,
            };
            //page.NavItemsId = navItemId;
            await _repo.AddPageAsync(AddPAge);
            return true;
        }

        public async Task<bool> UpdatePageAsync(int pageId, PageDto page)
        {
            // UPDATED: تحقق من وجود الكيان أولاً
            var entity = await _repo.FindPageEntityAsync(pageId);
            if (entity is null) return false;

            // UPDATED: عدّل على الكيان المُتعقَّب بدلاً من إنشاء كائن جديد
            entity.subLink = page.subLink;
            entity.subTilte = page.subTilte;

            // IMPORTANT: لا تغيّر NavItemsId أو الملاحة عشان ما تنقلش الصفحة بالغلط
            // entity.NavItemsId = entity.NavItemsId;

            // UPDATED: نفّذ الحفظ عبر الريبو (سواء Update على نفس الكيان أو SaveChanges)
            await _repo.UpdatePageAsync(entity); // لو UpdatePageAsync بيعمل SaveChanges داخلياً
                                                 // أو: await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePageAsync(int pageId)
        {
            var entity = await _repo.FindPageEntityAsync(pageId);
            if (entity is null) return false;
            await _repo.DeletePageAsync(pageId);
            return true;
        }
    }
}

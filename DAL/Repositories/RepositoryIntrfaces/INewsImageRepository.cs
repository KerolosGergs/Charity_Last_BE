using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface INewsImageRepository
    {
        Task<NewsImage> GetByIdAsync(int id);
        Task<IEnumerable<NewsImage>> GetByNewsItemIdAsync(int newsItemId);
        Task<NewsImage> AddAsync(NewsImage newsImage);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByNewsItemIdAsync(int newsItemId);
        public Task<NewsImage> GetByImageUrlAsync(string imageUrl);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.HomePage;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface ITrendSectionRepository
    {
        Task<TrendSection> GetTrendingVideosAsync();
        Task<TrendSection> UpdateTrendSection(TrendSection trendingVideos);
    }
}

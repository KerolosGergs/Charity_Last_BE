using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;

namespace DAL.Repositories.RepositoryClasses
{
    public class VideosLibraryRepository : GenericRepository<VideosLibrary> , IVideosLibraryRepository 
    {
        public VideosLibraryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

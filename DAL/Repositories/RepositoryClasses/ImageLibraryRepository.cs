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
    public class ImageLibraryRepository : GenericRepository<ImagesLibrary>, IImagesLibraryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ImageLibraryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

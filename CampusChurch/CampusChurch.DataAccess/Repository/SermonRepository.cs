using CampusChurch.DataAccess.Data;
using CampusChurch.DataAccess.Repository.IRepository;
using CampusChurch.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CampusChurch.DataAccess.Repository
{
    public class SermonRepository : Repository<Sermon>, ISermonRepository
    {
        private readonly ApplicationDbContext _db;

        public SermonRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Sermon obj)
        {
            var objFromDb = _db.Sermons.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Date = obj.Date;
                objFromDb.SeriesId = obj.SeriesId;
                objFromDb.Series = obj.Series;

                // Only update FilePath if it's provided
                if (!string.IsNullOrEmpty(obj.FilePath))
                {
                    objFromDb.FilePath = obj.FilePath;
                }

                // Only update ImagePath if it's provided
                if (!string.IsNullOrEmpty(obj.ImagePath))
                {
                    objFromDb.ImagePath = obj.ImagePath;
                }
            }
        }


        public Sermon GetFirstOrDefault(Expression<Func<Sermon, bool>> filter)
        {
            IQueryable<Sermon> query = _db.Sermons;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

       

    }
}



///old update
///

//public void Update(Sermon obj)
//{
//    var objFromDb = _db.Sermons.FirstOrDefault(u => u.Id == obj.Id);
//    if (objFromDb != null)
//    {
//        objFromDb.Title = obj.Title;
//        objFromDb.Description = obj.Description;
//        objFromDb.SeriesId = obj.SeriesId;
//        objFromDb.Series = obj.Series;
//        if (obj.FilePath != null)
//        {
//            objFromDb.FilePath = obj.FilePath;
//        }
//        objFromDb.Date = obj.Date;
//        if (obj.ImagePath != null)
//        {
//            objFromDb.ImagePath = obj.ImagePath;
//        }
//    }
//}

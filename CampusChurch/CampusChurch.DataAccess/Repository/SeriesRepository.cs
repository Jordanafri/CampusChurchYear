using CampusChurch.DataAccess.Data;
using CampusChurch.DataAccess.Repository.IRepository;
using CampusChurch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.DataAccess.Repository
{
    public class SeriesRepository : Repository<Series>, ISeriesRepository
    {
        private ApplicationDbContext _db;
        public SeriesRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Series obj) 
        {
            _db.Series.Update(obj);
        }
        public Series GetFirstOrDefault(Expression<Func<Series, bool>> filter)
        {
            IQueryable<Series> query = _db.Series;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
    }
}

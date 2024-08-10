using CampusChurch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.DataAccess.Repository.IRepository
{
    public interface ISeriesRepository : IRepository<Series>
    {
        void Update(Series obj);
        Series GetFirstOrDefault(Expression<Func<Series, bool>> filter);
    }
}

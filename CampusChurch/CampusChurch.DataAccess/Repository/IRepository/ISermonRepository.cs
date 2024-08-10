using CampusChurch.Models;
using System;
using System.Linq.Expressions;

namespace CampusChurch.DataAccess.Repository.IRepository
{
    public interface ISermonRepository : IRepository<Sermon>
    {
        void Update(Sermon obj);
        Sermon GetFirstOrDefault(Expression<Func<Sermon, bool>> filter);
    }
}

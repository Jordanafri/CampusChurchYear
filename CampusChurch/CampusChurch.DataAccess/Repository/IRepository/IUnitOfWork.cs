using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISeriesRepository Series { get; }
        ISermonRepository Sermon { get; }

        void Save();
    }
}

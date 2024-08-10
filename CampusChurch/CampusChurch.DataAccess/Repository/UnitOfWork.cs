using CampusChurch.DataAccess.Data;
using CampusChurch.DataAccess.Repository.IRepository;
using CampusChurch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public ISeriesRepository Series {  get; private set; }
        public ISermonRepository Sermon {  get; private set; }

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Series = new SeriesRepository(_db);
            Sermon = new SermonRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

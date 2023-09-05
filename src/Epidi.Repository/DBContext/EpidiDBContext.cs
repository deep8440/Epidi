using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.DBContext
{
    public class EpidiDBContext : DbContext
    {

        //public EpidiDBContext(DbContextOptions<EpidiDBContext> options) : base(options)
        //{
        //}
        public EpidiDBContext(DbContextOptions<EpidiDBContext> options)
          : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

    }
}

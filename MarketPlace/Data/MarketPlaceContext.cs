using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Models
{
    public class MarketPlaceContext : DbContext
    {
        public MarketPlaceContext (DbContextOptions<MarketPlaceContext> options)
            : base(options)
        {
        }

        public DbSet<MarketPlace.Models.Application> Application { get; set; }
    }
}

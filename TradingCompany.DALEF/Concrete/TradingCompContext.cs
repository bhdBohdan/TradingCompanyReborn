using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Data;

namespace TradingCompany.DALEF.Concrete
{
    public class TradingCompContext : TradCompCtx
    {
        private readonly string _connStr;

        public TradingCompContext(string connStr) : base()
        {
            _connStr = connStr;
        }

        public TradingCompContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(_connStr);
        
        

    }
}

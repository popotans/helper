using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class DbMapAccess : DbMap
    {
        public DbMapAccess(string dbPath)
        {
            this.db = new AccessDB(dbPath);
        }


    }
}

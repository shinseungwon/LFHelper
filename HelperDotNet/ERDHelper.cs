using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperDotNet
{
    public sealed class ERDHelper
    {
        public DbHelper dbHelper;

        public Logger logger;
        public bool logToDB = false;

        public ERDHelper(DbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public void SetLogger(Logger logger)
        {
            this.logger = logger;
        }
    }

    public sealed class TableObject
    {
        long id;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class connRec
    {

        public class BaseContext<TContext> : DbContext where TContext : DbContext
        {
            static BaseContext()
            {
                //Database.SetInitializer<TContext>(null);
            }
            protected BaseContext()

                : base("name=MSUDatabase1")
            {

            }
        }


        public class BaseContext2<TContext> : DbContext where TContext : DbContext
        {
            static BaseContext2()
            {
                //Database.SetInitializer<TContext>(null);
            }
            protected BaseContext2()

                : base("name=MSUDatabase2")
            {

            }
        }

        public class BaseContext3<TContext> : DbContext where TContext : DbContext
        {
            static BaseContext3()
            {
                //Database.SetInitializer<TContext>(null);
            }
            protected BaseContext3()

                : base("name=MSUDatabase3")
            {

            }
        }
    }
}
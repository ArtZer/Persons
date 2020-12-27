using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Persons
{
    class Statistic
    {
        public DataSet Show()
        {
            var sql = new SQLRequests();
            DataSet ds = sql.AmountPerson("01.12.2020", "26.12.2020");
            return ds; 
        }
    }
}

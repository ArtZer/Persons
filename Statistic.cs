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
        public List<int> ShowMonth(out List<int> money)
        {
            money = new List<int>();
            var countPersons = new List<int>();
            var sql = new SQLRequests();
            string[] month = { "01.01.2020", "01.02.2020", "01.03.2020", "01.04.2020", "01.05.2020", "01.06.2020", "01.07.2020", "01.08.2020", "01.09.2020", "01.10.2020", "01.11.2020", "01.12.2020", "01.01.2021" };

            for (int i = 0; i < 12; i++)
            {
                DataSet ds = sql.AmountPerson(month[i], month[i+1]);
                if(ds.Tables[0].Rows.Count != 0)
                {
                    money.Add(workdataset(ds));
                }
                else
                {
                    money.Add(0);
                }
                
                countPersons.Add(ds.Tables[0].Rows.Count);
            }            

            return countPersons;
        }

        private int workdataset(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            int money = 0;
            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;
                money += Convert.ToInt32(cells[5]);
            }
            money = (money/10000) / dt.Rows.Count;
            return money;
        }
    }
}

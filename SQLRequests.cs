using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Data;

namespace Persons
{
    class SQLRequests
    {
        private SqlCeConnection con;
        private SqlCeCommand cmd;
        private SqlCeDataAdapter adapter;
        DataSet dataSet;
        private string source = "DataSource=\"PersonsDB.sdf\"; password=\"qwerty\"";

        public void CreateDB()
        {
            SqlCeEngine en = new SqlCeEngine(source);
            en.CreateDatabase();
        }

        public void CreateTable()
        {
            con = new SqlCeConnection(source);
            con.Open();

            cmd = new SqlCeCommand(@"create table Persons
            (
                id int primary key identity(1,1),
                Фамилия nvarchar(100) not null,
                Имя nvarchar(100) not null,
                Отчество nvarchar(100),
                Должность nvarchar(100) not null,
                Оклад nvarchar(100) not null,
                ДатаПриема DateTime not null,
                ДатаУвольнения DateTime DEFAULT NULL
            )", con);

            cmd.ExecuteNonQuery();
            con.Close();            
        }

        public DataSet SelectQuestion(string table, string column, string param)
        {
            con = new SqlCeConnection(source);
            dataSet = new DataSet();

            if (param == "*")
            {
                var f = @"Select * From " + table;
                cmd = new SqlCeCommand(f, con);
            }
            else
            {
                var f = @"Select * From " + table + " Where [" + column + "] = '" + param + "'";
                cmd = new SqlCeCommand(f, con);
            }
            adapter = new SqlCeDataAdapter(cmd);

            adapter.Fill(dataSet);

            return dataSet;
        }

        public DataSet SelectQuestion(string table, string column, string param, string column2, string param2)
        {
            con = new SqlCeConnection(source);
            dataSet = new DataSet();

            var f = @"Select * From " + table + " Where [" + column + "] = '" + param + "' and [" + column2 + "] = '" + param2 + "'";
            cmd = new SqlCeCommand(f, con);
            
            adapter = new SqlCeDataAdapter(cmd);

            adapter.Fill(dataSet);

            return dataSet;
        }

        public void SelectChange(string table, string data, string id)
        {
            con = new SqlCeConnection(source);
            dataSet = new DataSet();

            var f = @"UPDATE " + table + " SET ДатаУвольнения = @Data WHERE id= '" + id + "' ";
            cmd = new SqlCeCommand(f, con);
            cmd.Parameters.AddWithValue("@Data", SqlDbType.DateTime).Value = data;

            adapter = new SqlCeDataAdapter(cmd);

            adapter.Fill(dataSet);
        }

        public List<int> amountPerson(string table)
        {
            string source = "Data Source=\"PersonsDB.sdf\"; password=\"qwerty\"";
            con = new SqlCeConnection(source);
            con.Open();

            var f = @"Select COUNT(*) From " + table + " Where ДатаПриема <= @Data and ДатаУвольнения is NULL";
            cmd = new SqlCeCommand(f, con);
            cmd.Parameters.AddWithValue("@Data", SqlDbType.DateTime).Value = DateTime.Today.ToString("d");
            SqlCeDataReader reader = cmd.ExecuteReader();

            var numbers = new List<int>();

            bool hasRow = reader.Read();
            if (hasRow)
            {
                numbers.Add(Convert.ToInt32(reader.GetValue(0)));
            }

            reader.Close();
            return numbers;
        } 

    }
}

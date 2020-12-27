using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace Persons
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SQLRequests sqlReq = new SQLRequests();

            string path = Directory.GetCurrentDirectory();
            string curFile = path + "\\PersonsDB.sdf";
            if (!File.Exists(curFile))
            {
                try
                {
                    sqlReq.CreateDB();
                    sqlReq.CreateTable();
                    MessageBox.Show("Файл БД и таблица созданы. Путь:  " + path);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Возникло исключение {ex.Message}");
                }
            }

            textBoxDataAdd.Text = DateTime.Today.ToString("d");
        }

        private SqlCeConnection con;
        private SqlCeCommand cmd;
        private SqlCeDataAdapter adapter;
        DataSet dataSet;
        private string source = "DataSource=\"PersonsDB.sdf\"; password=\"qwerty\"";
        SQLRequests sqlR = new SQLRequests();

        private void buttonAddPerson_Click_1(object sender, EventArgs e)
        {
            string messageError = CheckAddPerson();
            if(messageError != "")
            {
                MessageBox.Show(messageError);
            }

            var con = new SqlCeConnection(source);
            con.Open();
            SqlCeCommand com = new SqlCeCommand(@"INSERT INTO Persons (Фамилия,Имя,Отчество,Должность,Оклад,ДатаПриема,ДатаУвольнения) VALUES 
            ('" + textBoxFIO1.Text + "','" + textBoxFIO2.Text + "','" + textBoxFIO3.Text + "','" + comboBox1.Text + "','" + textBoxMoney.Text + "', @Data, NULL)", con);            
            com.Parameters.AddWithValue("@Data", SqlDbType.DateTime).Value = textBoxDataAdd.Text;

            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникло исключение {ex.Message}");
            }


            com.Dispose();
            con.Close();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            dataSet = sqlR.SelectQuestion("Persons", "", "*");

            mainGridView.DataSource = dataSet.Tables[0];
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            con = new SqlCeConnection(source);
            con.Open();
            cmd = new SqlCeCommand(@"Select * From Persons ", con);
            adapter = new SqlCeDataAdapter(cmd);

            SqlCeCommandBuilder CB = new SqlCeCommandBuilder(adapter);

            adapter.Update(dataSet);

            MessageBox.Show("Сохранено!");
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if(textBoxFirstName.Text == "" && textBoxLastName.Text == "")
            {
                MessageBox.Show("Заполните хотя бы одно поле для поиска!");
                return;
            }

            try
            {
                if (textBoxFirstName.Text != "" && textBoxLastName.Text != "")
                {
                    dataSet = sqlR.SelectQuestion("Persons", "Имя", textBoxFirstName.Text, "Фамилия", textBoxLastName.Text);
                }
                else if (textBoxFirstName.Text == "")
                {
                    dataSet = sqlR.SelectQuestion("Persons", "Фамилия", textBoxLastName.Text);
                }
                else
                {
                    dataSet = sqlR.SelectQuestion("Persons", "Имя", textBoxFirstName.Text);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Возникло исключение {ex.Message}");
                return;
            }

            mainGridView.DataSource = dataSet.Tables[0];
        }

        private void buttonFiredPerson_Click(object sender, EventArgs e)
        {
            if(textBoxDataFired.Text == "" || textBoxIDFired.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            try
            {
                sqlR.SelectChange("Persons", textBoxDataFired.Text, textBoxIDFired.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Возникло исключение {ex.Message}");
                return;
            }
            
            MessageBox.Show("Дата Увольнения была добавлена.");
        }

        private string CheckAddPerson()
        {
            if (textBoxFIO1.Text == "" || textBoxFIO2.Text == "" || textBoxFIO3.Text == "" || comboBox1.Text == "" || textBoxMoney.Text == "" || textBoxDataAdd.Text == "")
            {
                return "Вы не заполнели одно из полей!";
            }

            foreach (char ch in textBoxMoney.Text)
            {
                if (!char.IsDigit(ch))
                {
                    return "В поле оклад должны быть только цифры!";
                }
            }

            try
            {
                if (Convert.ToDateTime(textBoxDataAdd.Text) > DateTime.Now)
                {
                    return "Дата приема не может быть больше текущей!";
                }
            }
            catch(Exception ex)
            {
                return "Ошибка в формате даты! Исключение: " + ex;
            }            

            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Titles.Add("Кол сотрудников");
            chart1.Titles[0].Font = new Font("Utopia", 16);

            chart1.ChartAreas[0].BackColor = Color.Wheat;

            chart1.Series[0].Points.DataBindY(sqlR.amountPerson("Persons"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                var xml = new UnLoadingXML();
                xml.UnLoading(dataSet);
                MessageBox.Show("Выгрузка файла (Table) завершена");
            }
            catch (Exception ex)
            {
               MessageBox.Show("Исключение: " + ex);
            }

        }
    }
}

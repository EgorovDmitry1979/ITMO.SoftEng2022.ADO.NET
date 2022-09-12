using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // у.3.4 п.1

namespace DBCommand
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Упражнение 3.4. Исполнение запроса и отображение результата
        // Выполнение простого запроса
        private void button1_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder results = new System.Text.StringBuilder(); // у.3.4 п.4a
            sqlCommand1.CommandType = CommandType.Text; // у.3.4 п.4b
            sqlConnection1.Open(); // у.3.4 п.4c

            SqlDataReader reader = sqlCommand1.ExecuteReader(); // у.3.4 п.4d
            bool MoreResults = false;
            do
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        results.Append(reader[i].ToString() + "\t");
                    }
                    results.Append(Environment.NewLine);
                }
                MoreResults = reader.NextResult();
            } while (MoreResults);

            // у.3.4 п.4e
            reader.Close(); 
            sqlCommand1.Connection.Close();

            ResultsTextBox.Text = results.ToString(); // у.3.4 п.4f Предварительно создать текстовое поле и назвать.
        }
        // Вызов хранимой процедуры
        private void button2_Click(object sender, EventArgs e)
        {
            // п.5
            System.Text.StringBuilder results = new System.Text.StringBuilder();
            sqlCommand2.CommandType = CommandType.StoredProcedure;
            sqlCommand2.CommandText = "Ten Most Expensive Products"; // это название процедуры в Sql
            sqlCommand2.Connection.Open();
            SqlDataReader reader = sqlCommand2.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    results.Append(reader[i].ToString() + "\t");
                }
                results.Append(Environment.NewLine);
            }
            reader.Close();
            sqlCommand2.Connection.Close();
            ResultsTextBox1.Text = results.ToString();
        }

        // Выполнение команды, выполняющую операцию с каталогом
        private void button3_Click(object sender, EventArgs e)
        {
            // п.5
            try // п.8 реализация обработчика исключений
            {
                sqlCommand3.CommandType = CommandType.Text; // начало
                sqlCommand3.CommandText = "CREATE TABLE SalesPersonS (" +
                       "[SalesPersonID] [int] IDENTITY(1,1) NOT NULL, " +
                       "[FirstName] [nvarchar](50)  NULL, " +
                       "[LastName] [nvarchar](50)  NULL)";
                sqlCommand3.Connection.Open();
                sqlCommand3.ExecuteNonQuery();
                // sqlCommand3.Connection.Close(); // п.8 - переносим в блок finally
                MessageBox.Show("Таблица SalesPersonS создана"); // конец
            }

            //catch // п.8 обработчик кода для перехвата всех других типов исключений
            //и отображения для них общего сообщения (по примеру упражнения 1.3):
            catch (Exception Xcp)
            {
                MessageBox.Show(Xcp.Message, "Неожиданное исключение",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally // п.8 реализация обработчика исключений
            {
                sqlCommand3.Connection.Close(); // п.8 - переносим из верхного кода
            }
        }

        // Выполнение запроса с параметром
        private void button4_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder results = new System.Text.StringBuilder();
            sqlCommand4.CommandType = CommandType.Text;
            sqlCommand4.Parameters["@City"].Value = CityTextBox.Text;
            sqlConnection1.Open();
            SqlDataReader reader = sqlCommand4.ExecuteReader();
            bool MoreResults = false;
            do
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        results.Append(reader[i].ToString() + "\t");
                    }
                    results.Append(Environment.NewLine);
                }
                MoreResults = reader.NextResult();
            }
            
            while (MoreResults);
            reader.Close();
            sqlCommand4.Connection.Close();
            ResultsTextBox.Text = results.ToString();
        }

        // Выполнение параметризованной хранимой процедуры
        private void button5_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder results = new System.Text.StringBuilder();
            sqlCommand5.Parameters["@CategoryName"].Value = CategoryNameTextBox.Text;
            sqlCommand5.Parameters["@OrdYear"].Value = OrdYearTextBox.Text;
            sqlCommand5.Connection.Open();
            SqlDataReader reader = sqlCommand5.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    results.Append(reader[i].ToString() + "\t");
                }
                results.Append(Environment.NewLine);
            }
            reader.Close();
            sqlCommand5.Connection.Close();
            ResultsTextBox.Text = results.ToString();
        }
    }
}




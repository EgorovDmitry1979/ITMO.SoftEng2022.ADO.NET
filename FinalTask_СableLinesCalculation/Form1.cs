using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СableLinesCalculation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // засерить кнопку
            this.connection.StateChange += new System.Data.StateChangeEventHandler(this.connection_StateChange);
        }
        // засерить кнопку
        private void connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            button1.Enabled = (e.CurrentState == ConnectionState.Closed);
            button2.Enabled = (e.CurrentState == ConnectionState.Open);
            button3.Enabled = (e.CurrentState == ConnectionState.Open);
            button4.Enabled = (e.CurrentState == ConnectionState.Open);
            button5.Enabled = (e.CurrentState == ConnectionState.Open);
        }
        // Соединение с базой данных с пользователем и паролем
        OleDbConnection connection = new OleDbConnection();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    string UsID = userNameTextBox.Text;
                    string Psw = passwordTextBox.Text;
                    
                    if ((UsID == "1") && (Psw == "1"))
                    {
                        connection.ConnectionString = $"Provider=SQLOLEDB.1; Integrated Security=SSPI;" +
                            $"Persist Security Info=False; Initial Catalog=СableLines; Data Source=ASUS-SERVER\\SQLEXPRESS";

                        connection.Open();
                        MessageBox.Show("Соединение с базой данных выполнено успешно");
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин или пароль");
                    }
                }
                else
                    MessageBox.Show("Соединение с базой данных уже установлено");
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с базой данных");
            }
        }
        //Отключение от базы данных
        private void button2_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                MessageBox.Show("Соединение с базой данных закрыто");
            }
            else
                MessageBox.Show("Соединение с базой данных уже закрыто");
        }
        // загрузка данных из базы через DataSet
        private void button5_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                dataGridView1.DataSource = сableLinesDataSet.Excavation;
                dataGridView1.MultiSelect = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
                sqlDataAdapter1.Fill(сableLinesDataSet.Excavation);
            }
            else
                MessageBox.Show("Подключитесь к базе данных");
        }
        // Добавление строки в базу данных
        private void button3_Click(object sender, EventArgs e)
        {
            сableLinesDataSet1.ExcavationRow NewRow = (сableLinesDataSet1.ExcavationRow)сableLinesDataSet.Excavation.NewRow();
            string TT = textBox1.Text;
            string TD = textBox2.Text;
            string TW = textBox3.Text;
            string SF = textBox4.Text;
            string SB = textBox5.Text;
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    // Это обновляет форму на экране
                    NewRow.TrenchType = TT;
                    NewRow.TrenchDepth = TD;
                    NewRow.TrenchWidth = TW;
                    NewRow.SandFilling = SF;
                    NewRow.SandBackfilling = SB;
                    сableLinesDataSet.Excavation.Rows.Add(NewRow);

                    // Это обновляет таблицу в базе данных
                    SqlCommandBuilder commands = new SqlCommandBuilder(sqlDataAdapter1);
                    сableLinesDataSet.EndInit();
                    sqlDataAdapter1.Update(сableLinesDataSet.Tables["Excavation"]);
                }
                else
                    MessageBox.Show("Подключитесь к базе данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось добавить строку");
            }
        }
        // Удалить выбранную строку

        //private сableLinesDataSet1.ExcavationRow GetSelectedRow()
        //{
        //    String SelectedTrenchType = dataGridView1.CurrentRow.Cells["TrenchType"].Value.ToString();
        //    сableLinesDataSet1.ExcavationRow SelectedRow = сableLinesDataSet.Excavation.FindByTrenchType(SelectedTrenchType);
        //    return SelectedRow;
        //}

    private void button4_Click(object sender, EventArgs e)
        {
            //GetSelectedRow().Delete();
        }
    }
}

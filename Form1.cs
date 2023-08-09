using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SampleMySQL_DB {
    public partial class Form1 : Form {

        private MySqlConnection _connection;
        private MySqlCommand _command;

        public Form1()
        {
            InitializeComponent();
        }

        private bool TestConnection()
        {
            this._connection = new MySqlConnection(
                     "server=localhost; uid=root; database=samplemysql;"
                ); //"server=127.0.0.1;uid=root;pwd=12345;database=test"

            try
            {
                this._connection.Open();
                this._connection.Close();
                MessageBox.Show("Connection Successfull");
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Connection unsuccessfull");
                return false;
            }
        }

        private void FetchView()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM employee_tbl");
            MySqlDataAdapter adapter = new MySqlDataAdapter(command.CommandText, _connection);
            DataSet ds = new DataSet();

            adapter.Fill(ds, "Employee_tbl");
            dgvEmployee.DataSource = ds;
            dgvEmployee.DataMember = "Employee_tbl";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtEmployeeID.Text = this.txtEmployeeName.Text = this.txtEmployeeSal.Text
                = this.txtEmployeeAddr.Text = this.txtEmployeePhone.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this._command = new MySqlCommand(
                $"INSERT INTO employee_tbl(eid, ename, salary, address, telephone) VALUES({this.txtEmployeeID.Text}" +
                $", '{this.txtEmployeeName.Text}', {float.Parse(this.txtEmployeeSal.Text)}" +
                $", '{this.txtEmployeeAddr.Text}', {this.txtEmployeePhone.Text});"
                , _connection);

            this._connection.Open();
            this._command.ExecuteNonQuery();
            this._connection.Close();
            this.btnClear_Click(sender, e);
            this.FetchView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.TestConnection())
            {
                Application.Exit();
            }
            
            this.FetchView();
        }
    }
}

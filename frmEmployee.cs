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
    public partial class frmEmployee : Form {

        private MySqlConnection _connection;
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;
        private DataSet _ds;

        public frmEmployee()
        {
            InitializeComponent();
        }

        //private bool TestConnection()
        //{
        //    this._connection = new MySqlConnection(
        //             "server=localhost; uid=root; database=samplemysql;"
        //        ); //"server=127.0.0.1;uid=root;pwd=12345;database=test"

        //    try
        //    {
        //        this._connection.Open();
        //        this._connection.Close();
        //        MessageBox.Show("Connection Successfull");
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Connection unsuccessfull");
        //        return false;
        //    }
        //}

        private void Upsert(string id, string name, string salary, string address, string phone)
        {
            this._command = new MySqlCommand("" + 
                    "SELECT * FROM employee_tbl WHERE eid = " + id + ""
                );
            this._adapter = new MySqlDataAdapter(this._command.CommandText, _connection);

            this._ds.Clear();
            int selectData = this._adapter.Fill(this._ds, "employee_tbl");

            if (selectData == 0) {
                this._command = new MySqlCommand(
                $"INSERT INTO employee_tbl(eid, ename, salary, address, telephone) VALUES({id}" +
                $", '{name}', {float.Parse(salary)}" +
                $", '{address}', {phone});"
                , _connection);
            }
            else {
                this._command = new MySqlCommand(
                $"UPDATE emplpoyee_tbl SET" +
                $"ename =  '{name}', salary = '{float.Parse(salary)}', " +
                $"address = '{address}', telephone = '{phone}')" +
                $"WHERE eid = '{id}';"
                , _connection);
            }

            this._connection.Open();
            this._command.ExecuteNonQuery();
            this._connection.Close();
            this.btnClear_Click(null, null);
            this.FetchView();
        }

        private void AddDataBinding()
        {
            this.txtEmployeeID.DataBindings.Add("Text", _ds, "employee_tbl.eid");
            this.txtEmployeeName.DataBindings.Add("Text", _ds, "employee_tbl.ename");
            this.txtEmployeeSal.DataBindings.Add("Text", _ds, "employee_tbl.salary");
            this.txtEmployeeAddr.DataBindings.Add("Text", _ds, "employee_tbl.address");
            this.txtEmployeePhone.DataBindings.Add("Text", _ds, "employee_tbl.telephone");
        }

        private void ClearDataBinding()
        {
            this.txtEmployeeID.DataBindings.Clear();
            this.txtEmployeeName.DataBindings.Clear();
            this.txtEmployeeSal.DataBindings.Clear();
            this.txtEmployeeAddr.DataBindings.Clear();
            this.txtEmployeePhone.DataBindings.Clear();
        }

        private void FetchView()
        {
            this._command = new MySqlCommand("SELECT * FROM employee_tbl");
            this._adapter = new MySqlDataAdapter(this._command.CommandText, _connection);
            this._ds = new DataSet();

            try
            {
                _adapter.Fill(_ds, "Employee_tbl");
                dgvEmployee.DataSource = _ds;
                dgvEmployee.DataMember = "Employee_tbl";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SampleMySQL_DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtEmployeeID.Text = this.txtEmployeeName.Text = this.txtEmployeeSal.Text
                = this.txtEmployeeAddr.Text = this.txtEmployeePhone.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Upsert(this.txtEmployeeID.Text, this.txtEmployeeName.Text, this.txtEmployeeSal.Text
                , this.txtEmployeeAddr.Text, this.txtEmployeePhone.Text);
            this.btnClear_Click(sender, e);
            this.FetchView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (!this.TestConnection())
            //{
            //    Application.Exit();
            //}
            
            this.FetchView();
        }

        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.btnEdit.Checked)
            {
                this.btnClear_Click(sender, e);
                this.ClearDataBinding();
                this.AddDataBinding();
            } 
        }

        private void btnEdit_CheckedChanged(object sender, EventArgs e)
        {
            if(!this.btnEdit.Checked)
            {
                this.btnClear_Click(sender, e);
                this.ClearDataBinding();
            }
        }
    }
}

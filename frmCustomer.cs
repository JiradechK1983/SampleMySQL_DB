using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SampleMySQL_DB {

    public partial class frmCustomer : Form {

        private MySqlConnection _connection;
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;
        private DataSet _ds = new DataSet();

        public frmCustomer()
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

        private void AddDataBinding()
        {
            this.txtCustomerID.DataBindings.Add("Text", _ds, "customer_tbl.cid");
            this.txtCustomerName.DataBindings.Add("Text", _ds, "customer_tbl.cname");
            this.txtCustomerAddress.DataBindings.Add("Text", _ds, "customer_tbl.address");
            this.txtCustomerTel.DataBindings.Add("Text", _ds, "customer_tbl.telephone");
            this.txtCreditLimit.DataBindings.Add("Text", _ds, "customer_tbl.credit_lim");
            this.txtCurrentBal.DataBindings.Add("Text", _ds, "customer_tbl.curr_bal");
        }

        private void ClearDataBinding()
        {
            this.txtCustomerID.DataBindings.Clear();
            this.txtCustomerName.DataBindings.Clear();
            this.txtCustomerAddress.DataBindings.Clear();
            this.txtCustomerTel.DataBindings.Clear();
            this.txtCreditLimit.DataBindings.Clear();
            this.txtCurrentBal.DataBindings.Clear();
        }

        private void FetchView(string CusId, string CusName, string Credit_Lim, string Curr_Bal)
        {
            this._command = new MySqlCommand(
                    $"SELECT * FROM customer_tbl " +
                    $"WHERE cid LIKE '%{CusId}%' AND cname LIKE '%{CusName}%' " +
                    $"AND credit_lim LIKE '%{Credit_Lim}%' " +
                    $"AND curr_bal LIKE '%{Curr_Bal}%';"
                );

            this._adapter = new MySqlDataAdapter(this._command.CommandText, _connection);
            this._ds = new DataSet();

            _adapter.Fill(_ds, "customer_tbl");
            dgvCustomer.DataSource = _ds;
            dgvCustomer.DataMember = "customer_tbl";
        }

        private void Upsert(string CusId, string CusName, string CusAddress, string CusTel
            , string CusCredit_Lim, string CusCrr_Bal)
        {
            this._command = new MySqlCommand(
                $"CALL spUpsertCustomer('{CusId}', '{CusName}', '{CusAddress}', '{CusTel}'" +
                $", '{int.Parse(CusCredit_Lim)}', '{int.Parse(CusCrr_Bal)}')"
                , _connection);

            this._connection.Open();
            this._command.ExecuteNonQuery();
            this._connection.Close();
            this.btnClear_Click(null, null);
            this.FetchView(this.txtCustomerID.Text, this.txtCustomerName.Text
                , this.txtCreditLimit.Text, this.txtCurrentBal.Text);
        }

        private void dgvCustomer_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.btnEdit.Checked)
            {
                this.btnClear_Click(sender, e);
                this.ClearDataBinding();
                this.AddDataBinding();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Upsert(this.txtCustomerID.Text, this.txtCustomerName.Text
                , this.txtCustomerAddress.Text, this.txtCustomerTel.Text
                , this.txtCreditLimit.Text, this.txtCurrentBal.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCustomerID.Text = this.txtCustomerName.Text = this.txtCustomerAddress.Text
                = this.txtCustomerTel.Text = this.txtCreditLimit.Text = this.txtCurrentBal.Text = "";
        }

        private void btnEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.btnEdit.Checked)
            {
                this.btnClear_Click(sender, e);
                this.ClearDataBinding();
            }
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {

        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            if (!this.TestConnection())
            {
                Application.Exit();
            }

            this.FetchView(this.txtCustomerID.Text, this.txtCustomerName.Text
                , this.txtCreditLimit.Text, this.txtCurrentBal.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.FetchView(this.txtCustomerID.Text, this.txtCustomerName.Text
                , this.txtCreditLimit.Text, this.txtCurrentBal.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stok
{
    public partial class frmSatisListele : Form
    {
        public frmSatisListele()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");
        DataSet daset = new DataSet();

        private void satisListele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select*from satis", baglanti);
            adtr.Fill(daset, "satis");
            dataGridView1.DataSource = daset.Tables["satis"];


            baglanti.Close();
        }

        private void frmSatisListele_Load(object sender, EventArgs e)
        {
            satisListele();
        }
    }
}

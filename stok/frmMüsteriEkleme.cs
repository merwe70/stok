using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace stok
{
    public partial class frmMüsteriEkleme : Form
    {
        public frmMüsteriEkleme()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");

        private void frmMüsteriEkleme_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {


           


            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into musteri(tc,adsoyad,telefon,adres,email) values(@tc,@adsoyad,@telefon,@adres,@email) ", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAd.Text);
            komut.Parameters.AddWithValue("@telefon", txtTlf.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müşteri Kaydı Eklendi");
            foreach (Control item in this.Controls) /// bu formdaki kontrolleri dolaşmasını sağlıyoruz
            {
                if (item is TextBox) /// eğer bu kontroller textbox ise 
                {
                    item.Text = ""; /// textleri sil
                }
            }

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

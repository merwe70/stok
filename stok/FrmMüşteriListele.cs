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
    public partial class FrmMüşteriListele : Form
    {
        public FrmMüşteriListele()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");
        DataSet daset = new DataSet();/// kayıtları burda geçici olarak tutuyoruz

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("update musteri set adsoyad=@adsoyad,telefon=@telefon,adres=@adres,email=@email where tc=@tc", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAd.Text);
            komut.Parameters.AddWithValue("@telefon", txtTlf.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteri"].Clear();
            kayitGoster();
            MessageBox.Show("Müşteri Kaydı Güncellendi");
            foreach (Control item in this.Controls) /// bu formdaki kontrolleri dolaşmasını sağlıyoruz
            {
                if (item is TextBox) /// eğer bu kontroller textbox ise 
                {
                    item.Text = ""; /// textleri sil
                }
            }
        }

        private void FrmMüşteriListele_Load(object sender, EventArgs e)
        {
            kayitGoster();
        }

        private void kayitGoster()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("Select*from musteri", baglanti);
            adtr.Fill(daset, "musteri");
            dataGridView1.DataSource = daset.Tables["musteri"];
            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAd.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtTlf.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("delete from musteri where tc='" + dataGridView1.CurrentRow.Cells["tc"].Value.ToString()+ "'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteri"].Clear();
            kayitGoster();
            MessageBox.Show("Kayıt Silindi");

        }

        private void txtTcAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select*from musteri where tc like '%"+txtTcAra.Text+"%'",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

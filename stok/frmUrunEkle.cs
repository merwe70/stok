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
    public partial class frmUrunEkle : Form
    {
        public frmUrunEkle()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");

        bool durum;
        private void barkodKontrol()
        {
            durum = true;///istemediğimiz işlemi de false olarak tanımlayacağız
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from urun", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString() || txtBarkodNo.Text == "")
                    ///eğer veritabanında aynı barkod no varsa ya da boş geçilirse ekleme yapma dedik
                {
                    durum = false;
                }
                   
            }
           baglanti.Close();
        }
            
        


    private void button1_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if (durum==true)
            {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into urun(barkodno,kategori,marka,urunadi,miktari,alisfiyati,satisfiyati,tarih) " +
                "values(@barkodno,@kategori,@marka,@urunadi,@miktari,@alisfiyati,@satisfiyati,@tarih)", baglanti);
            komut.Parameters.AddWithValue("@barkodno",txtBarkodNo.Text);
            komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
            komut.Parameters.AddWithValue("@marka", comboMarka.Text);
            komut.Parameters.AddWithValue("@urunadi", txtUrunAdi.Text);
            komut.Parameters.AddWithValue("@miktari",int.Parse( txtMiktari.Text));
            komut.Parameters.AddWithValue("@alisfiyati",double.Parse( txtAlisFiyati.Text));
            komut.Parameters.AddWithValue("@satisfiyati", double.Parse( txtSatisFiyati.Text));
            komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir barkodno var","Uyarı");
            }
            
            comboMarka.Items.Clear();
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }

            }
        }
        private void kategoriGetir()
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from kategoribilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["kategori"].ToString());
            }
            baglanti.Close();
        }
        private void frmUrunEkle_Load(object sender, EventArgs e)
        {
            kategoriGetir();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from markabilgileri where kategori='"+comboKategori.SelectedItem+"'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void BarkodNotxt_TextChanged(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text=="")
            {
                lblMiktari.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text="";
                    }
                }
            }
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from urun where barkodno like '%"+BarkodNotxt.Text+"%'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["kategori"].ToString();
                Markatxt.Text = read["marka"].ToString();
                UrunAditxt.Text = read["urunadi"].ToString();
                lblMiktari.Text = read["miktari"].ToString();
                AlisFiyatitxt.Text = read["alisfiyati"].ToString();
                SatisFiyatitxt.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("update urun set miktari=miktari+'"+int.Parse(Miktaritxt.Text)+"' " +
                "where barkodno='"+BarkodNotxt.Text+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var Olan Ürüne Ekleme Yapıldı");




        }
    }
}

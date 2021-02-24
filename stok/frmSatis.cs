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
    public partial class frmSatis : Form
    {
        public frmSatis()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");
        DataSet daset = new DataSet();

        private void sepetListele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select*from sepet",baglanti);
            adtr.Fill(daset, "sepet");
            dataGridView1.DataSource = daset.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            
            baglanti.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMüsteriEkleme ekle = new frmMüsteriEkleme();
            ekle.ShowDialog();
        }
        bool durum;
        private void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from sepet",baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkod.Text==read["barkodno"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if (durum==true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into sepet(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) " +
                    "values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAd.Text);
                komut.Parameters.AddWithValue("@telefon", txtTel.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkod.Text);
                komut.Parameters.AddWithValue("@urunadi", txtUrun.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSaFiyat.Text));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                OleDbCommand komut2 = new OleDbCommand("update sepet set miktari=miktari+'"+int.Parse(txtMiktar.Text)+ "'where barkodno='" + txtBarkod.Text + "'", baglanti);
                komut2.ExecuteNonQuery();

                OleDbCommand komut3 = new OleDbCommand("update sepet set toplamfiyati=miktari*satisfiyati where barkodno='"+txtBarkod.Text+"'", baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();
            }
            
            txtMiktar.Text = "1"; 
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";

                    }
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmMüşteriListele listele = new FrmMüşteriListele();
            listele.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUrunEkle ekle = new frmUrunEkle();
            ekle.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunListele listele = new frmUrunListele();
            listele.ShowDialog();
        }

        private void hesapla()
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("select sum(toplamfiyati) from sepet", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + "TL";
                baglanti.Close();
            }
            catch (Exception)
            {

                ;
            }
        }
        private void frmSatis_Load(object sender, EventArgs e)
        {
            sepetListele();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text=="")
            {
                    txtAd.Text = "";
                    txtTel.Text = "";
            }
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from musteri where tc like '"+txtTc.Text+"'",baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                
                txtAd.Text = read["adsoyad"].ToString();
                txtTel.Text = read["telefon"].ToString();

            }
            baglanti.Close();
        }

        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from urun where barkodno like '" + txtBarkod.Text + "'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {

                txtUrun.Text = read["urunadi"].ToString();
                txtSaFiyat.Text = read["satisfiyati"].ToString();

            }
            baglanti.Close();
        }

        private void Temizle()
        {
            if (txtBarkod.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";

                        }
                    }

                }
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSaFiyat.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void txtSaFiyat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSaFiyat.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("delete from sepet where barkodno='"+dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString()+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            
            MessageBox.Show("Ürün Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("delete from sepet ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            
            MessageBox.Show("Ürünler Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatisListele listele = new frmSatisListele();
            listele.ShowDialog();
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount-1; i++)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) " +
                    "values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAd.Text);
                komut.Parameters.AddWithValue("@telefon", txtTel.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                OleDbCommand komut2 = new OleDbCommand("update urun set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + 
                    "' where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "'", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();  
            }

            baglanti.Open();
            OleDbCommand komut3 = new OleDbCommand("delete from sepet ", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
                
         }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

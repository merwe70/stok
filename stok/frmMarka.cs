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
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");
        bool durum;
        private void markaKontrol()
        {
            durum = true;///istemediğimiz işlemi de false olarak tanımlayacağız
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from markabilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text==read["kategori"].ToString() && textBox1.Text == read["marka"].ToString() || comboBox1.Text=="" || textBox1.Text == "")///aradığımız kayıt veritabanında varsa durumu false yap
                {///o kategoriye ait o marka varsa bunu engelle dedik veya tektx1 veya combo1 boş geçilirse ekleme yapma dedik
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            markaKontrol();
            if (durum==true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into markabilgileri(kategori,marka)values ('" + comboBox1.Text + "','" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Marka Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori ve marka var","Uyarı");
            }
           
            textBox1.Text = "";
            comboBox1.Text = "";
            
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategoriGetir();
        }

        private void kategoriGetir()
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from kategoribilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["kategori"].ToString());
            }
            baglanti.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

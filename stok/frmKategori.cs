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
    public partial class frmKategori : Form
    {
        public frmKategori()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=stokyeni.mdb");
        bool durum;
        private void kategoriKontol()
        {
            durum = true;///istemediğimiz işlemi de false olarak tanımlayacağız
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select*from kategoribilgileri",baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (textBox1.Text==read["kategori"].ToString() || textBox1.Text=="")///aradığımız kayıt veritabanında varsa durumu false yap
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void frmKategori_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategoriKontol();
            if (durum==true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into kategoribilgileri(kategori)values ('" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                
                MessageBox.Show("Kategori Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori var","Uyarı");
            }
            textBox1.Text = "";
        }
    }
}

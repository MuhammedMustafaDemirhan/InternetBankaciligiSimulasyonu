using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BankamatikSimilasyonu
{
    public partial class GirisEkrani : Form
    {
        public GirisEkrani()
        {
            InitializeComponent();
        }

        Baglanti baglan = new Baglanti();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select * From TBLKisiler Where HESAPNO=@p1 and SİFRE=@p2", baglan.baglanti());
            komut.Parameters.AddWithValue("@p1", TXTHesapNo.Text);
            komut.Parameters.AddWithValue("@p2", TXTSifre.Text);
            SqlDataReader dr = komut.ExecuteReader();
            if(dr.Read())
            {
                HesapHareketEkrani frmhesaphareket = new HesapHareketEkrani();
                frmhesaphareket.hesapno = TXTHesapNo.Text;
                frmhesaphareket.sifre = TXTSifre.Text;
                frmhesaphareket.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Giriş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GuncellemeEklemeEkrani frmekle = new GuncellemeEklemeEkrani();
            frmekle.giristuru = "Ekle";
            frmekle.Show();
            this.Hide();
        }
    }
}

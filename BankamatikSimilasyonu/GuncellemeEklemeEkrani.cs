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
using System.Data.SqlTypes;

namespace BankamatikSimilasyonu
{
    public partial class GuncellemeEklemeEkrani : Form
    {
        public GuncellemeEklemeEkrani()
        {
            InitializeComponent();
        }

        public string giristuru;
        public string ad, soyad, hesapno, telefon, tc, sifre;
        string rasthesapno;
        Baglanti baglan = new Baglanti();

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            rasthesapno = (rnd.Next(100000, 1000000)).ToString();
            SqlCommand komut = new SqlCommand("Select * From TBLKisiler Where HESAPNO=@p1", baglan.baglanti());
            komut.Parameters.AddWithValue("@p1", rasthesapno);
            SqlDataReader dr = komut.ExecuteReader();
            if(dr.Read())
            {
                MessageBox.Show("Hesap No Mevcut!\nTekrar deneyiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TXTHesapNo.Text = rasthesapno;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(giristuru == "Ekle")
            {
                GirisEkrani frm = new GirisEkrani();
                frm.Show();
                this.Hide();
            }
            else if(giristuru == "Güncelle")
            {
                HesapHareketEkrani frmhareket = new HesapHareketEkrani();
                frmhareket.hesapno = hesapno;
                frmhareket.Show();
                this.Hide();
            }
        }

        private void BTNGiris_Click(object sender, EventArgs e)
        {
            if(giristuru == "Ekle")
            {
                if(TXTAd.Text == "" || TXTSoyad.Text == "" || TXTHesapNo.Text == "" || MTXTTelNo.Text == "" || TXTTcNo.Text == "" || TXTSifre.Text == "")
                {
                    MessageBox.Show("Hiçbir alan boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand komutekle = new SqlCommand("Insert into TBLKisiler(AD,SOYAD,TC,TELEFON,HESAPNO,SİFRE) Values(@p1,@p2,@p3,@p4,@p5,@p6)", baglan.baglanti());
                    komutekle.Parameters.AddWithValue("@p1", TXTAd.Text);
                    komutekle.Parameters.AddWithValue("@p2", TXTSoyad.Text);
                    komutekle.Parameters.AddWithValue("@p3", TXTTcNo.Text);
                    komutekle.Parameters.AddWithValue("@p4", MTXTTelNo.Text);
                    komutekle.Parameters.AddWithValue("@p5", TXTHesapNo.Text);
                    komutekle.Parameters.AddWithValue("@p6", TXTSifre.Text);
                    komutekle.ExecuteNonQuery();
                    baglan.baglanti().Close();
                    MessageBox.Show("Sisteme Kaydınız Gerçekleştirildi!\nGiriş Yapabilirsiniz...", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GirisEkrani frmgiris = new GirisEkrani();
                    frmgiris.Show();
                    this.Hide();
                }
            }
            else if(giristuru == "Güncelle")
            {
                SqlCommand komutguncelle = new SqlCommand("Update TBLKisiler set AD=@p1,SOYAD=@p2,TC=@p3,TELEFON=@p4,HESAPNO=@p5,SİFRE=@p6 Where HESAPNO=@p7", baglan.baglanti());
                komutguncelle.Parameters.AddWithValue("@p1", TXTAd.Text);
                komutguncelle.Parameters.AddWithValue("@p2", TXTSoyad.Text);
                komutguncelle.Parameters.AddWithValue("@p3", TXTTcNo.Text);
                komutguncelle.Parameters.AddWithValue("@p4", MTXTTelNo.Text);
                komutguncelle.Parameters.AddWithValue("@p5", TXTHesapNo.Text);
                komutguncelle.Parameters.AddWithValue("@p6", TXTSifre.Text);
                komutguncelle.Parameters.AddWithValue("@p7", TXTHesapNo.Text);
                komutguncelle.ExecuteNonQuery();
                baglan.baglanti().Close();
                MessageBox.Show("Kaydınız Güncellendi!\nSisteme Dönüş Yapıyorsunuz...", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HesapHareketEkrani frmhareket = new HesapHareketEkrani();
                frmhareket.hesapno = hesapno;
                frmhareket.Show();
                this.Hide();
            }
        }

        private void GuncellemeEklemeEkrani_Load(object sender, EventArgs e)
        {
            if (giristuru == "Ekle")
            {
                TXTHesapNo.Text = "";
            }
            else if (giristuru == "Güncelle")
            {
                button1.Enabled = false;
                TXTHesapNo.Enabled = false;
                TXTAd.Text = ad;
                TXTSoyad.Text = soyad;
                TXTHesapNo.Text = hesapno;
                MTXTTelNo.Text = telefon;
                TXTTcNo.Text = tc;
                TXTSifre.Text = sifre;
            }
        }
    }
}

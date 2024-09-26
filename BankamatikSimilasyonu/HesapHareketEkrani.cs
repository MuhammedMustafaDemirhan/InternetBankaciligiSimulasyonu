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
    public partial class HesapHareketEkrani : Form
    {
        public HesapHareketEkrani()
        {
            InitializeComponent();
        }

        public string hesapno, sifre;
        string ad, soyad;
        Baglanti baglan = new Baglanti();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            GirisEkrani frmgiris = new GirisEkrani();
            frmgiris.Show();
        }

        private void HesapHareketEkrani_Load(object sender, EventArgs e)
        {
            LBLHesapNo.Text = hesapno;
            SqlCommand komut = new SqlCommand("Select * From TBLKisiler Where HESAPNO=@p1", baglan.baglanti());
            komut.Parameters.AddWithValue("@p1", hesapno);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                LBLAdSoyad.Text = dr[1] + " " + dr[2];
                ad = dr[1].ToString();
                soyad = dr[2].ToString();
                LBLTelefon.Text = dr[4].ToString();
                LBLTcKimlik.Text = dr[3].ToString();
            }
            SqlCommand komut1 = new SqlCommand("Select * From TBLHesap Where HESAPNO=@h1", baglan.baglanti());
            komut1.Parameters.AddWithValue("@h1", hesapno);
            SqlDataReader dr1 = komut1.ExecuteReader();
            while(dr1.Read())
            {
                if (dr1[1].ToString() == "")
                {
                    LBLBakiye.Text = "0";
                }
                else
                {
                    LBLBakiye.Text = dr1[1].ToString();
                }
            }
        }

        private void BTNGonder_Click(object sender, EventArgs e)
        {
            SqlCommand komutsorgula = new SqlCommand("Select HESAPNO From TBLHesap Where HESAPNO=@p1", baglan.baglanti());
            komutsorgula.Parameters.AddWithValue("@p1", TXTHesapNo.Text);
            SqlDataReader droku = komutsorgula.ExecuteReader();
            if(droku.Read())
            {
                if (TXTHesapNo.Text == LBLHesapNo.Text)
                {
                    MessageBox.Show("Kendinize para gönderemezsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand komutbakiye = new SqlCommand("Select BAKİYE From TBLHesap Where HESAPNO=@p1", baglan.baglanti());
                    komutbakiye.Parameters.AddWithValue("@p1", LBLHesapNo.Text);
                    SqlDataReader dr = komutbakiye.ExecuteReader();
                    while (dr.Read())
                    {
                        if (Convert.ToDecimal(dr[0].ToString()) < Convert.ToDecimal(TXTTutar.Text))
                        {
                            MessageBox.Show("Bakiye Yeterli Değil!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //GÖNDEREN HESAP
                            SqlCommand komutgonderici = new SqlCommand("Update TBLHesap set BAKİYE=BAKİYE-@p1 Where HESAPNO=@p2", baglan.baglanti());
                            komutgonderici.Parameters.AddWithValue("@p1", Convert.ToDecimal(TXTTutar.Text));
                            komutgonderici.Parameters.AddWithValue("@p2", LBLHesapNo.Text);
                            komutgonderici.ExecuteNonQuery();
                            baglan.baglanti().Close();

                            //ALICI HESAP
                            SqlCommand komutalici = new SqlCommand("Update TBLHesap set BAKİYE=BAKİYE+@p1 Where HESAPNO=@p2", baglan.baglanti());
                            komutalici.Parameters.AddWithValue("@p1", Convert.ToDecimal(TXTTutar.Text));
                            komutalici.Parameters.AddWithValue("@p2", TXTHesapNo.Text);
                            komutalici.ExecuteNonQuery();
                            baglan.baglanti().Close();

                            //HAREKET TABLOSUNA EKLEME
                            SqlCommand komutharekettablosu = new SqlCommand("Insert into TBLHAREKET(GONDEREN,ALICI,TUTAR) Values(@p1,@p2,@p3)", baglan.baglanti());
                            komutharekettablosu.Parameters.AddWithValue("@p1", LBLHesapNo.Text);
                            komutharekettablosu.Parameters.AddWithValue("@p2", TXTHesapNo.Text);
                            komutharekettablosu.Parameters.AddWithValue("@p3", Convert.ToDecimal(TXTTutar.Text));
                            komutharekettablosu.ExecuteNonQuery();
                            baglan.baglanti().Close();

                            decimal bakiye = Decimal.Parse(LBLBakiye.Text);
                            decimal tutar = int.Parse(TXTTutar.Text);
                            LBLBakiye.Text = (bakiye - tutar).ToString();

                            MessageBox.Show("İşleminiz Tamamlanmıştır.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Girilen hesap numarası sistemde kayıtlı değil!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LLBLHesapHareket_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HesapHareketleri frmhareket = new HesapHareketleri();
            frmhareket.hesapno = LBLHesapNo.Text;
            frmhareket.Show();
            this.Hide();
        }

        private void LLBLDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GuncellemeEklemeEkrani frmguncelle = new GuncellemeEklemeEkrani();
            frmguncelle.sifre = sifre;
            frmguncelle.giristuru = "Güncelle";
            frmguncelle.ad = ad;
            frmguncelle.soyad = soyad;
            frmguncelle.hesapno = LBLHesapNo.Text;
            frmguncelle.telefon = LBLTelefon.Text;
            frmguncelle.tc = LBLTcKimlik.Text;
            frmguncelle.Show();
            this.Hide();
        }
    }
}
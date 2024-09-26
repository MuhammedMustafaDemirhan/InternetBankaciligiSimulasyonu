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
    public partial class HesapHareketleri : Form
    {
        public HesapHareketleri()
        {
            InitializeComponent();
        }

        public string hesapno;

        Baglanti baglan = new Baglanti();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HesapHareketEkrani frm = new HesapHareketEkrani();
            frm.hesapno = hesapno;
            this.Hide();
            frm.Show();
        }

        private void HesapHareketleri_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select TBLKisiler.AD + ' ' + TBLKisiler.SOYAD as 'Alıcı Kişi',TUTAR as 'Tutar' From TBLHareket Inner Join TBLKisiler on TBLKisiler.HESAPNO = TBLHareket.ALICI Where GONDEREN=" + hesapno, baglan.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            SqlDataAdapter da1 = new SqlDataAdapter("Select TBLKisiler.AD + ' ' + TBLKisiler.SOYAD as 'Alıcı Kişi',TUTAR as 'Tutar' From TBLHareket Inner Join TBLKisiler on TBLKisiler.HESAPNO = TBLHareket.GONDEREN Where ALICI=" + hesapno, baglan.baglanti());
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView2.DataSource = dt1;
        }
    }
}
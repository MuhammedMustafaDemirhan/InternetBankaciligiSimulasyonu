using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BankamatikSimilasyonu
{
    internal class Baglanti
    {
        public SqlConnection baglanti()
        {
            SqlConnection bgl = new SqlConnection(@"Data Source=DESKTOP-CJ1512V;Initial Catalog=DbBankamatikSm;Integrated Security=True");
            bgl.Open();
            return bgl;
        }
    }
}
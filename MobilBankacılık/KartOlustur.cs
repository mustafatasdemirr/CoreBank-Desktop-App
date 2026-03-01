using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static MobilBankacılık.Form1;

namespace MobilBankacılık
{
    public partial class KartOlustur : Form
    {
        public KartOlustur()
        {
            InitializeComponent();
            Tasarim.Uygula(this);
        }

        private void KartOlustur_Load_1(object sender, EventArgs e)
        {
            cmbKartTürü.Items.Clear();
            cmbKartTürü.Items.Add("Banka Kartı");
            cmbKartTürü.Items.Add("Kredi Kartı");
            cmbKartTürü.Items.Add("Sanal Kart");
        }

        private void btnKartOlustur_Click(object sender, EventArgs e)
        {
            if (cmbKartTürü.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir kart türü seçiniz.", "Uyarı");
                return;
            }

            string kartTürü = cmbKartTürü.SelectedItem.ToString();
            int mevcutKullanıcıID = KullaniciBilgileri.KullaniciID;

            int krediKartıSayısı = GetKullanıcıKrediKartıSayısı(mevcutKullanıcıID);

            if (kartTürü == "Kredi Kartı" && krediKartıSayısı >= 3)
            {
                MessageBox.Show("Bir kullanıcı en fazla 3 adet kredi kartı oluşturabilir.", "Hata");
                return;
            }

            decimal toplamVarlik = GetKullanıcıToplamBakiye(mevcutKullanıcıID);
            decimal bakiye = 0;
            decimal limit = 0;

            switch (kartTürü)
            {
                case "Banka Kartı":

                    bakiye = toplamVarlik;
                    limit = 0;
                    break;

                case "Kredi Kartı":
                    bakiye = 0;

                    if (krediKartıSayısı == 0) limit = toplamVarlik * 0.5m;
                    else if (krediKartıSayısı == 1) limit = toplamVarlik * 0.3m;
                    else limit = toplamVarlik * 0.2m;

                    if (limit == 0) limit = 5000;
                    break;

                case "Sanal Kart":
                    bakiye = 0;
                    limit = 2000;
                    break;
            }

            string kartNumarası = GenerateKartNumarası();
            int cvv = GenerateCVV();
            DateTime sonKullanma = DateTime.Now.AddYears(5);

            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Kartlar 
                                   (KullanıcıID, KartNumarası, KartTuru, CVV, KartLimit, Bakiye, SonKullanmaTarihi) 
                                   VALUES 
                                   (@kid, @no, @tur, @cvv, @lim, @bak, @skt)";

                    SqlCommand komut = new SqlCommand(query, conn);
                    komut.Parameters.AddWithValue("@kid", mevcutKullanıcıID);
                    komut.Parameters.AddWithValue("@no", kartNumarası);
                    komut.Parameters.AddWithValue("@tur", kartTürü);
                    komut.Parameters.AddWithValue("@cvv", cvv);
                    komut.Parameters.AddWithValue("@lim", limit);
                    komut.Parameters.AddWithValue("@bak", bakiye);
                    komut.Parameters.AddWithValue("@skt", sonKullanma);

                    komut.ExecuteNonQuery();

                    MessageBox.Show($"{kartTürü} başarıyla oluşturuldu!\n\nKart No: {kartNumarası}\nLimit: {limit:C2}", "Başarılı");

                    txtKartNo.Text = kartNumarası;
                    txtKartLimit.Text = limit.ToString("C2");
                    txtKartCVV.Text = cvv.ToString();
                    txtKartBakiye.Text = bakiye.ToString("C2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kart oluşturma hatası: " + ex.Message);
                }
            }
        }
        private int GetKullanıcıKrediKartıSayısı(int uid)
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Kartlar WHERE KullanıcıID = @id AND KartTuru = 'Kredi Kartı'", conn);
                    cmd.Parameters.AddWithValue("@id", uid);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch { return 0; }
            }
        }

        private decimal GetKullanıcıToplamBakiye(int uid)
        {
            decimal toplam = 0;
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdTL = new SqlCommand("SELECT SUM(Bakiye) FROM Hesaplar WHERE KullanıcıID=@id AND HesapTürüID IN (1, 2)", conn);
                    cmdTL.Parameters.AddWithValue("@id", uid);
                    var resTL = cmdTL.ExecuteScalar();
                    if (resTL != DBNull.Value) toplam += Convert.ToDecimal(resTL);

                    SqlCommand cmdUSD = new SqlCommand("SELECT SUM(Bakiye) FROM Hesaplar WHERE KullanıcıID=@id AND HesapTürüID = 3", conn);
                    cmdUSD.Parameters.AddWithValue("@id", uid);
                    var resUSD = cmdUSD.ExecuteScalar();
                    if (resUSD != DBNull.Value) toplam += Convert.ToDecimal(resUSD) * 43;


                    SqlCommand cmdGold = new SqlCommand("SELECT SUM(Bakiye) FROM Hesaplar WHERE KullanıcıID=@id AND HesapTürüID = 4", conn);
                    cmdGold.Parameters.AddWithValue("@id", uid);
                    var resGold = cmdGold.ExecuteScalar();
                    if (resGold != DBNull.Value) toplam += Convert.ToDecimal(resGold) * 6250;
                }
                catch { }
            }
            return toplam;
        }

        private string GenerateKartNumarası()
        {
            Random rnd = new Random();

            return $"{rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)}";
        }

        private int GenerateCVV()
        {
            return new Random().Next(100, 999);
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static MobilBankacılık.Form1;

namespace MobilBankacılık
{
    public partial class HesapOluştur : Form
    {
        public HesapOluştur()
        {
            InitializeComponent();
            Tasarim.Uygula(this);
            TurleriYukle();
        }

        private void TurleriYukle()
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TürAdı FROM HesapTürü", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    listboxHesap.Items.Clear();
                    while (dr.Read())
                    {
                        listboxHesap.Items.Add(dr["TürAdı"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hesap türleri yüklenirken hata: " + ex.Message);
                }
            }
        }

        private void BtnHesapOlustur_Click(object sender, EventArgs e)
        {
            if (listboxHesap.SelectedItem == null)
            {
                MessageBox.Show("Lütfen listeden bir hesap türü seçiniz.", "Uyarı");
                return;
            }

            string turAdi = listboxHesap.SelectedItem.ToString();

            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdId = new SqlCommand("SELECT HesapTürüID FROM HesapTürü WHERE TürAdı=@ad", conn);
                    cmdId.Parameters.AddWithValue("@ad", turAdi);

                    object result = cmdId.ExecuteScalar();
                    if (result == null) return;

                    int turId = Convert.ToInt32(result);

                    Random rnd = new Random();

                    string part1 = rnd.Next(10000000, 99999999).ToString();
                    string part2 = rnd.Next(10000000, 99999999).ToString();

                    string iban = "TR" + "56" + part1 + part2 + "01";

                    string hesapNo = rnd.Next(100000, 999999).ToString();

                    string doviz = "TRY";
                    if (turId == 3) doviz = "USD";
                    else if (turId == 4) doviz = "GR";

                    string sql = @"INSERT INTO Hesaplar 
                                  (KullanıcıID, HesapTürüID, IBAN, HesapNo, DövizCinsi, Bakiye, OluşturmaTarihi) 
                                  VALUES 
                                  (@kid, @tid, @iban, @no, @dov, 0, @tarih)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@kid", KullaniciBilgileri.KullaniciID);
                    cmd.Parameters.AddWithValue("@tid", turId);
                    cmd.Parameters.AddWithValue("@iban", iban);
                    cmd.Parameters.AddWithValue("@no", hesapNo);
                    cmd.Parameters.AddWithValue("@dov", doviz);
                    cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"{turAdi} başarıyla oluşturuldu!\nHesap No: {hesapNo}", "Başarılı");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hesap oluşturulurken hata: " + ex.Message);
                }
            }
        }
        private void HesapOluştur_Load(object sender, EventArgs e) { }
    }
}
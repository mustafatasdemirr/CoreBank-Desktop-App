using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace MobilBankacılık
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Tasarim.Uygula(this);
            MskdTxtTC.MaxLength = 11;
            TxtBoxSifreKayit.UseSystemPasswordChar = true;
        }

        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private void BtnKayit_Click(object sender, EventArgs e)
        {
            if (MskdTxtTC.Text.Length != 11) { MessageBox.Show("TC 11 hane olmalıdır."); return; }

            string salt = GenerateSalt();
            string hashed = Form1.HashPassword(TxtBoxSifreKayit.Text, salt);

            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Kullanıcılar (Isim, Soyad, Telefon, SifreHash, SifreSalt, TC) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@p1", TxtBoxIsim.Text);
                        cmd.Parameters.AddWithValue("@p2", txtBoxSoyisim.Text);
                        cmd.Parameters.AddWithValue("@p3", MskdTxtTel.Text);
                        cmd.Parameters.AddWithValue("@p4", hashed);
                        cmd.Parameters.AddWithValue("@p5", salt);
                        cmd.Parameters.AddWithValue("@p6", MskdTxtTC.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Kayıt Başarılı! Giriş yapabilirsiniz.");
                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show("Kayıt Hatası: " + ex.Message); }
            }
        }
    }
}
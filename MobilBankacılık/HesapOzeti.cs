using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static MobilBankacılık.Form1;

namespace MobilBankacılık
{
    public partial class HesapOzeti : Form
    {
        public HesapOzeti()
        {
            InitializeComponent();
            Tasarim.Uygula(this);
        }

        private void HesapOzeti_Load(object sender, EventArgs e)
        {
            HesaplariYukle();
        }

        private void HesaplariYukle()
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IBAN, HesapID, HesapNo, Bakiye, HesapTürüID FROM Hesaplar WHERE KullanıcıID = @kullaniciID";

                    SqlCommand komut = new SqlCommand(query, conn);
                    komut.Parameters.AddWithValue("@kullaniciID", KullaniciBilgileri.KullaniciID);

                    SqlDataReader reader = komut.ExecuteReader();
                    cmbHesaplar.Items.Clear();

                    while (reader.Read())
                    {
                        int turID = Convert.ToInt32(reader["HesapTürüID"]);
                        string birim = "TL";

                        if (turID == 3) birim = "$";
                        else if (turID == 4) birim = "GR";

                        cmbHesaplar.Items.Add(new ComboItem
                        {
                            Text = $"{reader["HesapNo"]} - {reader["Bakiye"]} {birim}",
                            ID = Convert.ToInt32(reader["HesapID"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hesaplar yüklenirken hata: " + ex.Message);
                }
            }
        }
        private void cmbHesaplar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHesaplar.SelectedItem == null) return;

            ComboItem selected = cmbHesaplar.SelectedItem as ComboItem;
            int hesapID = selected.ID;

            OdemeleriYukle(hesapID);
            GonderilenTransferleriYukle(hesapID);
            GelenTransferleriYukle(hesapID);
            OzetYukle(hesapID);
        }

        private void GelenTransferleriYukle(int hesapID)
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT TransferTarihi, Miktar, Aciklama, 
                                   (SELECT IBAN FROM Hesaplar WHERE Hesaplar.HesapID = ParaTransferleri.GonderenHesapID) AS Gonderen
                                   FROM ParaTransferleri WHERE AlanHesapID = @hesapID";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@hesapID", hesapID);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dgvGelenTransferler.DataSource = dt;
                }
                catch { }
            }
        }

        private void GonderilenTransferleriYukle(int hesapID)
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT TransferTarihi, Miktar, Aciklama, 
                                   (SELECT IBAN FROM Hesaplar WHERE Hesaplar.HesapID = ParaTransferleri.AlanHesapID) AS Alici
                                   FROM ParaTransferleri WHERE GonderenHesapID = @hesapID";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@hesapID", hesapID);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dgvGonderilenTransferler.DataSource = dt;
                }
                catch { }
            }
        }

        private void OdemeleriYukle(int hesapID)
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT OdemeTarihi, Tutar, 
                                   (SELECT OdemeTuru FROM OdemeTurleri WHERE OdemeTurleri.OdemeTuruID = Odemeler.OdemeTuruID) AS Tur
                                   FROM Odemeler WHERE HesapID = @hesapID";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@hesapID", hesapID);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dgvOdemeler.DataSource = dt;
                }
                catch { }
            }
        }

        private void OzetYukle(int hesapID)
        {
            using (SqlConnection conn = Veritabani.BaglantiGetir())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT İşlemTarihi, İşlemTürü, Tutar, Açıklama FROM HesapÖzeti WHERE HesapID = @hesapID ORDER BY İşlemTarihi DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@hesapID", hesapID);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dgvAltin.DataSource = dt;
                }
                catch { }
            }
        }
        public class ComboItem
        {
            public string Text { get; set; }
            public int ID { get; set; }
            public override string ToString() { return Text; }
        }
    }
}
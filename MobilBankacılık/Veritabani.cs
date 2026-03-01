using System.Data.SqlClient;

namespace MobilBankacılık
{
    public static class Veritabani
    {
        private static string connectionString = "Data Source=PCOFJACK;Initial Catalog=Banka;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection BaglantiGetir()
        {
            return new SqlConnection(connectionString);
        }
    }
}
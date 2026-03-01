# 🏦 CoreBank - Gelişmiş Masaüstü Bankacılık Simülasyonu

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Framework 4.7.2](https://img.shields.io/badge/.NET%20Framework-4.7.2-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![ADO.NET](https://img.shields.io/badge/ADO.NET-Data%20Access-blue)

**CoreBank**, C# Windows Forms ve MSSQL kullanılarak sıfırdan geliştirilmiş, gerçek dünya bankacılık süreçlerini simüle eden kapsamlı bir masaüstü uygulamasıdır. 

Bu proje, yalnızca basit bir kullanıcı arayüzü tasarımı değil; arka planda çalışan bakiye kontrolleri, hesaplar arası tutarlı transfer mantığı ve detaylı loglama mekanizmalarıyla tam teşekküllü bir finansal altyapı sunmayı hedeflemektedir.

<div align="center">
  <table border="0">
    <tr>
      <td align="center" width="50%">
        <img src="Images/login.png" alt="CoreBank Giriş Ekranı" width="100%"><br>
        <em>Güvenli Giriş ve Kayıt Modülü</em>
      </td>
      <td align="center" width="50%">
        <img src="Images/dashboard.png" alt="CoreBank Ana Sayfa" width="100%"><br>
        <em>Kullanıcı Paneli ve Dinamik Menü</em>
      </td>
    </tr>
  </table>
</div>

## 🚀 Detaylı Sistem Özellikleri

### 👤 Müşteri ve Hesap Yönetimi
* **Kimlik Doğrulama:** TC Kimlik Numarası ve şifre ile güvenli giriş. Yeni müşteriler için sisteme kayıt modülü.
* **Çoklu Hesap Desteği:** Tek bir müşteriye bağlı bağımsız Vadesiz TL, Vadeli TL, Dolar ve Altın hesapları tanımlayabilme.
* **Bakiye ve Güvenlik Kontrolü:** Tüm işlemlerde anlık bakiye yetersizlik kontrolü ve hata yönetimi (Exception Handling).

### 💸 Finansal İşlemler
* **EFT/Havale Simülasyonu:** Alıcı IBAN, Ad-Soyad doğrulaması ve açıklama ekleyerek hesaplar arası tutarlı para transferi.
* **Fatura ve Kurum Ödemeleri:** İnternet, elektrik, su ve dijital platform (Netflix vb.) ödemelerinin gerçekleştirilip ana bakiyeden anında düşülmesi.

### 💱 Yatırım ve Emtia Piyasası
* **Döviz İşlemleri:** Güncel kurlar üzerinden hesaplanarak TL hesabından Dolar (USD) alma veya Dolardan TL'ye dönme.
* **Altın İşlemleri:** Gram bazında altın alım-satımı ve anlık portföy yansıması.

### 💳 Kart Servisleri ve Raporlama
* **Kart Oluşturma:** Kullanıcıya özel Kredi Kartı veya Banka Kartı tanımlama, limit belirleme ve CVV ataması.
* **Hesap Özeti (Loglama Sistemi):** Kullanıcının yaptığı tüm transferlerin, kurum ödemelerinin ve döviz işlemlerinin veritabanına kaydedilmesi ve tek bir panelde listelenmesi.

## 🛠️ Teknik Mimari ve Veritabanı Tasarımı

Proje, verilerin güvenliğini ve tutarlılığını sağlamak için **ADO.NET** kullanılarak MSSQL'e bağlanmaktadır. SQL sorgularında ilişkisel veritabanı (Relational Database) mantığı benimsenmiştir.

* **Kullanılan Teknolojiler:** C#, Windows Forms, MSSQL, ADO.NET (SqlDataReader, SqlCommand).
* **Veritabanı Yapısı:** Sistemde Müşteriler, Hesaplar, Kartlar ve İşlem Geçmişi (Hareketler) için ayrı ve birbirleriyle (Primary Key / Foreign Key) bağlantılı tablolar yer almaktadır.

## ⚙️ Kurulum ve Geliştirici Rehberi

Projeyi kendi bilgisayarınızda derlemek ve test etmek için:

1. **Repoyu Klonlayın:**
   ```bash
   git clone [https://github.com/mustafatasdemirr/CoreBank-Desktop-App.git](https://github.com/mustafatasdemirr/CoreBank-Desktop-App.git)
2. **Veritabanını Hazırlayın:**
   * SQL Server Management Studio (SSMS) programını açın.
   * Proje dizininde bulunan `BankDatabase.sql` dosyasını SSMS üzerinde çalıştırarak gerekli tabloları ve örnek verileri otomatik oluşturun.

3. **Bağlantı Ayarlarını Yapın:**
   * Visual Studio'da projeyi açın.
   * Projedeki SQL bağlantı dizesini (`Data Source=...`) kendi yerel SQL Server adınıza göre güncelleyin.

4. **Çalıştırın:** * Çözümü (Solution) derleyip projeyi başlatın.

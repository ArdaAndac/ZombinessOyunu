# 🧟 Zombiness: Interaktif Hayatta Kalma RPG

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Language](https://img.shields.io/badge/language-C%23-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6.svg)
![Status](https://img.shields.io/badge/status-Stable-success.svg)

> **"Tek bir yanlış karar, herkesin sonunu getirebilir."**

**Zombiless**, C# ve .NET Framework kullanılarak geliştirilmiş, **WinForms** tabanlı, hikaye odaklı bir hayatta kalma (Survival) oyunudur. Standart metin tabanlı oyunların aksine; dinamik görselleştirmeler, **Kelebek Etkisi (Butterfly Effect)** algoritması ve derinlemesine **Karakter İlişki Sistemi** içerir.

---

## 🌟 Öne Çıkan Özellikler

### 🦋 Kelebek Etkisi ve Dinamik Senaryo
Oyun, doğrusal bir çizgide ilerlemez. 1. Bölümde verdiğiniz küçük bir karar (örn: birine yiyecek vermek), 5. Bölümde o kişinin hayatta kalmasını veya size ihanet etmesini belirler.
* **Teknik Detay:** `SenaryoYoneticisi` sınıfı, geçmiş kararları (`KararGecmisi`) analiz ederek hikaye akışını çalışma zamanında (Runtime) yeniden yazar.

### ❤️ Gelişmiş İlişki Sistemi
Yanınızdaki NPC'ler (Nathan, Isla, Olivia) sadece birer figüran değildir. Sizinle olan ilişkileri matematiksel olarak hesaplanır.
* **Teknik Detay:** C# **Operator Overloading** kullanılarak, karakter nesneleri üzerinde matematiksel işlemler (örn: `Olivia + 20`) yapılarak kod okunabilirliği artırılmıştır.

### 🎒 Envanter ve Kaynak Yönetimi
Bulduğunuz eşyalar (Harita, Silah, Kimlik Kartı) sadece süs değildir; kilitli senaryo yollarını açar.
* **Teknik Detay:** `IEsya` arayüzü (Interface) sayesinde genişletilebilir, polimorfik bir envanter yapısı kurulmuştur.

### 💾 Veritabanısız Kayıt Sistemi (No-SQL)
Oyun, herhangi bir SQL sunucusuna ihtiyaç duymadan çalışır.
* **Teknik Detay:** Oyun durumu ve karakter verileri, özel bir serileştirme algoritması ile yerel diske (`/Veriler/kayit.sav`) şifreli bir formatta kaydedilir.

---

## 🛠️ Teknik Mimari ve Tasarım

Bu proje, bir "Spagetti Kod" yığını değil, **Nesne Yönelimli Programlama (OOP)** prensiplerine uygun bir mühendislik ürünüdür.

### Kullanılan Teknolojiler
* **Dil:** C# (.NET Framework 4.8)
* **UI:** Windows Forms (Dinamik Kontrol Üretimi)
* **Veri:** In-Memory Database & File I/O
* **Versiyon Kontrol:** Git & GitHub

### Tasarım Desenleri (Design Patterns)
1.  **Manager Pattern:** `SenaryoYoneticisi`, `BasarimYoneticisi` ve `SesYoneticisi` sınıfları merkezi yönetim sağlar.
2.  **Observer Pattern:** İlişki puanı değiştiğinde Arayüzü (UI) uyarmak için **Event** ve **Delegate** (`Action<string, int>`) yapıları kullanılmıştır.
3.  **Polymorphism:** `ISahne` arayüzü sayesinde normal sahneler ve `SavasSahnesi` (Boss Fight) aynı koleksiyon içinde yönetilir.

---

## 📸 Ekran Görüntüleri

| Ana Menü | Oyun İçi (Karar Anı) |
| :---: | :---: |
| ![Ana Menü](https://via.placeholder.com/400x225?text=Ana+Menu) | ![Oyun İçi](https://via.placeholder.com/400x225?text=Karar+Ani) |

| İlişki Sistemi | Boss Savaşı |
| :---: | :---: |
| ![İlişki](https://via.placeholder.com/400x225?text=Iliski+Sistemi) | ![Savaş](https://via.placeholder.com/400x225?text=Boss+Savasi) |

*(Not: Ekran görüntüleri geliştirme aşamasından alınmıştır.)*

---

## 🚀 Kurulum ve Çalıştırma

Bu proje **"Tak-Çalıştır"** mantığıyla tasarlanmıştır. Veritabanı kurulumu gerektirmez.

### Gereksinimler
* Windows İşletim Sistemi (10/11)
* Visual Studio 2019 veya 2022

### Adım Adım Kurulum
1.  **Depoyu Klonlayın:**
    ```bash
    git clone [https://github.com/ArdaAndac/Zombiless-RPG.git](https://github.com/ArdaAndac/Zombiless-RPG.git)
    ```
2.  **Projeyi Açın:**
    Klasör içindeki `AnimeOyunu.sln` dosyasına çift tıklayarak Visual Studio'da açın.
3.  **Başlatın:**
    Visual Studio üst menüsündeki **"Start"** (veya F5) tuşuna basın.
4.  **Hazırsınız!**
    Gerekli veri klasörleri (`/Veriler`) oyun tarafından otomatik oluşturulacaktır.

---

## 🎮 Nasıl Oynanır?

1.  **Seçim Yapın:** Hikaye akışına göre karşınıza çıkan butonlara tıklayın. Dikkatli olun, süreniz kısıtlı olabilir! (Yeşil zaman barına dikkat).
2.  **İlişkileri Yönetin:** Yan menüyü (Sol taraf) kullanarak karakterlerin size olan güvenini kontrol edin. Düşük güven, ihanet demektir.
3.  **Hayatta Kalın:** Enfekte olmaktan kaçının. Eğer enfekte olursanız, oyunun sonu değişecektir.

---

## 📂 Proje Yapısı
Zombiness-RPG/

Zombiless-RPG/

├── AnimeOyunu/

│   ├── Gorseller/           # Oyun içi sahne resimleri ve görsel varlıklar

│   ├── Sesler/              # Ses efektleri ve arka plan müzikleri

│   ├── Veriler/             # Runtime (Çalışma anı) oluşan Save ve Log dosyaları

│   ├── SenaryoYoneticisi.cs # [Backend] Oyun mantığı ve veri yönetimi

│   ├── BasarimYoneticisi.cs # [Backend] Karar geçmişi ve başarımlar

│   ├── Ogrenci.cs           # [Model] Karakter verileri ve operatör işlemleri

│   ├── EsyaSistemi.cs       # [Model] Envanter ve eşya arayüzleri

│   ├── Form1.cs             # [Frontend] Ana oyun ekranı ve dinamik UI

│   ├── MenuForm.cs          # [Frontend] Giriş menüsü

│   └── Program.cs           # Uygulama giriş noktası

└── README.md                # Kurulum ve tanıtım dokümantasyonu

## 👤 Geliştirici

**Arda Andaç**
* GitHub: [@ArdaAndac](https://github.com/ArdaAndac)
* Proje: Üniversite Dönem Projesi / Oyun Geliştirme

---

## 📄 Lisans

Bu proje [MIT](LICENSE) lisansı ile lisanslanmıştır. Eğitim ve geliştirme amaçlı özgürce kullanılabilir.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnimeOyunu
{
    public interface ISahne { int Id { get; set; } string HikayeMetni { get; set; } int Sure { get; set; } } // interface polimorfizm açıkla
     

    public class Sahne : ISahne // propertys
    {
        public Func<string> DinamikMetinHazirla { get; set; } //func nedir neden kullandım ne döndürür?
        public Action Secenek1Aksiyonu { get; set; } //action ne tutar neden bunu kullandım??
        public Action Secenek2Aksiyonu { get; set; }
        public string SesDosyasi { get; set; }
        public int Id { get; set; }
        public string HikayeMetni { get; set; }
        public string ResimAdi { get; set; }
        public int Sure { get; set; } = 60;
        public string Secenek1Metni { get; set; }
        public int Secenek1HedefId { get; set; }
        public string Secenek1Karakter { get; set; }
        public int Secenek1PuanEtkisi { get; set; }
        public string Secenek1Ipucu { get; set; }
        public string Secenek2Metni { get; set; }
        public int Secenek2HedefId { get; set; }
        public string Secenek2Karakter { get; set; }
        public int Secenek2PuanEtkisi { get; set; }
        public string Secenek2Ipucu { get; set; }
        public Action OlayAksiyonu { get; set; }
    }

    public class SavasSahnesi : Sahne
    {
        public string DusmanIsmi { get; set; }
        public int DusmanCan { get; set; }
        public int ZaferSonrasiHedefId { get; set; }
    }

    public static class SenaryoYoneticisi 
    {
        public enum OliviaState { Saglikli, Enfekte, Olu }
        public static bool IslaOlduMu = false;  
        public static bool NathanOlduMu = false; 
        public static OliviaState MevcutOlivia = OliviaState.Saglikli;
        public static ISahne AktifSahne;
        public static List<Ogrenci> SinifListesi = new List<Ogrenci>(); //list mantığını neden kullandım
        public static List<ISahne> Sahneler = new List<ISahne>();
        

        public static event Action<string, int> OnIliskiDegisti; 
                                                                 
        public static void SinyalGonder(string isim, int puan)
        {
            
            OnIliskiDegisti?.Invoke(isim, puan);
        }

       

        public static List<string> OyunLoglari = new List<string>();


        public static void LogEkle(string mesaj)
        {
            
            string zaman = DateTime.Now.ToString("HH:mm");
            OyunLoglari.Add($"[{zaman}] {mesaj}");
        }

        public static string LoglariGetir()
        {
            if (OyunLoglari.Count == 0) return "Henüz bir kayıt yok.";

            

            List<string> tersLog = new List<string>(OyunLoglari);
            tersLog.Reverse();//string.Reverse fonksiyonu

            return string.Join("\n\n", tersLog);//string.Join fonksiyonu
        } 

        public static void OyunuKaydet()

        {
            try
            {
                if (AktifSahne == null) throw new Exception("Aktif sahne bulunamadı!");
                File.WriteAllText("kayit.sav", $"{AktifSahne.Id}|{(int)MevcutOlivia}");
            }
            catch (Exception ex) { throw new Exception($"Kayıt hatası: {ex.Message}"); }
        }

        public static int OyunuYukle()
        {
            try
            {
                if (File.Exists("kayit.sav")) //kayıt dosyası olusturdu
                {
                    var d = File.ReadAllText("kayit.sav").Split('|');
                    if (d.Length >= 2)
                    {
                        MevcutOlivia = (OliviaState)int.Parse(d[1]);
                        return int.Parse(d[0]);
                    }
                }
            }
            catch (Exception ex) { throw new Exception($"Yükleme hatası: {ex.Message}"); }
            return 1;
        }

        public static string MevcutMetniGetir(ISahne sahne)
        {
            if (sahne == null) return "";
            return sahne.HikayeMetni;
        }

       

        public static ISahne SahneGetir(int id) //kapsülleme
        {

            var s = Sahneler.Find(x => x.Id == id);//lınq mantığı nedir neden foreach yerine bunu kullandım

            
            if (s != null && s is Sahne sahne) // dinamik if else
            {
                if (sahne.DinamikMetinHazirla != null)
                {
                    sahne.HikayeMetni = sahne.DinamikMetinHazirla();
                }
                // 
                if (id == 35)
                {
                    if (MevcutOlivia == OliviaState.Enfekte)
                    {
                      
                        sahne.ResimAdi = "sahne35_romantizm_olivia_enfekte.png";
                        sahne.HikayeMetni = "Olivia'nın yanına gittiğinde ter içinde titrediğini gördün. Enfeksiyon damarlarında siyah bir ağ gibi yayılıyor. Yakandan tuttu: 'Max... Dönüşmeme izin verme. Eğer o şeye dönüşürsem... beni sen öldür.'";
                    }
                    else
                    {
                        // Sağlıklı Durumu
                        sahne.ResimAdi = "sahne35_olivia_saglikli.png";
                        sahne.HikayeMetni = "Olivia sana gülümsedi ama gözlerinde yaşlar var. Elini tuttun. 'Bizi bırakmadığın için teşekkürler Max. Herkes vazgeçerken sen yanımdaydın.'";
                    }
                }

                // --- SAHNE 37: KARAR ANI (Hata veren yer düzeltildi) ---
                if (id == 37)
                {
                    if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        sahne.ResimAdi = "sahne36_olivia_karar.png"; // Dosya listende .png görünüyor
                        sahne.HikayeMetni = "Olivia yerde kriz geçiriyor. Dönüşüm başlamak üzere. Sana yalvaran gözlerle bakıyor: 'Beni o şeye dönüşmeden durdur... Yalvarırım.'";
                        sahne.Secenek1Metni = "Onu Yaşat (Risk Al)";
                        sahne.Secenek2Metni = "Tetiği Çek (Acısına Son Ver)";
                    }
                    else
                    {
                        sahne.ResimAdi = "sahne36_zafer.png";
                        sahne.HikayeMetni = "Başardık! Mutant öldü. Olivia hemen ilaçları topladı. 'Gidelim buradan Max!'";
                        sahne.Secenek1Metni = "Sığınağa Git";

                      
                        sahne.Secenek2Metni = "";
                    }
                }


                // --- SAHNE 50: SIĞINAK KAPISI ---
                if (id == 50)
                {
                    if (MevcutOlivia == OliviaState.Olu)
                    {
                        sahne.ResimAdi = "sahne50_olu.png"; 
                        sahne.HikayeMetni = "Olivia'nın cansız bedenini taşıdın. Kapıdaki asker 'Denek A-104 Ex olmuş' diyerek cesedi içeri aldı.";
                    }
                    else if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        sahne.ResimAdi = "sahne50_olu.png";
                        sahne.HikayeMetni = "Kapıdaki asker 'Denek A-104 tespit edildi' diyerek Olivia'ya yeşil bir sıvı enjekte etti. Kriz durdu ama bu bir tedavi değildi.";
                    }
                    else
                    {
                        sahne.ResimAdi = "sahne50_saglikli.png";
                        sahne.HikayeMetni = "Askerler bir tane kadına aldığımız ilaçla bir aşı oluşturup enjekte etti.Bu hiç normal değildi.";
                    }
                }

                // --- SAHNE 51: ODA ---
                if (id == 51)
                {
                    if (MevcutOlivia == OliviaState.Olu || MevcutOlivia == OliviaState.Enfekte)
                    {
                        sahne.ResimAdi = "sahne51_olu.png";
                        sahne.HikayeMetni = MevcutOlivia == OliviaState.Olu ?
                            "Odaya kilitlendiniz. Nath duvara vurdu: 'Cesedine bile saygı duymadılar!'" :
                            "Odaya kilitlendiniz. Olivia donuk gözlerle bakıyor.";
                    }
                    else
                    {
                        sahne.ResimAdi = "sahne51_saglikli.png";
                        sahne.HikayeMetni = "Odaya kilitlendiniz. Nath haritayı inceliyor: 'Bizi buraya çektiler.'";
                    }
                }

                // --- SAHNE 52: FİNAL ---
                if (id == 52)
                {
                    sahne.ResimAdi = (MevcutOlivia == OliviaState.Saglikli) ? "sahne52_saglikli.png" : "sahne52_olu.png";
                }

                // --- SAHNE 53: GERÇEKLER VE İSYAN KARARI ---
                if (id == 53)
                {
                    if (MevcutOlivia == OliviaState.Olu)
                    {
                        sahne.ResimAdi = "sahne53_olu.png"; // Veya enfekte görseli
                        sahne.HikayeMetni = "Kane'in dosyalarında Olivia sadece 'İmha Edilmiş Numune' olarak geçiyor. Burası bir sığınak değil, bir silah fabrikası. Tek çıkış yolu burayı başlarına yıkmak.";
                    }
                    else if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        sahne.ResimAdi = "sahne53_enfekte.png";
                        sahne.HikayeMetni = "Kane'in planı Olivia'yı 'Beta Üssü'ne götürüp tamamen bir silaha dönüştürmek. O bir hasta değil, bir prototip. Onu kurtarmanın tek yolu isyan başlatmak!";
                    }
                    else
                    {
                        sahne.ResimAdi = "sahne53_saglikli.png";
                        sahne.HikayeMetni = "Gerçekler korkunç. Olivia'nın bağışıklığı doğal değil, o bir deney ürünü. Kane onu damızlık olarak kullanmayı planlıyor. Bu laboratuvarı yok etmeliyiz!";
                    }
                }

                // --- SAHNE 54: NATH İLE YÜZLEŞME ---
                if (id == 54)
                {
                    // Nath'in puanını kontrol et
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    int nathPuan = nath != null ? nath.MoralPuani : 0;

                    // Görseli ayarla
                    if (MevcutOlivia == OliviaState.Enfekte) sahne.ResimAdi = "sahne54_enfekte.png";
                    else if (MevcutOlivia == OliviaState.Olu) sahne.ResimAdi = "sahne54_olu.png";
                    else sahne.ResimAdi = "sahne54_saglikli.png";

                    // Metni puana göre değiştir
                    if (nathPuan < 70)
                    {
                        sahne.HikayeMetni = "Nath silahını sana doğrulttu! 'Yeter artık Max! Senin yüzünden herkes ölüyor. Ben Kane'in tarafına geçiyorum. En azından o hayatta kalmayı biliyor.'";
                        sahne.Secenek1Metni = "Lanet Olsun... Koş!";
                    }
                    else
                    {
                        sahne.HikayeMetni = "Nath titreyen elleriyle silahı Kane'e çevirdi. 'Senden nefret ediyorum Max... Ama sensiz bu cehennemden çıkamayız. Hadi bitirelim şu işi.'";
                        sahne.Secenek1Metni = "Birlikte Kaç!";
                    }
                }

                // --- SAHNE 55: KAÇIŞ ---
                if (id == 55)
                {
                    if (MevcutOlivia == OliviaState.Olu)
                    {
                        sahne.ResimAdi = "sahne55_olu.png";
                        sahne.HikayeMetni = "Olivia'yı geride bıraktın. Alevler sığınağı yutarken, sadece hayatta kalmanın buruk tadı var ağzında.";
                    }
                    else if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        sahne.ResimAdi = "sahne55_enfekte.png";
                        sahne.HikayeMetni = "Dönüşmekte olan Olivia'yı sürükleyerek dışarı attın. İnsanlığını kaybediyor ama hala yaşıyor.";
                    }
                    else
                    {
                        sahne.ResimAdi = "sahne55_saglikli.png";
                        sahne.HikayeMetni = "Olivia'nın elini sımsıkı tutarak alevlerin arasından çıktın. Güneş doğuyor. Başardınız.";
                    }
                }
            }

            // 3. İlişki puanı etkilerini uygula (Otomatik)
            if (s != null)
            {
                AktifSahne = s;
                if (s is Sahne sahneObj && !string.IsNullOrEmpty(sahneObj.Secenek1Karakter) && sahneObj.Secenek1PuanEtkisi != 0)
                {
                    var karakter = SinifListesi.Find(x => x.Isim == sahneObj.Secenek1Karakter);
                    if (karakter != null) karakter.PuanDegistir(sahneObj.Secenek1PuanEtkisi);
                }
            }
            // İlişki Puanı ve Diğer İşlemler (Mevcut kodun)
            if (s != null)
            {
                AktifSahne = s;
                if (s is Sahne sahneObj)
                {
                    if (!string.IsNullOrEmpty(sahneObj.Secenek1Karakter) && sahneObj.Secenek1PuanEtkisi != 0)
                    {
                        var karakter = SinifListesi.Find(x => x.Isim == sahneObj.Secenek1Karakter);
                        if (karakter != null) karakter.PuanDegistir(sahneObj.Secenek1PuanEtkisi);
                    }
                }
            }

            return s;
        }

        public static string ListeyiGetir()
        {
            if (SinifListesi.Count == 0) return "Henüz karakter yok.";
            return "--- İLİŞKİLER ---\n\n" + string.Join("\n", SinifListesi.Select(x =>
                $"👤 {x.Isim} [{x.Rol}]\n   Moral: %{x.MoralPuani}\n   Durum: {x.IliskiDurumu}\n"));
        }


        public static void DurumuSifirla()
        {
            // Değişkenleri varsayılana döndür
            MevcutOlivia = OliviaState.Saglikli;
            IslaOlduMu = false;
            NathanOlduMu = false;
            AktifSahne = null;
            OyunLoglari.Clear();

            // Envanteri temizle
            Envanter.Esyalar.Clear();

            

            
            SinifListesi.Clear();
            SinifListesi.Add(new Ogrenci("Olivia", "Şifacı") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Nath", "Tank") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Isla", "Diplomat") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Dr. Kane", "Lider") { MoralPuani = 30 });
        }
        public static void SenaryoyuYukle()
        {
            if (Sahneler.Count > 0) return;

            // ===== KARAKTERLER (Kod yapına sadık kalındı) =====
            SinifListesi.Add(new Ogrenci("Olivia", "Şifacı") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Nath", "Tank") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Isla", "Diplomat") { MoralPuani = 50 });
            SinifListesi.Add(new Ogrenci("Dr. Kane", "Lider") { MoralPuani = 30 });
          
            Sahneler.Add(new Sahne
            {
                Id = -1,
                HikayeMetni = "Simülasyon başlatılıyor...\n\nRehberi okuduysan ve zihinsel olarak hazırsan, aşağıdaki butona basarak kabusa adım at.",
                ResimAdi = "", // Siyah ekran

                Secenek1Metni = "SİMÜLASYONU BAŞLAT",
                Secenek1HedefId = 99,
                Secenek1Ipucu = "Geri dönüş yok",

                OlayAksiyonu = () =>
                {
                    
                    var form = System.Windows.Forms.Application.OpenForms.OfType<Form1>().FirstOrDefault();

                    if (form != null)
                    {
                        string rehber = "⚠️ HAYATTA KALMA PROTOKOLLERİ ⚠️\n\n" +
                                        "⏳ ZAMAN BARINA DİKKAT ET:\n" +
                                        "Karar verirken süren kısıtlıdır. Eğer süre dolarsa oyun senin yerine (genellikle kötü) bir seçim yapar.\n\n" +
                                        "❤️ İLİŞKİLER HER ŞEYDİR:\n" +
                                        "Grup üyelerinin sana olan güveni (Moral Puanı) hikayeyi değiştirir. Sana güvenmeyen biri, kritik anda emrini dinlemeyebilir.\n\n" +
                                        "🎒 ENVANTERİNİ KULLAN:\n" +
                                        "Bulduğun eşyalar süs değildir. Doğru eşya, doğru sahnede yeni seçenekler açar.\n\n" +
                                        "🦋 KELEBEK ETKİSİ:\n" +
                                        "Şu an verdiğin önemsiz görünen bir karar, 3 bölüm sonra birinin ölümüne neden olabilir.";

                        form.OverlayGoster("SİMÜLASYON REHBERİ", rehber);
                    }
                }
            });

            // --- SAHNE 0: PROLOG (ATMOSFER GİRİŞİ) ---
            Sahneler.Add(new Sahne
            {
                Id = 99,  
                ResimAdi = "",
                HikayeMetni = "TARİH: 14 EKİM 2024\nSAAT: 08:42\nKONUM: NORTHWOOD LİSESİ\n\n" +
                              "Her şey o lanet siren sesiyle başladı. Önce bir tatbikat sandık... Sonra çığlıkları duyduk.\n\n" +
                              "Babam hep 'Hazırlıklı ol' derdi ama kimse buna hazırlıklı olamazdı. " +
                              "İnsanlar... dostlarım... birbirlerini parçalıyorlardı. " +
                              "Koridorlar kan gölüne, sınıflar mezbahaya döndü.\n\n" +
                              "Artık okul bitti. Sınav yok, gelecek yok.\n" +
                              "Sadece hayatta kalmak var.\n\n" +
                              "Nefesini tut Max. Başlıyoruz...",

                Secenek1Metni = "GÖZLERİNİ AÇ",
                Secenek1HedefId = 1, // Buradan Sahne 1'e (Sınıfa) geçecek
                Secenek1Ipucu = "Kabus Başlıyor"
            });
           
            Sahneler.Add(new Sahne
            {
                Id = 1,
                HikayeMetni = "O tiz siren sesi... Bildiğimiz dünyanın veda şarkısı gibi kulaklarımızı tırmaladı. Harrison koridorun karanlığında kaybolduğunda, sınıfa asılı kalan o ağır sessizliği kalemimin yere düşme sesi bozdu. Dışarıdaki o hayvansı hırıltılar sınıfa sızarken Max, hayatının en uzun saniyesindesin. Ne yapacaksın?",
                ResimAdi = "baslangic_sahnesi.png",
                Secenek1Metni = "Cama Yaklaş",
                Secenek1HedefId = 2,
                Secenek1Ipucu = "Gerçekle Yüzleş",
                Secenek2Metni = "Sıranın Altına Saklan",
                Secenek2HedefId = 3,
                Secenek2Ipucu = "Korkuya Teslim Ol",
                SesDosyasi = "gerilim_muzigi.wav",
            });

      
            Sahneler.Add(new Sahne
            {
                Id = 2,
                HikayeMetni = "Cama yaklaştığında gördüğün manzara ruhunu dondurdu. Bahçe, bir zamanlar gülen öğrencilerin değil, birbirini parçalayan gölgelerin mekanı olmuştu. Harrison'ın paltosunu yerde kanlar içinde gördün. Artık geri dönüş yok Max, kaos tam burada. Nath yanına gelip fısıldadı: 'Hemen bir plan yapmalıyız!'",
                ResimAdi = "sahne2_zombi.png",
                Secenek1Metni = "Arkadaşlarına Dön",
                Secenek1HedefId = 4,
                Secenek1Ipucu = "Grup Oluştur"
            });

            
            Sahneler.Add(new Sahne
            {
                Id = 3,
                HikayeMetni = "Sıranın altında titriyorsun. Koridordaki çığlıklar yaklaşıyor ve her darbe kapıyı biraz daha zorluyor. Nath seni omzundan tutup sarsana kadar zaman durmuş gibiydi. 'Max! Burada beklemek bizi sadece kolay av yapar. Kalk ve bize yardım et!' Artık saklanacak yer kalmadı.",
                ResimAdi = "baslangic_sahnesi.png",
                Secenek1Metni = "Gruba Katıl",
                Secenek1HedefId = 4,
                Secenek1Ipucu = "Birlikte Hareket Et"
            });

            Sahneler.Add(new Sahne
            {
                Id = 4,
                HikayeMetni = "Sınıfın geri kalanı çoktan kaçmış, sadece beş kişi kalmıştınız: Nath, Isla, Olivia, Dustin ve Sen. Nath elini sıktı: 'Korkuyorsun, hepimiz korkuyoruz ama buradan beraber çıkacağız.' Isla haritasını açmış babasının sığınağını gösteriyor. Olivia ise elleri titreyerek sana bakıyor. Kimi önceliklendireceksin?",
                ResimAdi = "sahne3_karar.png",
                Secenek1Metni = "Olivia'yı Teselli Et",
                Secenek1HedefId = 5,
                Secenek1Karakter = "Olivia",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(30); 
                    
                    BasarimYoneticisi.KararKaydet(4, "Olivia'yı teselli et",
                        "Olivia sana güvendi, ileride seni koruyacak", "Olivia", 30);
                },

                Secenek1Ipucu = "Ekip Moralini Yükselt",

                Secenek2Metni = "Isla'nın Planına Odaklan",
                Secenek2HedefId = 5,
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(30);

                    
                    BasarimYoneticisi.KararKaydet(4, "Isla'nın planına odaklan",
                        "Isla stratejini takdir etti", "Isla", 30);
                }
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 5,
                HikayeMetni = "Menteşelerin feryadı yankılandı. Kapı bir kâğıt gibi yırtılıp içeri o canavarlar doluştuğunda, Dustin'in arkada 'Yardım et!' diye bağırdığını duydun. Canavarların elleri Dustin'in omuzlarına dolanmış, onu karanlığa çekiyorlar. Max, Dustin'e elini uzatacak mısın yoksa kendi güvenliğin için geri mi çekileceksin?",
                ResimAdi = "sahne4_kapi.png",
                Secenek1Metni = "Dustin'e Elini Uzat",
                Secenek1HedefId = 6,
                Secenek1Ipucu = "Sadakat",
                Secenek2Metni = "Sadece Kapıyı Tut",
                Secenek2HedefId = 6,
                Secenek2Ipucu = "Soğukkanlılık"
            });

        
            Sahneler.Add(new Sahne
            {
                Id = 6,
                HikayeMetni = "Masaları kapıya doğru iterken kollarındaki kasların yandığını hissediyorsun. Nath haykırıyor: 'TUTMAMIZ LAZIM!' Ter ve kan kokusu odayı sarmış durumda. Kapının arkasındaki güç insanüstü, barikat her an patlayabilir. Bir çıkış yolu bulmalısınız, hemen!",
                ResimAdi = "sahne5_barikat.png",
                Secenek1Metni = "Yangın Söndürücüyü Kap",
                Secenek1HedefId = 7,

                Secenek1Ipucu = "Savunma Aracı Bul",
                OlayAksiyonu = () => { Envanter.Ekle(new Esya("Yangın Söndürücü", "Darbeye ve dumana uygun", "Silah")); }
            });

          
            Sahneler.Add(new Sahne
            {
                Id = 7,
                HikayeMetni = "Nath yangın söndürücüyle kapıyı döverken Isla havalandırma kapağını işaret etti: 'Buradan gitmeliyiz, koridor kapalı!' Nath ise kapıyı kırmaktan yana. Zaman daralıyor Max, bir karar ver: Havalandırmanın karanlık darlığı mı, yoksa kapının ardındaki mutlak kaos mu?",
                ResimAdi = "sahne6_karar.png",
                Secenek1Metni = "Havalandırmaya Gir",
                Secenek1HedefId = 8,
                Secenek1Ipucu = "Sessiz Kaçış",
                Secenek2Metni = "Kapıyı Kırıp Geç",
                Secenek2HedefId = 9,
                Secenek2Ipucu = "Doğrudan Çatışma"
            });

       
            Sahneler.Add(new Sahne
            {
                Id = 8,
                HikayeMetni = "Havalandırma borusunda ilerlerken toz ve metal kokusu ciğerlerine doluyor. Arkandaki Olivia'nın hızlı nefes alışlarını duyabiliyorsun. 'Lanet olsun,' diye fısıldıyor Nath önden. 'Bir zombi sesini çok yakından duydum.' Bu dar tünel bir kurtuluş mu yoksa bir tuzak mı?",
                ResimAdi = "sahne7_havalandirma.png",
                Secenek1Metni = "Sessizce Devam Et",
                Secenek1HedefId = 9,
                Secenek1Ipucu = "Gizlilik"
            });


            Sahneler.Add(new Sahne
            {
                Id = 9,
                HikayeMetni = "Koridora düştüğünüzde bir zombi doğrudan Nath'in üzerine atıldı! Nath yangın söndürücüyle yaratığın kafasına vururken bağırdı: 'KOŞUN! DURMAYIN!' Max, Nath'e yardım mı edeceksin yoksa kızları güvenli odaya mı taşıyacaksın?",
                ResimAdi = "sahne8_savas.png",
                Secenek1Metni = "Nath'e Yardım Et",
                Secenek1HedefId = 10,
               
                Secenek1Ipucu = "Birliktelik Gücü",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(30);
                },

                Secenek2Metni = "Güvenli Odaya Koş",
                Secenek2HedefId = 10,
                Secenek2Ipucu = "Hızlı Tahliye",
                Secenek2Aksiyonu = () =>
                {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-20);
                }
            });

          
            Sahneler.Add(new Sahne
            {
                Id = 10,
                HikayeMetni = "Kendinizi taktik odaya kilitlediniz. Isla hemen duvardaki haritayı işaret etti: 'Babamın yeri, Sığınak 4... Burası bizim tek kurtuluş şansımız. Ama metro hattını kullanmak zorundayız.' Nath haritaya bakıp başını salladı: 'Biletiniz kesildi çocuklar, cehenneme hoş geldiniz.'",
                ResimAdi = "sahne9_harita.png",
                Secenek1Metni = "Haritayı İncele",
                Secenek1HedefId = 11,
                Secenek1Ipucu = "Planı Ezberle",
                OlayAksiyonu = () => { Envanter.Ekle(new Esya("Metro Haritası", "Sığınak 4'e giden rotayı gösterir", "Görev")); }
            });

            SavasSahnesi mudurSavas = new SavasSahnesi
            {
                Id = 11,
                HikayeMetni = "Okul müdürü artık bir canavar! Nath feryat ediyor: 'Vur şuna Max!'",
                ResimAdi = "sahne11_boss_savas.png",
                DusmanIsmi = "Müdür",
                DusmanCan = 100,
                ZaferSonrasiHedefId = 12, // Zaferden sonra 12'ye git
                Secenek1Metni = "SALDIR!", 
                Secenek1Ipucu = "Kritik Vuruş"
            };
            Sahneler.Add(mudurSavas);

           
            Sahneler.Add(new Sahne
            {
                Id = 12,
                HikayeMetni = "Müdürü yendik ama okul artık bir mezar. Dışarısı yanıyor. Nath: 'Metroya gitmeliyiz, tek çıkış orası.'",
                ResimAdi = "sahne12_okul_cikis.png",
                Secenek1Metni = "Metroya İlerle",
                Secenek1HedefId = 13, 
                Secenek1Ipucu = "Yeni Rota"
            });

        
        
            Sahneler.Add(new Sahne
            {
                Id = 13,
                HikayeMetni = "Sokağın kuytu bir köşesine çöktüğümüzde adrenalin çekildi ve yerini derin bir sızıya bıraktı. Hepimiz yaralıyız... Olivia bir köşede sessizce ağlıyor, Isla ise titreyen elleriyle kolundaki çiziği sarmaya çalışıyor. İkisinin de sana ihtiyacı var ama zamanımız kısıtlı. Kimin yanına gideceksin?",
                ResimAdi = "sahne13_yaralar.png",

                // SEÇENEK 1: ISLA İLE ÖZEL AN
                Secenek1Metni = "Isla'ya Yardım Et",
                Secenek1HedefId = 131, // Isla'nın özel sahnesine gider
                Secenek1Ipucu = "Duygusal Destek (Isla +20)",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(30);
                },

                // SEÇENEK 2: OLIVIA İLE ÖZEL AN
                Secenek2Metni = "Olivia'yı Teselli Et",
                Secenek2HedefId = 132, // Olivia'nın özel sahnesine gider
                Secenek2Ipucu = "Güven Ver (Olivia +20)",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(30);
                }
            });

        
            Sahneler.Add(new Sahne
            {
                Id = 131,
                HikayeMetni = "Isla'nın yanına oturdun. O her zaman güçlü duran kızın gözlerindeki duvarların yıkıldığını gördün. 'Babamın sığınağı... Ya orası da düştüyse Max? Ya bütün bu plan boşunaysa?' Başını omzuna yasladı. Onun rasyonel zihninin de bazen bir omuza ihtiyacı var.",
                ResimAdi = "sahne13_isla_moment.png",
                Secenek1Metni = "Toparlan ve İlerle",
                Secenek1HedefId = 14 // Metro girişine bağlanır
            });

         
            Sahneler.Add(new Sahne
            {
                Id = 132,
                HikayeMetni = "Olivia'ya sarıldığında hıçkırıkları şiddetlendi. 'Kan kokusu... ellerimden hiç gitmiyor Max. Ne kadar yıkasam da çıkmayacak gibi.' Ona sıkıca sarıldın. Bu cehennemde masumiyetini korumaya çalışan tek kişi o ve senin desteğinle ayakta duruyor.",
                ResimAdi = "sahne13_olivia_moment.png",
                Secenek1Metni = "Toparlan ve İlerle",
                Secenek1HedefId = 14 // Metro girişine bağlanır
            });
        
            Sahneler.Add(new Sahne
            {
                Id = 14,
                HikayeMetni = "Metronun rutubetli ve soğuk girişine ulaştınız. Tepedeki 'STATION' tabelası can çekişen bir kalp gibi yanıp sönüyor. Dustin burnunu çekerek fısıldadı: 'İçerisi çok karanlık... Bir şeylerin bizi izlediğini hissedebiliyorum.' Nath silahını/sopasını hazırladı: 'Hazır olun, raylar üzerinde hata kabul etmezler.'",
                ResimAdi = "sahne14_metro_giris.png",
                Secenek1Metni = "Karanlığa Gir",
                Secenek1HedefId = 15
            });


            Sahneler.Add(new Sahne
            {
                Id = 15,
                HikayeMetni = "Fenerlerin cılız ışığı tünelin paslı duvarlarında dans ediyor. Arkadan gelen su damlalarının sesi, Dustin'in sendeleyen adımlarıyla birleşiyor. Isla telsizindeki cızırtıları dindirmeye çalışırken aniden durdu: 'Duyuyor musunuz? Bu bir yardım çığlığı mı yoksa bir pusu mu?'",
                ResimAdi = "sahne15_metro_yuruyus.png",
                Secenek1Metni = "Sesi Takip Et",
                Secenek1HedefId = 16,
                Secenek1Ipucu = "Risk Al"
            });


            Sahneler.Add(new Sahne
            {
                Id = 16,
                HikayeMetni = "Karanlıktan fırlayan bir gölge Dustin'in üzerine atıldı! Dişlerin kemiğe çarpma sesini tüm tünel duydu. Nath yaratığı defettiğinde artık çok geçti... Dustin bacağını tutarak yere yığıldı, pantolonundan sızan kan simsiyah görünüyordu. 'Max... yardım et... çok soğuk...' dedi titreyerek.",
                ResimAdi = "sahne16_isirik.png",
                Secenek1Metni = "Yarasını Kontrol Et",
                Secenek1HedefId = 17,
                Secenek1Ipucu = "Gerçekle Yüzleş"
            });

            Sahneler.Add(new Sahne
            {
                Id = 17,
                HikayeMetni = "Nath silahı Dustin'in başına dayadı: 'Bitir şunu Max.' Olivia'nın çığlığı tünelde yankılanıyor.",
                ResimAdi = "sahne17_infaz.png",

                // SEÇENEK 1: TETİĞİ ÇEK
                Secenek1Metni = "Tetiği Çek",
                Secenek1HedefId = 20,
                Secenek1Karakter = "", 
                Secenek1PuanEtkisi = 0,
                Secenek1Ipucu = "Huzur Ver",

                Secenek1Aksiyonu = () => {
                    MevcutOlivia = OliviaState.Saglikli; 
                    BasarimYoneticisi.BasarimKontrol("SOGUKKANLI");

             
                    BasarimYoneticisi.KararKaydet(17, "Dustin'i öldürdün",
                        "Olivia travma geçirdi ama enfeksiyondan kurtuldu", "Olivia", -40);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(50); // Nath memnun
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(-20);
                  
                    var olivia = SinifListesi.Find(x => x.Isim == "Olivia");
                    if (olivia != null)
                    {
                        olivia.PuanDegistir(-40); // Olivia travma geçirdi
                    }
                },

                // SEÇENEK 2: YAPAMAM
                Secenek2Metni = "Yapamam!",
                Secenek2HedefId = 19
            });
          
            Sahneler.Add(new Sahne
            {
                Id = 19,
                HikayeMetni = "Tereddüdün bedeli ağır oldu... Dustin Olivia'yı ısırdı! Olivia acıyla yere yığılıyor.",
                ResimAdi = "sahne19_felaket.png",

                Secenek1Metni = "Kaç!",
                Secenek1HedefId = 20,

                Secenek1Aksiyonu = () => {
                    MevcutOlivia = OliviaState.Enfekte;
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(-50);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-50);
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(+15);
                    var olivia = SinifListesi.Find(x => x.Isim == "Olivia");
                    if (olivia != null)
                    {
                        olivia.Durum = SaglikDurumu.Enfekte;
                        olivia.PuanDegistir(-30);
                    }
                }
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 20,
                HikayeMetni = "Metro çıkışında Kane ve silahlı adamlarının soğuk namluları bizi karşıladı. Kane, sigarasından derin bir nefes çekip Olivia'nın üzerindeki taze kanlara baktı: 'Sığınağıma hoş geldiniz... Ama unutmamanız gereken bir şey var: Burada kimse bedavaya ekmek yemez. Özellikle de aranızda bir saatli bomba varsa.'",
                ResimAdi = "sahne20_kane_giris.png",
                Secenek1Metni = "Müzakere Et",
                Secenek1HedefId = 21,
                Secenek1Ipucu = "Güven Kazanmaya Çalış"
            });
    
            Sahneler.Add(new Sahne
            {
                Id = 21,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Kane silahını Olivia'nın alnına doğrulttu: 'Leş kokusunu buradan alabiliyorum evlat. Bu kız ısırılmış! Kurallar basit: Enfekteler içeri giremez.' (Nath silahına davrandı)" :
                    "Kane silahını indirdi ama gözleri hala üzerinizde: 'Temiz görünüyorsunuz... Ama burası bir hayır kurumu değil. İçeri girmek istiyorsanız bedelini ödersiniz.'",
                ResimAdi = "sahne21_kane_tehdit.png", //

                Secenek1Metni = "Takas Teklif Et",
                Secenek1HedefId = 211,
                Secenek1Ipucu = "Isla bunu onaylar", 
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(40);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-25);
                },

                // SEÇENEK 2: DİK DUR
                Secenek2Metni = "Silahını İndir!",
                Secenek2HedefId = 211,
                Secenek2Ipucu = "Nath saygı duyar", // Sayı yok
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(30);
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-25);
                }
            });
      
            Sahneler.Add(new Sahne
            {
                Id = 211,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Kane silahını Olivia'nın alnına doğrulttu: 'Leş kokusunu buradan alabiliyorum evlat. Bu kız ısırılmış! Kurallar basit: Enfekteler içeri giremez.' (Nath silahına davrandı)" :
                    "Kane silahını indirdi ama gözleri hala üzerinizde: 'Temiz görünüyorsunuz... Ama burası bir hayır kurumu değil. İçeri girmek istiyorsanız bedelini ödersiniz.'",
                ResimAdi = "sahne22_kane_tehdit.png", //

                // SEÇENEK 1: MANTIKLI YAKLAŞ
                Secenek1Metni = "Takas Teklif Et",
                Secenek1HedefId = 22,
                Secenek1Ipucu = "Isla bunu onaylar", // Sayı yok, sadece his
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(40);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-25);
                },

                // SEÇENEK 2: DİK DUR
                Secenek2Metni = "Silahını İndir!",
                Secenek2HedefId = 21,
                Secenek2Ipucu = "Nath saygı duyar", // Sayı yok
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(30);
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-25);
                }
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 21,
                HikayeMetni = "Kane hafifçe gülümsedi. 'Cesaretinizi sevdim. Ama benim adamlarımın güvene ihtiyacı var.' İşaret parmağıyla tünelin karanlık tarafını gösterdi. 'İçeri girmeden önce temizlenmeniz gerek.'",
                ResimAdi = "sahne21_kane_saygi.png", //
                Secenek1Metni = "Emre Uy",
                Secenek1HedefId = 22
            });

            Sahneler.Add(new Sahne
            {
                Id = 22,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Askerler Olivia'yı kollarından tuttu! Kane: 'Bunu Karantina Hücresi 4'e atın. Dönüşürse ben hallederim.' Olivia çığlık atıyor: 'Max! Bırakma beni!'" :
                    "Kane: 'Kız revire gidecek. Kan testleri temiz çıkarsa yanınıza döner.' Olivia korkuyla kolunu tuttu. 'Max, ya geri dönemezsem?'",
                ResimAdi = "sahne23_olivia_hedef.png", //

                // SEÇENEK 1: KABULLEN
                Secenek1Metni = "Müdahale Etme",
                Secenek1HedefId = 23,
                Secenek1Ipucu = "Olivia yıkılır...", // Gizli büyük puan kaybı (-50)
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(-50);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(15);
                },

                // SEÇENEK 2: DİREN
                Secenek2Metni = "Onunla Gitmek İste",
                Secenek2HedefId = 23,
                Secenek2Ipucu = "Olivia sana bağlanır", // Gizli puan artışı (+40)
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(40);
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-20);
                }
            });

        
            SavasSahnesi kosucuBoss = new SavasSahnesi
            {
                Id = 23,
                HikayeMetni = "Kane sizi ana kapıdan almak yerine yan tünele itti. 'Burası Koşucular Tüneli. Sağ çıkarsanız sığınağı hak edersiniz.'\n\nKaranlığın içinden yüzlerce parlayan göz ve aç hırıltılar yaklaşıyor. SÜRÜ GELİYOR!",
                ResimAdi = "sahne24_kosucular.png", //
                DusmanIsmi = "Koşucu Sürüsü",
                DusmanCan = 150, // Biraz daha zorlu
                ZaferSonrasiHedefId = 25,
                Secenek1Metni = "ATEŞ SERBEST!", // Savaş butonu
                Secenek1Ipucu = "Cehennemi Yaşat"
            };
            Sahneler.Add(kosucuBoss);

            Sahneler.Add(new Sahne
            {
                Id = 25,
                HikayeMetni = "Sürüyü atlattınız ama tünelin sonundaki kapı kilitli! Kane hoparlörden seslendi: 'Silahlarınızı kapının altından atın. Yoksa kapı açılmaz.' Nath öfkeyle: 'ASLA! Silahımı verirsem ölürüm.'",
                ResimAdi = "sahne25_guven_testi.png", //

                // SEÇENEK 1: NATH'İ İKNA ET
                Secenek1Metni = "Silahı Ver Nath",
                Secenek1HedefId = 26,
                Secenek1Ipucu = "Nath'in güveni sarsılır", // -40 Puan
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-45);
                },

                // SEÇENEK 2: TEHDİT ET
                Secenek2Metni = "Kapıyı Patlatırız!",
                Secenek2HedefId = 26,
                Secenek2Ipucu = "Riskli ama Nath güvenir", // +20 Puan
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(30);
                    Envanter.Ekle(new Esya("Kırık Kilit", "Zorla girildi", "Hurda"));
                }
            });

       
            Sahneler.Add(new Sahne
            {
                Id = 26,
                HikayeMetni = "Ağır metal kapı gıcırdayarak açıldı. İçerisi... beklediğinizden çok farklı. Jeneratör sesleri, silah rafları ve maskeli adamlar. Burası bir sığınaktan çok bir askeri üs.",
                ResimAdi = "sahne26_siginak_giris.png", //
                Secenek1Metni = "İçeri Gir",
                Secenek1HedefId = 27
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 27,
                HikayeMetni = "İçeri girer girmez üzerinize basınçlı dezenfektan buharı sıkıldı! Nefes almak imkansız. Kane maskesinin ardından gülüyor: 'Mikroplarınızdan arınmanız lazım aptallar!' Nath öksürerek yere düştü.",
                ResimAdi = "sahne27_buhar_tuzagi.png", //
                Secenek1Metni = "Dayan",
                Secenek1HedefId = 28
            });

          
            Sahneler.Add(new Sahne
            {
                Id = 28,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Size bir kutu soğuk fasulye verdiler. Isla sessizce yiyor. Yanınızdaki boş yere bakıyorsun... Olivia şu an o hücrede, belki de dönüşmek üzere. Lokmalar boğazına diziliyor." :
                    "Size bir kutu soğuk fasulye verdiler. Olivia titreyerek yemeğini yemeye çalışıyor. 'Bizi ayırmadılar Max, bu iyiye işaret değil mi?' diye fısıldıyor.",
                ResimAdi = "sahne28_yemek.png", //
                Secenek1Metni = "Dinlen",
                Secenek1HedefId = 29
            });

            Sahneler.Add(new Sahne
            {
                Id = 29,
                HikayeMetni = "Kane masaya eski bir metro haritasını serdi. Parmağını kırmızıyla işaretlenmiş bir noktaya vurdu: 'Eczane. Buradaki antibiyotik ve serum stoklarını istiyorum. Eğer getirirseniz sığınakta kalıcı olursunuz. Getiremezseniz... dışarı atılırsınız.'",
                ResimAdi = "sahne29_gorev_haritasi.png", //
                Secenek1Metni = "Görevi Kabul Et",
                Secenek1HedefId = 30, // Tünel yürüyüşü başlar
                Secenek1Ipucu = "Yeni Görev: Eczane",
                OlayAksiyonu = () => { Envanter.Ekle(new Esya("Kane'in Haritası", "Eczane rotası", "Görev")); }
            });
        
            Sahneler.Add(new Sahne
            {
                Id = 30,
                HikayeMetni = "Eczaneye giden yol sandığımızdan uzun sürdü. Tünelin rutubetli havası ciğerlerimizi yakıyor. Grup yorgun düştü. Nath önden yolu kontrol ederken arkada iki kişi geride kaldı. Birinin sana ihtiyacı var Max, bu sessizliği kiminle bozacaksın?",
                ResimAdi = "sahne30_tunel_yuruyus.png", //

                // SEÇENEK 1: ISLA (Romantik/Duygusal)
                Secenek1Metni = "Isla ile Konuş",
                Secenek1HedefId = 31,
                Secenek1Ipucu = "Duygusal Bağ Kur",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(45); // Isla'nın güveni artar
                },

                // SEÇENEK 2: OLIVIA (Duruma göre değişir)
                Secenek2Metni = "Olivia'yı Kontrol Et",
                Secenek2HedefId = 35,
                Secenek2Ipucu = "Durumunu Gör",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(45); // Olivia minnet duyar
                }
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 31,
                HikayeMetni = "Isla'nın yanına oturdun. Elindeki lightstick'in mavi ışığı yüzündeki yorgunluğu aydınlatıyor. 'Herkes beni sadece diplomat sanıyor Max. Babamın kızı... Ama korkuyorum. Babamın sesi her gün zihnimde yankılanıyor.' Başını omzuna yasladı. İlk defa maskesini indirdi.",
                ResimAdi = "sahne31_romantizm_isla.png", //
                Secenek1Metni = "Yola Devam Et",
                Secenek1HedefId = 34 // Boss savaşına bağlanır
            });

            Sahneler.Add(new Sahne
            {
                Id = 35,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Olivia'nın yanına gittiğinde ter içinde titrediğini gördün. Enfeksiyon damarlarında siyah bir ağ gibi yayılıyor. Yakandan tuttu ve fısıldadı: 'Max... Dönüşmeme izin verme. Eğer o şeye dönüşürsem... beni sen öldür. Lütfen.'" :
                    "Olivia sana gülümsedi ama gözlerinde yaşlar var. Elini tuttun. 'Bizi bırakmadığın için teşekkürler Max. Herkes vazgeçerken sen yanımdaydın. Bu cehennemden çıkarsak... sana bir kahve borcum olsun.'",
                ResimAdi = MevcutOlivia == OliviaState.Enfekte ?
                    "sahne35_romantizm_olivia_enfekte.png" : //
                    "sahne35_olivia_saglikli.png",           //
                Secenek1Metni = "Yola Devam Et",
                Secenek1HedefId = 34 // Boss savaşına bağlanır
            });

            
            SavasSahnesi labirentBoss = new SavasSahnesi
            {
                Id = 34,
                HikayeMetni = "Konuşmanız, metal rafların devrilme sesiyle bölündü! Depo bölümündesiniz ve yalnız değilsiniz. Gölgelerin arasından devasa, mutasyona uğramış bir 'Labirent Bekçisi' çıkıyor! Eczaneye giden yol onun arkasında!",
                ResimAdi = "sahne34_depo_labirent.png", // Dosya ismini prompt'tan aldım
                DusmanIsmi = "Labirent Bekçisi",
                DusmanCan = 200, // Zorlu bir savaş
                ZaferSonrasiHedefId = 33, // Zaferden sonra telsiz sahnesine
                Secenek1Metni = "SAVAŞ!",
                Secenek1Ipucu = "Kritik Saldırı"
            };
            Sahneler.Add(labirentBoss);

          
            Sahneler.Add(new Sahne
            {
                Id = 33,
                HikayeMetni = "Canavarı yendikten sonra yerdeki bir askerin telsizi cızırdamaya başladı. Isla donup kaldı. Telsizdeki ses babasına ait: 'Komuta merkezi düştü... Isla... kızım... beni duyuyorsan... seni seviyorum... Sığınak 4... güvenli değil...' Ses kesildi. Isla olduğu yere çöktü.",
                ResimAdi = "sahne33_isla_telsiz.png", //

                // SEÇENEK 1: TESELLİ ET (Özel Sahneye Gider)
                Secenek1Metni = "Ona Sarıl",
                Secenek1HedefId = 331,
                Secenek1Ipucu = "Isla'yı sakinleştir",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(25); // Büyük bağ kurma
                },

                // SEÇENEK 2: ODAKLA (Doğrudan Göreve)
                Secenek2Metni = "Görevi Hatırlat",
                Secenek2HedefId = 341,
                Secenek2Ipucu = "Mantıklı ol",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(-10); // Biraz kırılır ama toparlar
                }
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 331,
                HikayeMetni = "Isla'ya sıkıca sarıldın. O güçlü, soğukkanlı kız kollarında küçük bir çocuk gibi ağlıyor. 'Yalnız kaldım Max... Babam gitti.' Saçlarını okşayarak ona güç verdin. Artık sadece hayatta kalmak için değil, birbiriniz için savaşıyorsunuz.",
                ResimAdi = "sahne331_isla_sarilma.png", //
                Secenek1Metni = "Aramaya Devam Et",
                Secenek1HedefId = 341 // Ceset arama sahnesine bağlanır
            });

           
            Sahneler.Add(new Sahne
            {
                Id = 341,
                HikayeMetni = "Duygusal anı geride bırakıp eczanenin personel girişine ulaştınız. Yerde Dr. Morales'in cesedi yatıyor. Elindeki kartta 'ARAŞTIRMA DEPARTMANI' yazıyor. Kane'in bizden istediği sadece ilaç değil... burada başka bir şeyler dönüyor.",
                ResimAdi = "sahne341_ceset_arama.png", //
                Secenek1Metni = "Cesedi Ara",
                Secenek1HedefId = 36, // Sonraki sahneye (Kartı alma / İlaçları bulma)
                OlayAksiyonu = () => { Envanter.Ekle(new Esya("Dr. Morales'in Kartı", "Gizli laboratuvar erişimi", "Anahtar")); }
            });
         
            SavasSahnesi mutantDoktor = new SavasSahnesi
            {
                Id = 36,
                HikayeMetni = "Metal gıcırtıları çığlıklara karıştı! Havalandırma kapağı büyük bir gürültüyle patladı ve içeriye Dr. Kane'in başarısız deneyi 'Mutant A-01' düştü. Çok kollu, derisi asitle erimiş gibi görünen bu ucube, eczane deposunun tek hakimi gibi hırlıyor. İlaçlara ulaşmak için önce bu kabusu aşmak zorundasınız!",
                ResimAdi = "sahne36_boss_saldiri.png",
                DusmanIsmi = "Mutant A-01",
                DusmanCan = 300,
                ZaferSonrasiHedefId = 37,

              
                Secenek1Metni = "Zayıf Noktaya Odaklan",
                Secenek1Ipucu = "Riskli ama Nath bu cesareti sever",
                Secenek1Aksiyonu = () => {
            
                  
                }
            };
            Sahneler.Add(mutantDoktor);


            Sahneler.Add(new Sahne
            {
                Id = 37,
                HikayeMetni = "...",
                ResimAdi = "sahne36_zafer.png",

                // SEÇENEK 1: YAŞAT
                Secenek1HedefId = 50,
                Secenek1Aksiyonu = () => {
                    if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(35);
                        SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-60);
                    }
                    else
                    {
                        SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(15);
                    }
                },

                // SEÇENEK 2: ACISINA SON VER
                Secenek2Metni = "",
                Secenek2HedefId = 50,
                Secenek2Aksiyonu = () => {
     
                    if (MevcutOlivia == OliviaState.Enfekte)
                    {
                        MevcutOlivia = OliviaState.Olu;
                        SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(-100);
                        SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-50);
                        SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(30);
                    }
                }
            });

            // --- ID 50: SIĞINAK GİRİŞİ VE MÜDAHALE ---
            Sahneler.Add(new Sahne
            {
                Id = 50,
                HikayeMetni = "...", // SahneGetir'de dolacak
                ResimAdi = "sahne50_saglikli.png",

                // SEÇENEK 1: İSYANKAR YOL
                Secenek1Metni = "İğne Nedir? Sorgula!",
                Secenek1HedefId = 51,
                Secenek1Ipucu = "Kane otoritesine meydan okursun",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-25);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(20); // Nath sorgulayanı sever
                },

                // SEÇENEK 2: İTAATKAR YOL
                Secenek2Metni = "Sessizce İzin Ver",
                Secenek2HedefId = 51,
                Secenek2Ipucu = "Kane'in güvenini kazanırsın",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(20);
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(-10);
                }
            });

        
            Sahneler.Add(new Sahne
            {
                Id = 51,
                HikayeMetni = "...", // SahneGetir'de dolacak
                ResimAdi = "sahne51_saglikli.png",

                // SEÇENEK 1: MANTIK YOLU
                Secenek1Metni = "Haritayı Analiz Et",
                Secenek1HedefId = 52,
                Secenek1Ipucu = "Nath ile plan yap",
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(25);
                },

                // SEÇENEK 2: DUYGU YOLU
                Secenek2Metni = "Olivia ile Konuş",
                Secenek2HedefId = 52,
                Secenek2Ipucu = "Onu sakinleştir",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Olivia")?.PuanDegistir(25);
                }
            });

        
            Sahneler.Add(new Sahne
            {
                Id = 52,
                HikayeMetni = "Bilgisayar ekranındaki soğuk mavi ışık yüzünüze vuruyor. Dosyanın adı: 'PROJE A-104'. İçeriği okudukça kanınız donuyor: 'Özne, virüsle simbiyotik bir bağ kuruyor. O bir tedavi değil, insanlığın bir sonraki evrimi veya sonu.' Raporun hemen altında Olivia'nın lise fotoğrafı ataçlanmış. Tam o sırada, arkanızdaki gölge hareket etti. Asker silahının emniyetini açtı: 'Görmemeniz gereken şeyleri gördünüz.'",
                ResimAdi = "sahne52_saglikli.png",

                // SEÇENEK 1: ZEKA (Veri Hırsızlığı)
                Secenek1Metni = "Verileri Kopyala",
                Secenek1HedefId = 53,
                Secenek1Ipucu = "Kanıtları al ve kaç",
                Secenek1Aksiyonu = () => {
                    Envanter.Ekle(new Esya("Şifreli USB", "Proje A-104 Kanıtları", "Veri"));
                },

                // SEÇENEK 2: SAVAŞ (Cesaret)
                Secenek2Metni = "Silahına Davran",
                Secenek2HedefId = 54,
                Secenek2Ipucu = "Sürpriz saldırı yap"


            });

           
            Sahneler.Add(new Sahne
            {
                Id = 53,
                HikayeMetni = "...", // SahneGetir'de dolacak (Gizli Üs Gerçeği)
                ResimAdi = "sahne53_saglikli.png",

                // TEK VE BÜYÜK SEÇENEK: İSYAN
                Secenek1Metni = "İSYAN BAŞLAT (Zombileri Uyandır)",
                Secenek1HedefId = 54, // Nath ile yüzleşmeye gider
                Secenek1Ipucu = "Kaos tek çıkış yolumuz",
                Secenek1Aksiyonu = () => {
                    SenaryoYoneticisi.LogEkle("Karantina hücreleri açıldı. Tesis kaosa sürüklendi.");
                    // Kaos efekti veya sesi eklenebilir
                },

                // İkinci seçenek yok, tek yol isyan
                Secenek2Metni = "",
                Secenek2HedefId = 0
            });

          
            Sahneler.Add(new Sahne
            {
                Id = 54,
                HikayeMetni = "...", // SahneGetir'de puana göre değişecek
                ResimAdi = "sahne54_saglikli.png",

     
                Secenek1Metni = "Devam Et",
                Secenek1HedefId = 55, // Kaçışa bağlanır

                Secenek2Metni = "",
                Secenek2HedefId = 0
            });

            // --- ID 55: BÜYÜK KAÇIŞ ---
            Sahneler.Add(new Sahne
            {
                Id = 55,
                HikayeMetni = "...", // SahneGetir'de dolacak
                ResimAdi = "sahne55_saglikli.png",

                // FİNAL
                Secenek1Metni = "Diğer Üsse Doğru Yola Çık",
                Secenek1HedefId = 56, // Final Ekranı
                Secenek1Aksiyonu = () => {
                    SenaryoYoneticisi.LogEkle("Bölüm 1 Sonu. Sığınaktan kaçıldı.");
                }
            });

            // --- ID 56: BÖLÜM SONU ---
            Sahneler.Add(new Sahne
            {
                Id = 56,
                HikayeMetni = "Arkanızda yanan bir sığınak, önünüzde bilinmez bir yol... Kane'in bahsettiği diğer askeri üssü bulmalısınız. Bu sadece bir son değil, daha büyük bir kabusun başlangıcıydı.\n\nTEBRİKLER! BÖLÜM 1 TAMAMLANDI.",
                ResimAdi = "final_orman_yolculuk.png",

                Secenek1Metni = "Diğer Üsse Doğru Yola Çık",
                Secenek1HedefId = 57,

            });
           
            Sahneler.Add(new Sahne
            {
                Id = 57,
                ResimAdi = "final_orman_yolculuk.png",
                Secenek1Metni = "İlerlemeye Devam Et",
                Secenek1HedefId = 58,
                DinamikMetinHazirla = () =>
                {
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    bool nathSatti = (nath != null && nath.MoralPuani < 70);

                    if (MevcutOlivia == OliviaState.Enfekte && !nathSatti)
                        return "Harita elimde sırılsıklam... Dalları iterek ilerliyoruz. Nathan hemen arkamda, ormanın sessizliğini bozan tek şey onun güven veren adımları. Ama Olivia... Her durduğumuzda o hırıltılı nefesi duyuyorum. Nathan ona bakarken gözlerindeki o çaresizliği görüyorum; 'Az kaldı Max' diyor. Sanki arkamızdan bir şey bizi izliyormuş gibi hissediyorum ama tek odak noktam karşıdaki ışıklar.";

                    if (MevcutOlivia == OliviaState.Enfekte && nathSatti)
                        return "Nathan bizi sattı... Haritaya her baktığımda o kahredici ihanetini hissediyorum. Şimdi bu karanlık ormanda sadece Isla ve her an bilincini yitirebilecek bir Olivia ile baş başayım. Nathan muhtemelen çoktan üsse varmıştır diye düşünüyorum ama ensemde soğuk bir nefes var. İhanetin ağırlığı, Olivia'nın hırıltılarıyla birleşiyor. Beta Üssü'ne ulaşmak tek şansımız.";

                    if (MevcutOlivia == OliviaState.Olu && !nathSatti)
                        return "Elimdeki harita artık anlamsız. Olivia yok... Nathan ile saatlerdir tek kelime etmedik. Nathan sustukça orman daha da sessizleşiyor. Sanki bastığımız her dal, Olivia'nın yokluğunu yüzümüze çarpıyor. Nathan'ın çökmüş omuzlarında Olivia'yı koruyamamış olmanın yükü var. Arkamızdaki karanlık sanki bizi yutmak istiyor ama biz sadece önümüzdeki ışıklara, Beta Üssü'ne odaklandık.";

                    if (MevcutOlivia == OliviaState.Olu && nathSatti)
                        return "Yapayalnızım... Harita elimde titriyor. Nathan bizi terk edip Kane'e sığındı, Olivia ise artık toprak altında. Isla ile bu karanlık ağaçların arasından geçerken Nathan'ın her an bir yerlerden çıkıp bizi avlamasından korkuyorum. Arkamızdaki sessizlik tekinsiz. Beta Üssü'nün kuleleri uzaktan görünüyor ama oraya vardığımızda her şeyin biteceğine inanmak çok zor.";

                    if (MevcutOlivia == OliviaState.Saglikli && nathSatti)
                        return "Nathan bizi Kane'e sattı... Onca yaşanandan sonra bizi arkadan vurdu. Ama Olivia hala yanımda, hala sağlıklı. Haritaya bakıp 'Az kaldı Max' dediğinde içimdeki ateş tekrar harlanıyor. Nathan'ın yokluğu ormanı daha korkunç kılıyor ama Olivia'nın elini tutmak bana güç veriyor. Hızla üsse yaklaşırken arkamızda bıraktığımız gölgelerin bize yaklaştığından habersiziz.";

                    return "Her şey planladığımız gibi... Nathan önden yolu açıyor, Olivia ise elimi tutuyor. Bu harita bizi kurtuluşa götürecek. Nathan'ın sadakati ve Olivia'nın sağlığı, bu dünyada sahip olduğum tek gerçek. Az kaldı... Üs kapılarına vardığımızda her şey düzelecek diye umuyorum ama içimde tarif edemediğim bir huzursuzluk, arkamızdaki ormandan gelen tuhaf bir his var.";
                }
            });

            // --- SAHNE 58: ÜS KAPISI (SAVAŞ ALANI) ---
            Sahneler.Add(new Sahne
            {
                Id = 58,
                ResimAdi = "final_orman_yaklasim.png",
                Secenek1Metni = "Kapılara Ulaş ve Arkana Bak",
                Secenek1HedefId = 64, // Kane'in arkadan gelişi burada başlayacak
                DinamikMetinHazirla = () =>
                {
                    return "Ormanın bittiği yerdeyiz. Devasa çelik duvarlar önümüzde yükseliyor. Tam kurtulduk derken, Isla aniden durup arkasını işaret etti: 'Max... Bir şey geliyor!' Ormanın içinden gelen ağır ayak sesleri ve hırıltılar... Güvende olduğumuzu sandığımız kapının önünde, asıl cehennemin arkamızdan geldiğini anlıyoruz. Kaçacak yerimiz yok. Kane ve (eğer sattıysa) Nathan tam arkamızda!";
                }
            });
            // --- SAHNE 64: KANE İLE KARŞILAŞMA (BÖLÜM 1 - DENEYLER) ---
            Sahneler.Add(new Sahne
            {
                Id = 64,
                ResimAdi = "final_kane_monolog.png",
                Secenek1Metni = "Devamını Dinle...",
                Secenek1HedefId = 640, // Part 2'ye gider
                DinamikMetinHazirla = () =>
                {
                    return "Ormanın derinliklerinde, ağaçların arasında bir silüet belirdi. Dr. Kane... Ama tanıdığın o soğukkanlı adamdan eser yok. Üniforması parçalanmış, boynundaki damarlar kapkara kesilmiş. Enfeksiyon onu ele geçiriyor. Elinde tek bir mavi tüpü sıkıca tutuyor.\n\n" +
                           "'Bak bana Max!' diye hırladı. 'Bu virüs bir hata değildi. İnsanlık zayıftı, çürüktü. Biz sadece evrimi hızlandırdık. Acı yoksa, gelişim de yoktur. Ben dünyayı kurtarmaya çalıştım!'";
                }
            });

            Sahneler.Add(new Sahne
            {
                Id = 640,
                ResimAdi = "final_kane_monolog.png",
                Secenek1Metni = "Sonraki",
                Secenek1HedefId = 65, // Karar anına gider
                DinamikMetinHazirla = () =>
                {
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    bool nathSatti = nath != null && nath.MoralPuani < 70;

               

                    string metin = "Kane öksürerek yere tükürdü, kan kusuyor. Gözlerini sana dikti:\n\n";

                    if (nathSatti)
                    {
                       
                        metin += "'Ve o dostun Nathan... Sığınakta beni satıp kaçabileceğini sandı. Onu bizzat kendi ellerimle boğdum! Cesedi şu an alevlerin arasında.'\n\n";
                    }
                    else
                    {
                       
                        metin += "'Ve dostun Nathan... Görüyorum ki o cehennemden çıkmayı başarmış. Ne dokunaklı bir sadakat.'\n\n";
                        metin += "Kane, Nathan'a nefretle baktı: 'Seni o sığınakta gebertmeliydim!'";
                    }

                    metin += "\n\n'Hepiniz suçlusunuz! Arkadaşlarımı, projemi, geleceğimi yok ettiniz. Şimdi bu orman sizin mezarınız olacak!'";
                    return metin;
                }
            });

            
            Sahneler.Add(new Sahne
            {
                Id = 65,
                ResimAdi = "final_kane_silah.png",
                HikayeMetni = "Kane titreyen eliyle silahını sana doğrulttu. Gözü dönmüş durumda. Bir saniye içinde tetiği çekecek. Ne yapacaksın?",
                Sure = 0, // ÖNEMLİ: Timer'ı kapat, bu sahne karardır

                Secenek1Metni = "KENDİNİ FEDA ET",
                Secenek1HedefId = 66,

                Secenek2Metni = "BEKLE (Hareketsiz Kal)",
                Secenek2HedefId = 0,
                Secenek2Aksiyonu = () =>
                {
                    var kane = SinifListesi.Find(x => x.Isim == "Dr. Kane");
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    var isla = SinifListesi.Find(x => x.Isim == "Isla");
                    var olivia = SinifListesi.Find(x => x.Isim == "Olivia");

                    bool nathSatti = nath != null && nath.MoralPuani < 70;
                    int kanePuan = kane != null ? kane.MoralPuani : 0;
                    int gidilecekId = 0;

                    if (kanePuan >= 40)
                    {
                        gidilecekId = 67;
                    }
                    else
                    {
                        if (MevcutOlivia == OliviaState.Enfekte)
                        {
                            if (olivia != null && olivia.MoralPuani >= 50) gidilecekId = 71;
                            else gidilecekId = 72;
                        }
                        else if (MevcutOlivia == OliviaState.Olu && nathSatti)
                        {
                            if (isla != null && isla.MoralPuani < 60) gidilecekId = 69;
                            else gidilecekId = 70;
                        }
                        else if (!nathSatti)
                        {
                            gidilecekId = 68;
                        }
                        else
                        {
                            int islaP = isla?.MoralPuani ?? 0;
                            int oliP = olivia?.MoralPuani ?? 0;
                            gidilecekId = (islaP > oliP) ? 70 : 73;
                        }
                    }

         
                    var form = System.Windows.Forms.Application.OpenForms.OfType<Form1>().FirstOrDefault();
                    if (form != null && gidilecekId > 0)
                    {
               
                        form.Invoke(new System.Action(() => {
                            form.SahneyiEkranaBas(gidilecekId);
                        }));
                    }
                }
            });

          
        

            Sahneler.Add(new Sahne
            {
                Id = 21,
                HikayeMetni = MevcutOlivia == OliviaState.Enfekte ?
                    "Kane silahını Olivia'nın alnına doğrulttu: 'Leş kokusunu buradan alabiliyorum evlat. Bu kız ısırılmış! Kurallar basit: Enfekteler içeri giremez.' (Nath silahına davrandı)" :
                    "Kane silahını indirdi ama gözleri hala üzerinizde: 'Temiz görünüyorsunuz... Ama burası bir hayır kurumu değil. İçeri girmek istiyorsanız bedelini ödersiniz.'",
                ResimAdi = "sahne21_kane_tehdit.png",

                Secenek1Metni = "Takas Teklif Et",
                Secenek1HedefId = 211,
                Secenek1Ipucu = "Isla bunu onaylar",
                Secenek1Karakter = "", 
                Secenek1PuanEtkisi = 0, 
                Secenek1Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Isla")?.PuanDegistir(40);
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(-25);
                },

                Secenek2Metni = "Silahını İndir!",
                Secenek2HedefId = 21,
                Secenek2Ipucu = "Nath saygı duyar",
                Secenek2Aksiyonu = () => {
                    SinifListesi.Find(x => x.Isim == "Nath")?.PuanDegistir(30);
                    SinifListesi.Find(x => x.Isim == "Dr. Kane")?.PuanDegistir(-25);
                }
            });

         
            Sahneler.Add(new Sahne
            {
                Id = 66,
                ResimAdi = "final_feda_max.png",
                HikayeMetni = "Kimsenin ölmesine izin veremezdin. Kane tetiği çektiği an arkadaşlarının önüne atıldın. Göğsünde keskin bir yanma hissettin. Yere düşerken Kane'in diğerleri tarafından etkisiz hale getirildiğini gördün. Gözlerin kararırken duyduğun son şey arkadaşlarının haykırışlarıydı. Sen bir kahraman olarak öldün.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100
            });

          
            Sahneler.Add(new Sahne
            {
                Id = 67,
                ResimAdi = "final_kane_baris.png",
                HikayeMetni = "Kane silahı indirdi. Gözlerindeki delilik bir anlığına söndü. 'Ben ne yaptım...' diye fısıldadı. Cebindeki ilacı çıkarıp sana fırlattı. 'Al bunu. Belki hala bir umut vardır. Git buradan, beni kendi cehennemimde yalnız bırak.' Kane ormanda kalırken, elinde insanlığın son umuduyla yola devam ettin. Herkes kurtuldu.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100
            });

            Sahneler.Add(new Sahne
            {
                Id = 68,
                ResimAdi = "final_feda_nath.png",
                HikayeMetni = "Kane tetiği çekti! Ama kurşun sana isabet etmedi. Nathan devasa cüssesiyle önüne atladı. Üç kurşun... Nathan yere yığılırken son gücüyle Kane'e ateş edip onu vurdu. Nathan kanlar içinde sana gülümsedi: 'Seni koruyacağımı söylemiştim şef...' En sadık dostunu kaybettin ama yaşadın.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100,
                OlayAksiyonu = () => { NathanOlduMu = true; } 
            });

        
            Sahneler.Add(new Sahne
            {
                Id = 69,
                ResimAdi = "final_isla_ihanet.png",
                HikayeMetni = "Isla seni Kane'in mermilerinin önüne itti! 'Üzgünüm Max!' O kaçarken sen ihanetle öldün.",
                Secenek1Metni = "OYUN BİTTİ",
                Secenek1Aksiyonu = () => System.Windows.Forms.Application.Restart(),
              
            });
   
            Sahneler.Add(new Sahne
            {
                Id = 70,
                ResimAdi = "final_isla_feda.png",
                HikayeMetni = "\"Kane tetiği çektiği an Isla çığlık atarak önüne atladı! 'Yaşa Max!' Mermiler o narin bedenine saplandı. Isla kollarında can verirken Kane'in şaşkınlığından faydalanıp onu vurdun. Isla senin için kendini feda etti.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100,
                OlayAksiyonu = () => { IslaOlduMu = true; } 
            });
            
            Sahneler.Add(new Sahne
            {
                Id = 71,
                ResimAdi = "final_olivia_canavar.png",
                HikayeMetni = "Kane silahını ateşlemeden Olivia korkunç bir çığlık attı! 'Ona dokunma!' İçindeki virüsü serbest bıraktı. Kemikleri çatırdadı, derisi siyahlaştı ve devasa bir canavara dönüştü. Kane ne olduğunu anlayamadan Olivia onu parçaladı. Sizi kurtardı ama o artık geri dönüşü olmayan bir canavar.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100
            });

            
            Sahneler.Add(new Sahne
            {
                Id = 72,
                ResimAdi = "final_kotu_son.png",
                HikayeMetni = "Bekledin... Belki biri bir şey yapar diye. Ama Olivia virüse yenik düşmüştü, diğerleri ise donup kalmıştı. Kane kahkaha atarak tetiği çekti. Önce seni, sonra diğerlerini vurdu. Cesaret edememenin bedelini hepiniz canınızla ödediniz.",
                Secenek1Metni = "OYUN BİTTİ",
                Secenek1HedefId = 100
            });

            
            Sahneler.Add(new Sahne
            {
                Id = 73,
                ResimAdi = "final_isla_feda.png",
                HikayeMetni = "Olivia senin önüne atladı. 'Bizi bırakmadığın için sağ ol Max...' Kane'i vurdun ama Olivia'yı kaybettin.",
                Secenek1Metni = "ÜSSE GİR",
                Secenek1HedefId = 100,
                OlayAksiyonu = () => { MevcutOlivia = OliviaState.Olu; } 
            });

            Sahneler.Add(new Sahne
            {
                Id = 100,
                ResimAdi = "final_tedavi_laboratuvar.png",
                Secenek1Metni = "Raporu İncele >>",
                Secenek1HedefId = 0,

             
                DinamikMetinHazirla = () =>
                {
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    var isla = SinifListesi.Find(x => x.Isim == "Isla");
                    var kane = SinifListesi.Find(x => x.Isim == "Dr. Kane");

                    bool barisOldu = kane != null && kane.MoralPuani >= 40;
                    bool oliviaYasiyor = MevcutOlivia == OliviaState.Saglikli;
                    bool nathYasiyor = !NathanOlduMu && (barisOldu || (nath != null && nath.MoralPuani >= 70));
                    bool islaYasiyor = !IslaOlduMu && isla != null && isla.MoralPuani >= 40;
                    bool yapayalniz = !oliviaYasiyor && !nathYasiyor && !islaYasiyor;

                    string metin = "";

                    if (yapayalniz)
                    {
                        metin = "Beta Üssü'nün kapıları arkamdan kapandığında derin bir sessizlik oldu. Yanımda kimse yoktu. Ne Nathan'ın esprileri, ne Olivia'nın sıcaklığı, ne de Isla'nın sesi...\n\n";
                        metin += "Bilim insanları getirdiğim örnekten aşıyı üretti. 'İnsanlığı kurtardın' diyorlar. Ama ben aynaya baktığımda sadece hayatta kalan bir korkak görüyorum. Bu bir zafer değil, bir cenaze. Tedaviyi buldum ama paylaşacak kimsem kalmadı.";
                    }
                    else
                    {
                        metin = "Beta Üssü'ne ulaştığımızda omuzlarımızdaki yük kalktı. Kane'den (veya laboratuvardan) aldığımız veriler sayesinde aşı üretildi.\n\n";

                        if (oliviaYasiyor && nathYasiyor && islaYasiyor)
                            metin += "Ben, Nathan, Olivia ve Isla... O cehennemden sağ çıktık. Dostlarım yanımda olduğu sürece yeni dünyayı inşa edebiliriz. Kaybettiğimiz her şey için, bu aşı yeni bir umut olacak.";
                        else if (oliviaYasiyor && nathYasiyor)
                            metin += "Ben, Nathan ve Olivia... O cehennemden sağ çıktık. Dostlarım yanımda olduğu sürece yeni dünyayı inşa edebiliriz. Kaybettiğimiz her şey için, bu aşı yeni bir umut olacak.";
                        else if (oliviaYasiyor)
                            metin += "Olivia elimi sımsıkı tutuyor. 'Bitti Max,' diyor. Diğerlerini kaybettik ama en azından birbirimize sahibiz. Onun iyileştiğini görmek her şeye değdi.";
                        else if (nathYasiyor)
                            metin += "Nathan omzuma vurdu: 'Başardık evlat.' Olivia'yı kurtaramadık ama onun anısı bu aşıyla yaşayacak. En azından arkamı kollayacak dostum hala yanımda.";
                        else if (islaYasiyor)
                            metin += "Çok kayıp verdik ama en azından Isla hayatta. Birlikte yeni bir başlangıç yapacağız. Babasının mirasını yaşatacak.";
                        else
                            metin += "Dostlarımın fedakarlığı sayesinde buradayım. Onların cesareti olmasaydı bu aşı asla üretilemezdi. İsimlerini asla unutturmayacağım.";
                    }

                    return metin;
                },

                Secenek1Aksiyonu = () =>
                {
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    var isla = SinifListesi.Find(x => x.Isim == "Isla");
                    var kane = SinifListesi.Find(x => x.Isim == "Dr. Kane");

                    bool oliviaHayatta = MevcutOlivia == OliviaState.Saglikli;
                    bool barisOldu = kane != null && kane.MoralPuani >= 40;
                    bool nathHayatta = !NathanOlduMu && (nath != null && nath.MoralPuani >= 70) && (barisOldu || AktifSahne.Id != 72);
                    bool islaHayatta = !IslaOlduMu && (isla != null && isla.MoralPuani >= 40);
                    bool yapayalniz = !oliviaHayatta && !nathHayatta && !islaHayatta;

                    string baslik = yapayalniz ? "⚠️ KAYIP VE ZAYİAT RAPORU" : "✅ GÖREV BAŞARI RAPORU";
                    string rapor = "";

                    rapor += ">> OLIVIA:\n";
                    if (oliviaHayatta) rapor += "   DURUM: HAYATTA (Sağlıklı) 🌟\n";
                    else if (MevcutOlivia == OliviaState.Enfekte) rapor += "   DURUM: KAYIP (Dönüştü) ☣️\n";
                    else rapor += "   DURUM: ÖLÜ (Salgına Yenildi) 💀\n";

                    rapor += "\n>> NATHAN:\n";
                    if (NathanOlduMu) rapor += "   DURUM: ŞEHİT (Feda Etti) ✝️\n";
                    else if (nathHayatta) rapor += "   DURUM: HAYATTA (Kahraman) 🛡️\n";
                    else rapor += "   DURUM: ÖLÜ (Hain Olarak Vuruldu) ❌\n";

                    rapor += "\n>> ISLA:\n";
                    if (IslaOlduMu) rapor += "   DURUM: ŞEHİT (Feda Etti) 💔\n";
                    else if (islaHayatta) rapor += "   DURUM: HAYATTA (Üsse Ulaştı) ✅\n";
                    else rapor += "   DURUM: KAYIP (Firar Etti) 🏃‍♀️\n";

                    rapor += "\n>> DR. KANE:\n";
                    if (barisOldu) rapor += "   DURUM: PİŞMAN (İşbirliği Yaptı) 🕊️\n";
                    else rapor += "   DURUM: ETKİSİZ HALE GETİRİLDİ 🔫\n";

                    rapor += "\n----------------------------------\nSONUÇ: ";
                    if (yapayalniz) rapor += "YALNIZ KURTULAN.\n(Tedavi bulundu ama paylaşacak kimse kalmadı.)";
                    else if (oliviaHayatta && nathHayatta) rapor += "KUSURSUZ ZAFER!\n(Çekirdek kadro hayatta.)";
                    else rapor += "BURUK ZAFER.\n(Bazı dostlar feda edildi.)";

                    if (oliviaHayatta && nathHayatta && islaHayatta)
                    {
                        BasarimYoneticisi.BasarimKontrol("KAHRAMAN");
                    }

                    // 2. Yapayalnız
                    if (!oliviaHayatta && !nathHayatta && !islaHayatta)
                    {
                        BasarimYoneticisi.BasarimKontrol("HAYATTA_KALAN");
                    }

                    // 3. Romantik
                 

                    // 4. Diplomat
                    if (kane != null && kane.MoralPuani >= 40)
                    {
                        BasarimYoneticisi.BasarimKontrol("DIPLOMAT");
                    }

                    // 5. Sadakat
                    if (nath != null && nath.MoralPuani >= 70)
                    {
                        BasarimYoneticisi.BasarimKontrol("SADAKAT");
                    }

                    // 6. İhanet
                    if (nath != null && nath.MoralPuani < 70)
                    {
                        BasarimYoneticisi.BasarimKontrol("IHANET");
                    }

                    // 7. Koleksiyoncu
                    if (Envanter.Esyalar.Count >= 5)
                    {
                        BasarimYoneticisi.BasarimKontrol("KOLEKSIYONCU");
                    }

                    // Overlay'i göster
                    var form = System.Windows.Forms.Application.OpenForms.OfType<Form1>().FirstOrDefault();
                    if (form != null)
                    {
                        form.OverlayGoster(baslik, rapor);
                    }
                }
            });


            Sahneler.Add(new Sahne
            {
                Id = 101,
                ResimAdi = "final_tedavi_laboratuvar.png",
                HikayeMetni = "Sistem verileri işleniyor... Rapor ekrana yansıtıldı.",
                Secenek1Metni = "ANA MENÜYE DÖN",
                Secenek1Aksiyonu = () =>
                {
                    var aktifOyun = System.Windows.Forms.Application.OpenForms.OfType<Form1>().FirstOrDefault();
                    if (aktifOyun != null) aktifOyun.Close();
                },
                OlayAksiyonu = () =>
                {
                    var nath = SinifListesi.Find(x => x.Isim == "Nath");
                    var isla = SinifListesi.Find(x => x.Isim == "Isla");
                    var kane = SinifListesi.Find(x => x.Isim == "Dr. Kane");

                    bool oliviaHayatta = MevcutOlivia == OliviaState.Saglikli;
                    bool barisOldu = kane != null && kane.MoralPuani >= 40;
      
                    bool nathHayatta = !NathanOlduMu &&
                                       (nath != null && nath.MoralPuani >= 70) && // Satmamış olması lazım
                                       (barisOldu || AktifSahne.Id != 72); // Kötü sonda herkes ölür
                    bool islaHayatta = !IslaOlduMu && (isla != null && isla.MoralPuani >= 40);
                    bool yapayalniz = !oliviaHayatta && !nathHayatta && !islaHayatta;

                    string baslik = yapayalniz ? "⚠️ KAYIP VE ZAYİAT RAPORU" : "✅ GÖREV BAŞARI RAPORU";
                    string rapor = "";

                    rapor += ">> OLIVIA:\n";
                    if (oliviaHayatta) rapor += "   DURUM: HAYATTA (Sağlıklı) 🌟\n";
                    else if (MevcutOlivia == OliviaState.Enfekte) rapor += "   DURUM: KAYIP (Dönüştü) ☣️\n";
                    else rapor += "   DURUM: ÖLÜ (Salgına Yenildi) 💀\n";

                    rapor += "\n>> NATHAN:\n";
                    if (NathanOlduMu) rapor += "   DURUM: ŞEHİT (Feda Etti) ✝️\n";
                    else if (nathHayatta) rapor += "   DURUM: HAYATTA (Kahraman) 🛡️\n";
                    else rapor += "   DURUM: ÖLÜ (Hain Olarak Vuruldu) ❌\n";

                    rapor += "\n>> ISLA:\n";
                    if (IslaOlduMu) rapor += "   DURUM: ŞEHİT (Feda Etti) 💔\n";
                    else if (islaHayatta) rapor += "   DURUM: HAYATTA (Üsse Ulaştı) ✅\n";
                    else rapor += "   DURUM: KAYIP (Firar Etti) 🏃‍♀️\n";

                    rapor += "\n>> DR. KANE:\n";
                    if (barisOldu) rapor += "   DURUM: PİŞMAN (İşbirliği Yaptı) 🕊️\n";
                    else rapor += "   DURUM: ETKİSİZ HALE GETİRİLDİ 🔫\n";

                    rapor += "\n----------------------------------\nSONUÇ: ";
                    if (yapayalniz) rapor += "YALNIZ KURTULAN.\n(Tedavi bulundu ama paylaşacak kimse kalmadı.)";
                    else if (oliviaHayatta && nathHayatta) rapor += "KUSURSUZ ZAFER!\n(Çekirdek kadro hayatta.)";
                    else rapor += "BURUK ZAFER.\n(Bazı dostlar feda edildi.)";

                    var form = System.Windows.Forms.Application.OpenForms[0] as Form1;
                    if (form != null) form.OverlayGoster(baslik, rapor);
                }
            });
        }
    }
}

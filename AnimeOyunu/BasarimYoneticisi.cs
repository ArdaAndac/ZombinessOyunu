using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AnimeOyunu
{
    public class Basarim 
    {
        public string Id { get; set; }
        public string Isim { get; set; }
        public string Aciklama { get; set; }
        public string Emoji { get; set; }
        public bool Acildi { get; set; } = false;
    }

    public class KararKaydi
    {
        public int SahneId { get; set; }
        public string Karar { get; set; }
        public string Sonuc { get; set; }
        public string EtkilenenKarakter { get; set; }
        public int PuanDegisimi { get; set; }
    }

    public static class BasarimYoneticisi //neden static
    {
        public static List<Basarim> TumBasarimlar = new List<Basarim>();
        public static List<KararKaydi> KararGecmisi = new List<KararKaydi>();

        public static void BasarimlariYukle()
        {
            if (TumBasarimlar.Count > 0) return;

            TumBasarimlar.Add(new Basarim
            {
                Id = "KAHRAMAN",
                Isim = "Kahraman",
                Aciklama = "Olivia, Nathan ve Isla'yı hayatta tut",
                Emoji = "🏆"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "SOGUKKANLI",
                Isim = "Soğukkanlı",
                Aciklama = "Dustin'i kendi ellerinle öldür",
                Emoji = "🧊"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "ROMANTIK",
                Isim = "Aşık",
                Aciklama = "Olivia ile ilişkini 80'in üzerine çıkar",
                Emoji = "💕"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "DIPLOMAT",
                Isim = "Diplomat",
                Aciklama = "Kane ile barış yap",
                Emoji = "🕊️"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "HAYATTA_KALAN",
                Isim = "Yalnız Kurt",
                Aciklama = "Herkes ölürken sen hayatta kal",
                Emoji = "💀"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "SADAKAT",
                Isim = "Sadık Dost",
                Aciklama = "Nathan'ın sana olan güvenini koru (70+)",
                Emoji = "🛡️"
            });

            TumBasarimlar.Add(new Basarim
            {
                Id = "IHANET",
                Isim = "İhanet",
                Aciklama = "Bir arkadaşının seni satmasına sebep ol",
                Emoji = "🗡️"
            });
        }

        public static void BasarimKontrol(string basarimId)
        {
            var basarim = TumBasarimlar.Find(x => x.Id == basarimId);

            if (basarim != null && !basarim.Acildi)
            {
                basarim.Acildi = true;
                SenaryoYoneticisi.LogEkle($"🏆 BAŞARIM AÇILDI: {basarim.Isim}");
            }
        }

        public static void KararKaydet(int sahneId, string karar, string sonuc, string karakter = "", int puan = 0) 
        {
            KararGecmisi.Add(new KararKaydi
            {
                SahneId = sahneId,
                Karar = karar,
                Sonuc = sonuc,
                EtkilenenKarakter = karakter,
                PuanDegisimi = puan
            });
        }

        public static string BasarimlariListele()
        {
            BasarimlariYukle();

            int acilan = TumBasarimlar.Count(x => x.Acildi); // sayac 
            int toplam = TumBasarimlar.Count;

            string liste = $"🏆 BAŞARIMLAR ({acilan}/{toplam})\n\n";

            foreach (var basarim in TumBasarimlar)
            {
                if (basarim.Acildi)
                {
                    liste += $"✅ {basarim.Emoji} {basarim.Isim}\n";
                    liste += $"   {basarim.Aciklama}\n\n";
                }
                else
                {
                    liste += $"🔒 ??? Gizli Başarım\n";
                    liste += $"   {basarim.Aciklama}\n\n";
                }
            }

            return liste;
        }

        public static string KelebekHaritasiniGetir()
        {
            if (KararGecmisi.Count == 0) 
                return "Henüz hiçbir kritik karar vermedin.";

            string harita = "🦋 KARARLARININ ETKİLERİ\n\n";

            foreach (var kayit in KararGecmisi) 
            {
                harita += $"📍 SAHNE {kayit.SahneId}\n"; 
                harita += $"   Karar: {kayit.Karar}\n";
                harita += $"   Sonuç: {kayit.Sonuc}\n";

                if (!string.IsNullOrEmpty(kayit.EtkilenenKarakter))
                {
                    string isaret = kayit.PuanDegisimi > 0 ? "+" : "";
                    harita += $"   Etki: {kayit.EtkilenenKarakter} {isaret}{kayit.PuanDegisimi}\n";
                }

                harita += "\n";
            }

            return harita;
        }
    }
}
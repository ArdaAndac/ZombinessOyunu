using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeOyunu
{
    public interface IEsya { string Ad { get; set; } string Tip { get; set; } string Aciklama { get; set; } } //interface inerface kullanma sebebim farklı sınıfları birleştirmek

    public class Esya : IEsya
    {
        public string Ad { get; set; }
        public string Tip { get; set; }
        public string Aciklama { get; set; }
        public Esya(string ad, string aciklama, string tip = "Genel") { Ad = ad; Aciklama = aciklama; Tip = tip; }
    }

    public static class Envanter
    {
        public static List<IEsya> Esyalar = new List<IEsya>();
        // EsyaSistemi.cs -> Envanter sınıfı içindeki Ekle metodunu değiştir:

        public static void Ekle(IEsya yeniEsya)
        {
            if (yeniEsya != null)
            {
                Esyalar.Add(yeniEsya);

                // --- LOG EKLENTİSİ ---
                SenaryoYoneticisi.LogEkle($"ENVANTER: {yeniEsya.Ad} çantaya eklendi.");
            }
        }
        public static string Listele() // listedeki her bir eşyayı al onu süslü yazı formuna çevir
        {
            if (Esyalar.Count == 0) return "Çantanız şu an boş.";
            return "--- ÇANTA ANALİZİ ---\n\n" + string.Join("\n\n", Esyalar.Select(x => $"📦 {x.Ad.ToUpper()}\n   Not: {x.Aciklama}"));
        }
    }
}
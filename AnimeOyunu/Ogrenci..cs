using System;

namespace AnimeOyunu
{
    public enum SaglikDurumu { Saglikli, Yarali, Enfekte, Olu }

    public class Ogrenci 
    {
        public string Isim { get; set; }
        public string Rol { get; set; } 
        public int MoralPuani { get; set; } = 50; 
        public SaglikDurumu Durum { get; set; } = SaglikDurumu.Saglikli;

        public Ogrenci(string isim, string rol) 
        {
            Isim = isim;
            Rol = rol;
        }

        // --- OPERATOR OVERLOADING (OPERATÖR AŞIRI YÜKLEME) ---
        public static Ogrenci operator +(Ogrenci o, int puan)
        {
            o.MoralPuani += puan;
            if (o.MoralPuani > 100) o.MoralPuani = 100;
            return o;
        }

        public static Ogrenci operator -(Ogrenci o, int puan)
        {
            o.MoralPuani -= puan;
            if (o.MoralPuani < 0) o.MoralPuani = 0;
            return o;
        }

       
        public string IliskiDurumu 
        {
            get
            {
                if (MoralPuani >= 80) return "Sana Hayran";
                if (MoralPuani >= 50) return "Güveniyor";
                if (MoralPuani >= 20) return "Şüpheci";
                return "Senden Nefret Ediyor";
            }
        }

        

        public void PuanDegistir(int miktar)
        {
            int eskiPuan = MoralPuani;

            
            if (miktar > 0)
            {
                var dummy = this + miktar;
            }
            else
            {
                var dummy = this - Math.Abs(miktar);
            }

            int fark = MoralPuani - eskiPuan;

           
            if (fark != 0)
            {
                // 1. Loga yaz (Mevcut sistemin)
                string sembol = fark > 0 ? "+" : "";
                string ifade = fark > 0 ? "arttı" : "azaldı";
                SenaryoYoneticisi.LogEkle($"{Isim} ile ilişkin {sembol}{fark} {ifade}. (Güncel: %{MoralPuani})"); // oyunun beynine sinyal gönderen kod 

             
            
                

                SenaryoYoneticisi.SinyalGonder(Isim, fark);
            }
        }

        public override string ToString()
        {
            string durumSembol = "";
            if (Durum == SaglikDurumu.Saglikli) durumSembol = "💚";
            else if (Durum == SaglikDurumu.Yarali) durumSembol = "❤️‍🩹";
            else if (Durum == SaglikDurumu.Enfekte) durumSembol = "🧟";
            else durumSembol = "💀";

            return $"{durumSembol} {Isim} [{Rol}] - Moral: {MoralPuani} ({IliskiDurumu})";
        }
    }
}
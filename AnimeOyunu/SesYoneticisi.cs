using System.Media;
using System.IO;
using System.Windows.Forms;

namespace AnimeOyunu
{
    public static class SesYoneticisi
    {
        private static SoundPlayer player;

 
        public static void SesCal(string sesDosyasi, bool dongu = false)
        {
            try
            {
                string yol = Path.Combine(Application.StartupPath, "Sesler", sesDosyasi);

                if (File.Exists(yol))
                {
                    if (player != null) player.Stop();

                    player = new SoundPlayer(yol);

                    if (dongu)
                        player.PlayLooping(); // 
                    else
                        player.Play();
                }
            }
            catch { /* Ses yoksa oyun çökmesin */ }
        }

        public static bool SesAcik = true;

        public static void SesiKapatAc()
        {
            SesAcik = !SesAcik;
            if (!SesAcik)
            {
                Durdur(); //
            }
        }


        public static void Durdur()
        {
            if (player != null) player.Stop();
        }
    }
}
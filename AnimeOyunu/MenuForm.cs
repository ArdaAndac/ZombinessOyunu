using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimeOyunu
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();

            this.Text = "Echoes of the Bunker";
            this.BackColor = Color.Black;
            this.WindowState = FormWindowState.Maximized; 
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Load += (s, e) => ArayuzuDuzenle();
            this.Resize += (s, e) => ArayuzuDuzenle();
        }

        private void ArayuzuDuzenle()
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

          

            int butonGenislik = 300;
            int butonYukseklik = 80;
            int bosluk = 60; 

            int toplamGenislik = (3 * butonGenislik) + (2 * bosluk);

           
            int baslangicX = (w - toplamGenislik) / 2;


            int sabitY = h - 180;

            ButonStiliVer(btnYeni, "YENİ OYUN", baslangicX, sabitY, butonGenislik, butonYukseklik);

            ButonStiliVer(btnDevam, "DEVAM ET", baslangicX + butonGenislik + bosluk, sabitY, butonGenislik, butonYukseklik);

            ButonStiliVer(btnCikis, "ÇIKIŞ", baslangicX + (butonGenislik + bosluk) * 2, sabitY, butonGenislik, butonYukseklik);
        }

        private void ButonStiliVer(Button btn, string yazi, int x, int y, int w, int h)
        {
            if (btn == null) return;

           
            btn.Text = yazi;
            btn.Size = new Size(w, h);
            btn.Location = new Point(x, y);

            
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(220, 10, 10, 10); 
            btn.ForeColor = Color.WhiteSmoke;
            btn.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

          
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(100, 255, 0, 0); 
            btn.FlatAppearance.MouseOverBackColor = Color.DarkRed;
            btn.FlatAppearance.MouseDownBackColor = Color.Black;
        }

        
        private void btnYeni_Click(object sender, EventArgs e)
        {
            this.Hide(); 

            SenaryoYoneticisi.DurumuSifirla();
   
            Form1 oyun = new Form1(-1);
            oyun.Show();

           
            oyun.FormClosed += (s, args) => this.Show();
        }

        private void btnDevam_Click(object sender, EventArgs e)
        {
            try
            {
                int id = SenaryoYoneticisi.OyunuYukle();
                if (id > 0)
                {
                    this.Hide();
                    Form1 oyun = new Form1(id);
                    oyun.Show();

                    
                    oyun.FormClosed += (s, args) => this.Show();
                }
                else MessageBox.Show("Kayıt bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch { }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
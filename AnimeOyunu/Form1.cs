using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AnimeOyunu
{
    public partial class Form1 : Form
    {
        private ISahne aktifSahne;  
        private Timer oyunTimer;
        private Button btnPause;
        private Panel pnlPause;
        private int toplamSure, kalanSure;
        private Panel pnlZamanArka, pnlZamanOn, pnlOverlay;
        private Label lblOverlayBaslik, lblOverlayIcerik;
        private Button btnKalp, btnCanta, btnLog, btnSave, btnLoad;
        private Button btnSes;
        private Panel pnlSidebar;
      
        private int sidebarGenislikAcik = 200;
        private int sidebarGenislikKapali = 60;


        // yaoucu netıd
        public Form1(int baslangicId = 1)
        {
            InitializeComponent(); // ekrana formsu getirir
            this.WindowState = FormWindowState.Maximized; 
            this.FormBorderStyle = FormBorderStyle.None; 
                                                          

            this.DoubleBuffered = true; 
            this.BackColor = Color.Black;

            this.Tag = baslangicId;

            oyunTimer = new Timer { Interval = 25 }; 
            oyunTimer.Tick += (s, e) => OyunTimer_Tick(); 
            this.Resize += (s, e) => ArayuzuYerlestir();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                BasarimYoneticisi.BasarimlariYukle(); // abone mantığı
                SenaryoYoneticisi.OnIliskiDegisti += (isim, puan) => IlişkiAnimasyonuGoster(isim, puan); //bildirim mantığı

                // Sahne yükleme
                if (SenaryoYoneticisi.Sahneler.Count == 0)
                {
                    SenaryoYoneticisi.SenaryoyuYukle();
                }

                // Arka plan ayarları
                pbArkaPlan.Dock = DockStyle.Fill;
                pbArkaPlan.SizeMode = PictureBoxSizeMode.StretchImage;
                pbArkaPlan.SendToBack();

                // Hikaye kutusu
                lblHikaye.BackColor = Color.FromArgb(210, 0, 0, 0);
                lblHikaye.ForeColor = Color.White;
                lblHikaye.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblHikaye.TextAlign = ContentAlignment.MiddleCenter;

                // Zaman çubuğu
                pnlZamanArka = new Panel { Height = 6, BackColor = Color.FromArgb(100, 255, 255, 255), Visible = false };
                pnlZamanOn = new Panel { Height = 6, BackColor = Color.LimeGreen };
                pnlZamanArka.Controls.Add(pnlZamanOn);
                this.Controls.Add(pnlZamanArka);

                // Overlay paneli
                pnlOverlay = new Panel { BackColor = Color.FromArgb(250, 15, 15, 25), Visible = false };
                lblOverlayBaslik = new Label { Dock = DockStyle.Top, Height = 60, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.Gold, TextAlign = ContentAlignment.MiddleCenter };
                lblOverlayIcerik = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12), ForeColor = Color.WhiteSmoke, TextAlign = ContentAlignment.TopLeft, Padding = new Padding(35) };
                Button btnKapat = new Button { Dock = DockStyle.Bottom, Height = 50, Text = "KAPAT", FlatStyle = FlatStyle.Flat, BackColor = Color.Firebrick, ForeColor = Color.White };

                btnKapat.Click += (s, ev) =>
                {
                    pnlOverlay.Visible = false;
                    if (aktifSahne?.Id == 100) this.Close();
                };

                pnlOverlay.Controls.Add(lblOverlayIcerik);
                pnlOverlay.Controls.Add(lblOverlayBaslik);
                pnlOverlay.Controls.Add(btnKapat);
                this.Controls.Add(pnlOverlay);

                ArayuzuKur();
                PausePaneliniHazirla();

                // ✅ BAŞLANGIÇ ID'Sİ
                int baslangicId = 1;
                if (this.Tag is int id) baslangicId = id;

                // Müziği başlat
                SesYoneticisi.SesCal("gerilim_muzigi.wav", true);

                
                SahneyiEkranaBas(baslangicId);
            }
            catch (Exception ex)
            {
                HataGoster("Form Yükleme Hatası", ex);
            }
        }

         
        private void ArayuzuYerlestir()
        {
            try
            {
                int w = ClientSize.Width;
                int h = ClientSize.Height;
                int m = 30; // Kenar boşluğu

                // 1. Sidebar Genişliği (Kapalıysa 60, Açıksa 200 gibi düşün)
                int solBosluk = sidebarGenislikKapali + m;

                // 2. Hikaye Metni 
                int metinYukseklik = 130; // Metin kutusunun yüksekliği
                if (lblHikaye != null)
                {
                    // Metni ekranın en altına yapıştırıyoruz
                    lblHikaye.SetBounds(solBosluk, h - metinYukseklik - m, w - solBosluk - m, metinYukseklik);
                    lblHikaye.BringToFront();
                }

                // 3. Zaman Barı 
                if (pnlZamanArka != null && lblHikaye != null)
                {
                    pnlZamanArka.SetBounds(solBosluk, lblHikaye.Top - 15, lblHikaye.Width, 8);
                    pnlZamanArka.BringToFront();
                }

                // 4. Seçenek Butonları 
          
                int butonYukseklik = 60;
                int butonGenislik = (w - solBosluk - m - 40) / 2; // İki butonu yan yana sığdır

                // Butonların Y konumu: Metin kutusunun tepesinden 80 piksel yukarıda başlasın
                int butonY = (lblHikaye != null) ? lblHikaye.Top - butonYukseklik - 40 : h - 300;

                if (Secenek1 != null)
                {
                    Secenek1.SetBounds(solBosluk, butonY, butonGenislik, butonYukseklik);
                    Secenek1.BringToFront();
                }

                if (Secenek2 != null)
                {
                    // İkinci buton ilkinin bittiği yerden başlasın
                    Secenek2.SetBounds(solBosluk + butonGenislik + 40, butonY, butonGenislik, butonYukseklik);
                    Secenek2.BringToFront();
                }

                // 5. Overlay ve Diğerleri
                if (pnlOverlay != null && pnlOverlay.Visible)
                {
                    pnlOverlay.SetBounds((w - 750) / 2, (h - 550) / 2, 750, 550);
                    pnlOverlay.BringToFront();
                }

                // Pause Butonu (Sağ Üst)
                if (btnPause != null && !pnlSidebar.Controls.Contains(btnPause))
                {
                    btnPause.SetBounds(w - 80, 20, 60, 60);
                    btnPause.BringToFront();
                }
            }
            catch { }
        }

      
        public  void SahneyiEkranaBas(int id)
        {
            try
            {
                oyunTimer.Stop();
                aktifSahne = SenaryoYoneticisi.SahneGetir(id); // sahneyi ekranı getirmeye yarar

                if (aktifSahne == null)
                {
                    HataGoster("Sahne Hatası", new Exception($"Sahne ID {id} bulunamadı!"));
                    return;
                }

                lblHikaye.Text = SenaryoYoneticisi.MevcutMetniGetir(aktifSahne); // metni ve resmi güncelle

                if (aktifSahne is Sahne s) // desen eşleştirme yani s isminde bir değişken dönüştür ve koda gir bu sayede s.SecenekMetin gibi özelliklere erişebilirz
                {
                    // Ses dosyası
                    if (!string.IsNullOrEmpty(s.SesDosyasi))
                    {
                        bool dongu = !s.SesDosyasi.StartsWith("efekt_");
                        SesYoneticisi.SesCal(s.SesDosyasi, dongu);
                    }

                    // Olay aksiyonu örnek burada harita falan ekleniyorsa burada olur 
                    try
                    {
                        s.OlayAksiyonu?.Invoke(); // ? eğer olay aksyinou boş değilse devam et Invoke()--Bu metod Actionu tetikler
                    }
                    catch (Exception ex)
                    {
                        HataGoster("Olay Aksiyonu Hatası", ex);
                    }

                    // Resim yükleme
                    try
                    {
                        string yol = System.IO.Path.Combine(Application.StartupPath, "Gorseller", s.ResimAdi);
                        if (System.IO.File.Exists(yol))
                        {
                            if (pbArkaPlan.Image != null)
                            {
                                var oldImage = pbArkaPlan.Image;
                                pbArkaPlan.Image = null;
                                oldImage.Dispose();
                            }
                            pbArkaPlan.Image = Image.FromFile(yol);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Resim yükleme hatası: {ex.Message}");
                    }

                    // Buton 1
                    if (!string.IsNullOrEmpty(s.Secenek1Metni))
                    {
                        Secenek1.Text = s.Secenek1Metni + (string.IsNullOrEmpty(s.Secenek1Ipucu) ? "" : $"\n({s.Secenek1Ipucu})");
                        Secenek1.Visible = true;
                    }
                    else
                    {
                        Secenek1.Visible = false;
                    }

                    // Buton 2
                    if (!string.IsNullOrEmpty(s.Secenek2Metni))
                    {
                        Secenek2.Text = s.Secenek2Metni + (string.IsNullOrEmpty(s.Secenek2Ipucu) ? "" : $"\n({s.Secenek2Ipucu})");
                        Secenek2.Visible = true;
                    }
                    else
                    {
                        Secenek2.Visible = false;
                    }
                }

                // Timer başlat
                if ((Secenek1.Visible || Secenek2.Visible) && aktifSahne.Sure > 0)
                {
                    toplamSure = aktifSahne.Sure * 10;
                    kalanSure = toplamSure;
                    pnlZamanArka.Visible = true;
                    pnlZamanOn.Width = pnlZamanArka.Width;
                    oyunTimer.Start();
                }
                else
                {
                    pnlZamanArka.Visible = false;
                    oyunTimer.Stop();
                }

                ArayuzuYerlestir();
            }
            catch (Exception ex)
            {
                HataGoster("Sahne Yükleme Hatası", ex);
            }
        }

        // Form1.cs içinde herhangi bir yere yapıştır
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //override
        {
            if (keyData == Keys.Escape)
            {
                
                // Eğer panel henüz oluşturulmadıysa (null ise) hiçbir şey yapma, devam et.
                if (pnlPause == null) return base.ProcessCmdKey(ref msg, keyData);
                // -------------------

                if (pnlPause.Visible)
                    OyunuDevamEttir();
                else
                    OyunuDurdur();

                return true; // 
            }
            return base.ProcessCmdKey(ref msg, keyData); 
        }
        private async void btnSecenek1_Click(object sender, EventArgs e) // async metodu niye kullandım
        {
            try
            {
                if (aktifSahne == null) return;

                // BOSS SAVAŞI
                if (aktifSahne is SavasSahnesi sv)
                {
                    oyunTimer.Stop();

                    // Vuruş efekti
                    Color orijinalRenk = this.BackColor;
                    Color orijinalYaziRenk = lblHikaye.BackColor;
                    this.BackColor = Color.DarkRed;
                    lblHikaye.BackColor = Color.Red;
                    await System.Threading.Tasks.Task.Delay(100);//await özelliği
                    this.BackColor = orijinalRenk;
                    lblHikaye.BackColor = orijinalYaziRenk;

                    sv.Secenek1Aksiyonu?.Invoke();
                    sv.DusmanCan -= 35;

                    if (sv.DusmanCan <= 0)
                    {
                        // Zafer
                        Secenek1.Visible = false;
                        Secenek2.Visible = false;
                        pnlZamanArka.Visible = false;
                        lblHikaye.Text = $"\n\n*** {sv.DusmanIsmi.ToUpper()} ÖLDÜRÜLDÜ! ***\n\nYol açıldı...";
                        await System.Threading.Tasks.Task.Delay(2000);

                       
                        SahneyiEkranaBas(sv.ZaferSonrasiHedefId);
                    }
                    else
                    {
                        lblHikaye.Text = $"{sv.HikayeMetni}\n\n[DÜŞMAN SAĞLIĞI: %{sv.DusmanCan}]";
                        oyunTimer.Start();
                    }
                }
                // NORMAL SAHNE
                else if (aktifSahne is Sahne s) // s parametresinin önemi
                {
                    oyunTimer.Stop();
                    s.Secenek1Aksiyonu?.Invoke();
                    if (s.Secenek1HedefId > 0)
                    {
                      
                        SahneyiEkranaBas(s.Secenek1HedefId);
                    }
                }
            }
            catch (Exception ex)
            {
                HataGoster("Seçim Hatası", ex);
                oyunTimer.Start();
            }
        }
        private void btnSecenek2_Click(object sender, EventArgs e) // neden await kullanmadım 
        {
            try
            {
                oyunTimer.Stop();

                if (aktifSahne is Sahne s)
                {
                    s.Secenek2Aksiyonu?.Invoke();

                    if (s.Secenek2HedefId > 0)
                    {
                       
                        SahneyiEkranaBas(s.Secenek2HedefId);
                    }
                }
            }
            catch (Exception ex)
            {
                HataGoster("Seçim 2 Hatası", ex);
            }
        }

        private void pbArkaPlan_Click(object sender, EventArgs e) { }

        private void OyunTimer_Tick() // timer ayarlamma
        {
            try
            {
                if (kalanSure > 0)
                {
                    kalanSure--;
                    if (pnlZamanArka.Width > 0)
                    {
                        pnlZamanOn.Width = (int)(pnlZamanArka.Width * ((float)kalanSure / toplamSure));
                    }
                }
                else
                {
                    oyunTimer.Stop();
                    // Süre dolduğunda varsayılan seçeneği tetikle
                    if (Secenek1.Visible)
                    {
                        btnSecenek1_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                oyunTimer.Stop();
                System.Diagnostics.Debug.WriteLine($"Timer hatası: {ex.Message}");
            }
        }

        
        private void ArayuzuKur() // dinamik Uİ oluşturma 
        {
            try
            {
                // 1. Sidebar Panelini Oluştur (Yoksa)
                if (pnlSidebar == null)
                {
                    pnlSidebar = new Panel
                    {
                        BackColor = Color.FromArgb(180, 15, 15, 20),
                        Width = sidebarGenislikKapali,
                        Dock = DockStyle.Left,
                        Padding = new Padding(0, 20, 0, 0)
                    };
                    this.Controls.Add(pnlSidebar);
                    pnlSidebar.BringToFront();

                    // Mouse paneli üzerine gelince genişlesin, çıkınca daralsın
                    pnlSidebar.MouseEnter += (s, e) => { pnlSidebar.Width = sidebarGenislikAcik; };
                    pnlSidebar.MouseLeave += (s, e) => { pnlSidebar.Width = sidebarGenislikKapali; };
                }

                pnlSidebar.Controls.Clear();

  

                // İlişkiler
                btnKalp = YanMenüButonu("❤️", "İLİŞKİLER", (s, e) => OverlayGoster("İLİŞKİLER", SenaryoYoneticisi.ListeyiGetir()));

                // Envanter
                btnCanta = YanMenüButonu("🎒", "ENVANTER", (s, e) => OverlayGoster("ENVANTER", Envanter.Listele()));

                // Günlük
                btnLog = YanMenüButonu("📜", "GÜNLÜK", (s, e) => OverlayGoster("OYUN GÜNLÜĞÜ", SenaryoYoneticisi.LoglariGetir()));

                // Durdur
                btnPause = YanMenüButonu("⏸️", "DURDUR", (s, e) => OyunuDurdur());
                btnPause.BackColor = Color.OrangeRed;

                // Başarımlar
                Button btnBasarim = YanMenüButonu("🏆", "BAŞARIMLAR", (s, e) => OverlayGoster("BAŞARIMLAR", BasarimYoneticisi.BasarimlariListele()));
                btnBasarim.Name = "btnBasarim";

                // Kelebek Etkisi
                Button btnKelebek = YanMenüButonu("🦋", "KELEBEK", (s, e) => OverlayGoster("KELEBEK ETKİSİ", BasarimYoneticisi.KelebekHaritasiniGetir()));
                btnKelebek.Name = "btnKelebek";

                // --- KAYDET BUTONU ---
                btnSave = YanMenüButonu("💾", "KAYDET", (s, e) =>
                {
                    try
                    { 
                        SenaryoYoneticisi.OyunuKaydet();
                        MessageBox.Show("Oyunbaşarıyla kaydedildi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information); //messagebox nedir acıkla
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kaydetme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

                btnLoad = YanMenüButonu("📂", "YÜKLE", (s, e) =>
                {
                    try
                    {
                        int yuklenenId = SenaryoYoneticisi.OyunuYukle();
                        if (yuklenenId > 0)
                        {
                            SahneyiEkranaBas(yuklenenId);
                            MessageBox.Show("Oyun yüklendi! Kaldığın yerden devam ediyorsun.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Daha önce alınmış bir kayıt dosyası bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Yükleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

                // Ses Butonu
                btnSes = YanMenüButonu("🔊", "SES", (s, e) => {
                    SesYoneticisi.SesiKapatAc();
                    btnSes.Text = SesYoneticisi.SesAcik ? "🔊   SES" : "🔇   SES";
                });

                ArayuzuYerlestir();
            }
            catch (Exception ex)
            {
                HataGoster("Arayüz Kurulum Hatası", ex);
            }
        }

        private Button YanMenüButonu(string ikon, string metin, EventHandler h)
        {
            Button b = new Button
            {
                Text = ikon + "   " + metin, // İkon ve İsim yan yana
                Size = new Size(sidebarGenislikAcik, 60),
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent, // Panel rengini alsın
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Height = 60
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += h;

            // Mouse butona gelince de paneli genişlet
            b.MouseEnter += (s, e) => { pnlSidebar.Width = sidebarGenislikAcik; };

            pnlSidebar.Controls.Add(b);
            return b;
        }

        public void OverlayGoster(string b, string i)
        {
            try
            {
                OyunuDurdur(); // Menü açıldığı an zamanı durdur

                lblOverlayBaslik.Text = b;
                lblOverlayIcerik.Text = i;
                pnlOverlay.Visible = true;
                pnlOverlay.BringToFront();
                ArayuzuYerlestir();
            }
            catch (Exception ex)
            {
                HataGoster("Overlay Hatası", ex);
            }
        }

        // MERKEZI HATA YÖNETİMİ
        private void HataGoster(string baslik, Exception ex)
        {
            string mesaj = $"Hata: {ex.Message}\n\nDetay: {ex.StackTrace}";
            MessageBox.Show(mesaj, baslik, MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Hata logunu "Veriler" klasörüne yaz
            try
            {
                string klasorYolu = System.IO.Path.Combine(Application.StartupPath, "Veriler");

                // Klasör kontrolü (Garanti olsun)
                if (!System.IO.Directory.Exists(klasorYolu))
                {
                    System.IO.Directory.CreateDirectory(klasorYolu);
                }

                string logDosyasi = System.IO.Path.Combine(klasorYolu, "hata_log.txt");
                string log = $"[{DateTime.Now}] {baslik}\n{mesaj}\n{new string('-', 80)}\n";

                System.IO.File.AppendAllText(logDosyasi, log);
            }
            catch
            {
                // Log yazarken hata alırsak sessizce geç, oyunu bozma.
            }
        }
        // Form1.cs class'ının içine, en alta bu metodu ekle:
        // Form1.cs içinde en alttaki bu metodu bul ve tamamen bununla değiştir:
        private void PausePaneliniHazirla()
        {
            // 1. PAUSE PANELİ
            pnlPause = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(200, 0, 0, 0),
                Visible = false
            };

   
            Panel pnlKutu = new Panel
            {
                Size = new Size(400, 300),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlKutu.Left = (this.ClientSize.Width - 400) / 2;
            pnlKutu.Top = (this.ClientSize.Height - 300) / 2;

            this.Resize += (s, e) => {
                if (pnlKutu != null)
                {
                    pnlKutu.Left = (this.ClientSize.Width - 400) / 2;
                    pnlKutu.Top = (this.ClientSize.Height - 300) / 2;
                }
            };

            Label lblBaslik = new Label
            {
                Text = "OYUN DURAKLATILDI",
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Impact", 24),
                ForeColor = Color.White
            };

          

            Button btnDevamEt = new Button
            {
                Text = "▶️ DEVAM ET",
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDevamEt.Click += (s, e) => OyunuDevamEttir();

            Button btnAnaMenu = new Button
            {
                Text = "🏠 ANA MENÜ",
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            // Kapanma hatası burada giderildi:
            btnAnaMenu.Click += (s, e) => { this.Close(); };

            // Elemanları kutuya ekle
            pnlKutu.Controls.Add(btnDevamEt);
            pnlKutu.Controls.Add(lblBaslik);
            pnlKutu.Controls.Add(btnAnaMenu);

            pnlPause.Controls.Add(pnlKutu);
            this.Controls.Add(pnlPause);
            pnlPause.BringToFront();
        }

        // Oyunu Durduran Metot
        public void OyunuDurdur()
        {
            if (oyunTimer.Enabled) oyunTimer.Stop(); // Zamanı dondur
            pnlPause.Visible = true; // Menüyü aç
            pnlPause.BringToFront();
        }

        // Oyunu Devam Ettiren Metot
        public void OyunuDevamEttir()
        {
            pnlPause.Visible = false; // Menüyü gizle

            // Eğer sahnede zaman çubuğu varsa ve görünürse süreyi tekrar başlat
            // (Böylece süresiz sahnelerde boşuna timer çalışmaz)
            if (pnlZamanArka.Visible)
            {
                oyunTimer.Start();
            }
        }

        // ==========================================
   

        private void IlişkiAnimasyonuGoster(string isim, int puan)
        {
            // Label Oluştur
            Label lblEfekt = new Label();
            lblEfekt.Text = $"{isim}: {(puan > 0 ? "+" : "")}{puan}";
            lblEfekt.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblEfekt.AutoSize = true;
            lblEfekt.BackColor = Color.Transparent;

            // Renk Ayarı: Pozitifse Yeşil, Negatifse Kırmızı
            lblEfekt.ForeColor = puan > 0 ? Color.LimeGreen : Color.Red;

            // Konum: Ekranın sol üst-orta kısmında çıksın
            lblEfekt.Location = new Point(50, 200);

            this.Controls.Add(lblEfekt);
            lblEfekt.BringToFront();

            // timer animasyonu
            Timer animasyonTimer = new Timer();
            animasyonTimer.Interval = 30; // Hız
            int sayac = 0;



            animasyonTimer.Tick += (s, e) =>
            {
                lblEfekt.Top -= 2; // Yukarı kaydır
                sayac++;

                // 30 adım (yaklaşık 1 saniye) sonra yok et
                if (sayac > 30)
                {
                    animasyonTimer.Stop();
                    this.Controls.Remove(lblEfekt);
                    lblEfekt.Dispose();
                    animasyonTimer.Dispose();
                }
            };


            animasyonTimer.Start();
        }
    }
}
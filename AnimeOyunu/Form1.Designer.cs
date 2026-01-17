namespace AnimeOyunu
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pbArkaPlan;
        private System.Windows.Forms.Label lblHikaye;
        private System.Windows.Forms.Button Secenek1;
        private System.Windows.Forms.Button Secenek2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pbArkaPlan = new System.Windows.Forms.PictureBox();
            this.lblHikaye = new System.Windows.Forms.Label();
            this.Secenek1 = new System.Windows.Forms.Button();
            this.Secenek2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbArkaPlan)).BeginInit();
            this.SuspendLayout();

            // pbArkaPlan
            this.pbArkaPlan.Location = new System.Drawing.Point(0, 0);
            this.pbArkaPlan.Name = "pbArkaPlan";
            this.pbArkaPlan.Size = new System.Drawing.Size(800, 450);
            this.pbArkaPlan.TabStop = false;
            this.pbArkaPlan.Click += new System.EventHandler(this.pbArkaPlan_Click);

            // lblHikaye
            this.lblHikaye.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblHikaye.Location = new System.Drawing.Point(12, 340);
            this.lblHikaye.Size = new System.Drawing.Size(776, 90);
            this.lblHikaye.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHikaye.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblHikaye.ForeColor = System.Drawing.Color.White;
            this.lblHikaye.BackColor = System.Drawing.Color.FromArgb(200, 0, 0, 0);

            // Secenek1
            this.Secenek1.Location = new System.Drawing.Point(30, 270);
            this.Secenek1.Size = new System.Drawing.Size(150, 50);
            this.Secenek1.Text = "Seçenek 1";
            this.Secenek1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.Secenek1.ForeColor = System.Drawing.Color.White;
            this.Secenek1.BackColor = System.Drawing.Color.FromArgb(180, 30, 30, 30);
            this.Secenek1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Secenek1.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.Secenek1.FlatAppearance.BorderSize = 2;
            this.Secenek1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(200, 60, 60, 60);
            this.Secenek1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Secenek1.Click += new System.EventHandler(this.btnSecenek1_Click);

            // Secenek2
            this.Secenek2.Location = new System.Drawing.Point(620, 270);
            this.Secenek2.Size = new System.Drawing.Size(150, 50);
            this.Secenek2.Text = "Seçenek 2";
            this.Secenek2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.Secenek2.ForeColor = System.Drawing.Color.White;
            this.Secenek2.BackColor = System.Drawing.Color.FromArgb(180, 30, 30, 30);
            this.Secenek2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Secenek2.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.Secenek2.FlatAppearance.BorderSize = 2;
            this.Secenek2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(200, 60, 60, 60);
            this.Secenek2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Secenek2.Click += new System.EventHandler(this.btnSecenek2_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Secenek2);
            this.Controls.Add(this.Secenek1);
            this.Controls.Add(this.lblHikaye);
            this.Controls.Add(this.pbArkaPlan);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Text = "Anime Oyunu - Zombi Apokalipsi";
            this.BackColor = System.Drawing.Color.Black;

            ((System.ComponentModel.ISupportInitialize)(this.pbArkaPlan)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
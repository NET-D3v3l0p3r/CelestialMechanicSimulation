namespace CelestialMechanicsSimulator.GUI
{
    partial class UI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.planet_path = new System.Windows.Forms.PictureBox();
            this.PName = new System.Windows.Forms.Label();
            this.PSize = new System.Windows.Forms.Label();
            this.PA = new System.Windows.Forms.Label();
            this.PB = new System.Windows.Forms.Label();
            this.numeric = new System.Windows.Forms.Label();
            this.linear = new System.Windows.Forms.Label();
            this.a_p = new System.Windows.Forms.Label();
            this.lb_vx = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_vx = new System.Windows.Forms.TrackBar();
            this.lb_vy = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_vy = new System.Windows.Forms.TrackBar();
            this.lb_vz = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_vz = new System.Windows.Forms.TrackBar();
            this.panel_tb = new System.Windows.Forms.Panel();
            this.mass = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_mass = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.planet_path)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vz)).BeginInit();
            this.panel_tb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_mass)).BeginInit();
            this.SuspendLayout();
            // 
            // planet_path
            // 
            this.planet_path.Location = new System.Drawing.Point(3, 133);
            this.planet_path.Name = "planet_path";
            this.planet_path.Size = new System.Drawing.Size(286, 238);
            this.planet_path.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.planet_path.TabIndex = 0;
            this.planet_path.TabStop = false;
            this.planet_path.Paint += new System.Windows.Forms.PaintEventHandler(this.planet_path_Paint);
            // 
            // PName
            // 
            this.PName.AutoSize = true;
            this.PName.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PName.ForeColor = System.Drawing.Color.Red;
            this.PName.Location = new System.Drawing.Point(25, 18);
            this.PName.Name = "PName";
            this.PName.Size = new System.Drawing.Size(59, 16);
            this.PName.TabIndex = 1;
            this.PName.Text = "Planet:";
            // 
            // PSize
            // 
            this.PSize.AutoSize = true;
            this.PSize.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PSize.ForeColor = System.Drawing.Color.Red;
            this.PSize.Location = new System.Drawing.Point(25, 34);
            this.PSize.Name = "PSize";
            this.PSize.Size = new System.Drawing.Size(38, 16);
            this.PSize.TabIndex = 3;
            this.PSize.Text = "Size:";
            // 
            // PA
            // 
            this.PA.AutoSize = true;
            this.PA.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PA.ForeColor = System.Drawing.Color.Red;
            this.PA.Location = new System.Drawing.Point(25, 50);
            this.PA.Name = "PA";
            this.PA.Size = new System.Drawing.Size(91, 16);
            this.PA.TabIndex = 4;
            this.PA.Text = "MajorAxisA:";
            // 
            // PB
            // 
            this.PB.AutoSize = true;
            this.PB.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PB.ForeColor = System.Drawing.Color.Red;
            this.PB.Location = new System.Drawing.Point(25, 66);
            this.PB.Name = "PB";
            this.PB.Size = new System.Drawing.Size(87, 16);
            this.PB.TabIndex = 5;
            this.PB.Text = "MinorAxisB:";
            // 
            // numeric
            // 
            this.numeric.AutoSize = true;
            this.numeric.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric.ForeColor = System.Drawing.Color.Red;
            this.numeric.Location = new System.Drawing.Point(25, 82);
            this.numeric.Name = "numeric";
            this.numeric.Size = new System.Drawing.Size(19, 16);
            this.numeric.TabIndex = 6;
            this.numeric.Text = "ε:";
            // 
            // linear
            // 
            this.linear.AutoSize = true;
            this.linear.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linear.ForeColor = System.Drawing.Color.Red;
            this.linear.Location = new System.Drawing.Point(25, 98);
            this.linear.Name = "linear";
            this.linear.Size = new System.Drawing.Size(151, 16);
            this.linear.TabIndex = 7;
            this.linear.Text = "Lineare Exccentricty:";
            // 
            // a_p
            // 
            this.a_p.AutoSize = true;
            this.a_p.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.a_p.ForeColor = System.Drawing.Color.Red;
            this.a_p.Location = new System.Drawing.Point(25, 114);
            this.a_p.Name = "a_p";
            this.a_p.Size = new System.Drawing.Size(117, 16);
            this.a_p.TabIndex = 8;
            this.a_p.Text = "Aphel - Periphel:";
            // 
            // lb_vx
            // 
            this.lb_vx.AutoSize = true;
            this.lb_vx.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_vx.ForeColor = System.Drawing.Color.Red;
            this.lb_vx.Location = new System.Drawing.Point(5, 134);
            this.lb_vx.Name = "lb_vx";
            this.lb_vx.Size = new System.Drawing.Size(45, 16);
            this.lb_vx.TabIndex = 21;
            this.lb_vx.Text = "0 m/s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(3, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Vel_X:";
            // 
            // tb_vx
            // 
            this.tb_vx.Location = new System.Drawing.Point(49, 105);
            this.tb_vx.Name = "tb_vx";
            this.tb_vx.Size = new System.Drawing.Size(205, 45);
            this.tb_vx.TabIndex = 19;
            this.tb_vx.ValueChanged += new System.EventHandler(this.tb_vx_ValueChanged);
            // 
            // lb_vy
            // 
            this.lb_vy.AutoSize = true;
            this.lb_vy.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_vy.ForeColor = System.Drawing.Color.Red;
            this.lb_vy.Location = new System.Drawing.Point(5, 216);
            this.lb_vy.Name = "lb_vy";
            this.lb_vy.Size = new System.Drawing.Size(45, 16);
            this.lb_vy.TabIndex = 24;
            this.lb_vy.Text = "0 m/s";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(3, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Vel_Y:";
            // 
            // tb_vy
            // 
            this.tb_vy.Location = new System.Drawing.Point(49, 187);
            this.tb_vy.Name = "tb_vy";
            this.tb_vy.Size = new System.Drawing.Size(205, 45);
            this.tb_vy.TabIndex = 22;
            this.tb_vy.ValueChanged += new System.EventHandler(this.tb_vy_ValueChanged);
            // 
            // lb_vz
            // 
            this.lb_vz.AutoSize = true;
            this.lb_vz.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_vz.ForeColor = System.Drawing.Color.Red;
            this.lb_vz.Location = new System.Drawing.Point(5, 303);
            this.lb_vz.Name = "lb_vz";
            this.lb_vz.Size = new System.Drawing.Size(45, 16);
            this.lb_vz.TabIndex = 27;
            this.lb_vz.Text = "0 m/s";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(3, 274);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 16);
            this.label7.TabIndex = 26;
            this.label7.Text = "Vel_Z";
            // 
            // tb_vz
            // 
            this.tb_vz.Location = new System.Drawing.Point(49, 274);
            this.tb_vz.Name = "tb_vz";
            this.tb_vz.Size = new System.Drawing.Size(205, 45);
            this.tb_vz.TabIndex = 25;
            this.tb_vz.ValueChanged += new System.EventHandler(this.tb_vz_ValueChanged);
            // 
            // panel_tb
            // 
            this.panel_tb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_tb.Controls.Add(this.mass);
            this.panel_tb.Controls.Add(this.label2);
            this.panel_tb.Controls.Add(this.tb_mass);
            this.panel_tb.Controls.Add(this.label3);
            this.panel_tb.Controls.Add(this.lb_vz);
            this.panel_tb.Controls.Add(this.tb_vx);
            this.panel_tb.Controls.Add(this.label7);
            this.panel_tb.Controls.Add(this.lb_vx);
            this.panel_tb.Controls.Add(this.tb_vz);
            this.panel_tb.Controls.Add(this.tb_vy);
            this.panel_tb.Controls.Add(this.lb_vy);
            this.panel_tb.Controls.Add(this.label5);
            this.panel_tb.Location = new System.Drawing.Point(7, 377);
            this.panel_tb.Name = "panel_tb";
            this.panel_tb.Size = new System.Drawing.Size(275, 372);
            this.panel_tb.TabIndex = 28;
            // 
            // mass
            // 
            this.mass.AutoSize = true;
            this.mass.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mass.ForeColor = System.Drawing.Color.Red;
            this.mass.Location = new System.Drawing.Point(4, 48);
            this.mass.Name = "mass";
            this.mass.Size = new System.Drawing.Size(34, 16);
            this.mass.TabIndex = 30;
            this.mass.Text = "0 kg";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Quartz MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "Mass:";
            // 
            // tb_mass
            // 
            this.tb_mass.Location = new System.Drawing.Point(49, 10);
            this.tb_mass.Name = "tb_mass";
            this.tb_mass.Size = new System.Drawing.Size(205, 45);
            this.tb_mass.TabIndex = 28;
            this.tb_mass.ValueChanged += new System.EventHandler(this.tb_mass_ValueChanged_1);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(288, 761);
            this.Controls.Add(this.panel_tb);
            this.Controls.Add(this.a_p);
            this.Controls.Add(this.linear);
            this.Controls.Add(this.numeric);
            this.Controls.Add(this.PB);
            this.Controls.Add(this.PA);
            this.Controls.Add(this.PSize);
            this.Controls.Add(this.PName);
            this.Controls.Add(this.planet_path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UI";
            this.Text = "GUI";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GUI_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.planet_path)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_vz)).EndInit();
            this.panel_tb.ResumeLayout(false);
            this.panel_tb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_mass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox planet_path;
        private System.Windows.Forms.Label PName;
        private System.Windows.Forms.Label PSize;
        private System.Windows.Forms.Label PA;
        private System.Windows.Forms.Label PB;
        private System.Windows.Forms.Label numeric;
        private System.Windows.Forms.Label linear;
        private System.Windows.Forms.Label a_p;
        private System.Windows.Forms.Label lb_vx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar tb_vx;
        private System.Windows.Forms.Label lb_vy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar tb_vy;
        private System.Windows.Forms.Label lb_vz;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar tb_vz;
        private System.Windows.Forms.Panel panel_tb;
        private System.Windows.Forms.Label mass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tb_mass;

    }
}
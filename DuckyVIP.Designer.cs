namespace Ducky_CMD
{
    partial class DuckyVIP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DuckyVIP));
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.toggle_leader = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.comboBox_miningSpeed = new Guna.UI2.WinForms.Guna2ComboBox();
            this.toggle_IsolateMSpeed = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(100, 12);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(116, 95);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 0;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.Click += new System.EventHandler(this.guna2PictureBox1_Click);
            // 
            // toggle_leader
            // 
            this.toggle_leader.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggle_leader.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggle_leader.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggle_leader.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggle_leader.Location = new System.Drawing.Point(94, 171);
            this.toggle_leader.Name = "toggle_leader";
            this.toggle_leader.Size = new System.Drawing.Size(35, 20);
            this.toggle_leader.TabIndex = 1;
            this.toggle_leader.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggle_leader.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggle_leader.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggle_leader.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.toggle_leader.CheckedChanged += new System.EventHandler(this.guna2ToggleSwitch1_CheckedChanged);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.IndianRed;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(101, 136);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(115, 27);
            this.guna2HtmlLabel1.TabIndex = 2;
            this.guna2HtmlLabel1.Text = "--------------";
            this.guna2HtmlLabel1.Click += new System.EventHandler(this.guna2HtmlLabel1_Click);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.DimGray;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(135, 172);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(108, 19);
            this.guna2HtmlLabel2.TabIndex = 3;
            this.guna2HtmlLabel2.Text = "Set as AFK Leader";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox2.Image")));
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(72, -2);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(169, 137);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox2.TabIndex = 4;
            this.guna2PictureBox2.TabStop = false;
            this.guna2PictureBox2.Visible = false;
            this.guna2PictureBox2.Click += new System.EventHandler(this.guna2PictureBox2_Click);
            // 
            // comboBox_miningSpeed
            // 
            this.comboBox_miningSpeed.BackColor = System.Drawing.Color.Transparent;
            this.comboBox_miningSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(130)))), ((int)(((byte)(96)))));
            this.comboBox_miningSpeed.BorderRadius = 10;
            this.comboBox_miningSpeed.BorderThickness = 3;
            this.comboBox_miningSpeed.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox_miningSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_miningSpeed.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(172)))), ((int)(((byte)(128)))));
            this.comboBox_miningSpeed.FocusedColor = System.Drawing.Color.IndianRed;
            this.comboBox_miningSpeed.FocusedState.BorderColor = System.Drawing.Color.IndianRed;
            this.comboBox_miningSpeed.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_miningSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboBox_miningSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_miningSpeed.ItemHeight = 30;
            this.comboBox_miningSpeed.Items.AddRange(new object[] {
            "0.1s",
            "0.5s",
            "1s",
            "2s",
            "3s",
            "Hold"});
            this.comboBox_miningSpeed.Location = new System.Drawing.Point(11, 187);
            this.comboBox_miningSpeed.Name = "comboBox_miningSpeed";
            this.comboBox_miningSpeed.Size = new System.Drawing.Size(74, 36);
            this.comboBox_miningSpeed.TabIndex = 14;
            this.comboBox_miningSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.comboBox_miningSpeed.TextOffset = new System.Drawing.Point(20, 0);
            this.comboBox_miningSpeed.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.comboBox_miningSpeed.Visible = false;
            this.comboBox_miningSpeed.SelectedIndexChanged += new System.EventHandler(this.comboBox_miningSpeed_SelectedIndexChanged);
            // 
            // toggle_IsolateMSpeed
            // 
            this.toggle_IsolateMSpeed.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggle_IsolateMSpeed.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggle_IsolateMSpeed.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggle_IsolateMSpeed.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggle_IsolateMSpeed.Location = new System.Drawing.Point(94, 199);
            this.toggle_IsolateMSpeed.Name = "toggle_IsolateMSpeed";
            this.toggle_IsolateMSpeed.Size = new System.Drawing.Size(35, 20);
            this.toggle_IsolateMSpeed.TabIndex = 15;
            this.toggle_IsolateMSpeed.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggle_IsolateMSpeed.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.toggle_IsolateMSpeed.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggle_IsolateMSpeed.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.toggle_IsolateMSpeed.CheckedChanged += new System.EventHandler(this.toggle_IsolateMSpeed_CheckedChanged);
            // 
            // guna2HtmlLabel3
            // 
            this.guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel3.ForeColor = System.Drawing.Color.DimGray;
            this.guna2HtmlLabel3.Location = new System.Drawing.Point(135, 199);
            this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            this.guna2HtmlLabel3.Size = new System.Drawing.Size(126, 19);
            this.guna2HtmlLabel3.TabIndex = 16;
            this.guna2HtmlLabel3.Text = "Isolate Mining Speed";
            this.guna2HtmlLabel3.DoubleClick += new System.EventHandler(this.guna2HtmlLabel3_DoubleClick);
            // 
            // DuckyVIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(310, 232);
            this.Controls.Add(this.guna2HtmlLabel3);
            this.Controls.Add(this.toggle_IsolateMSpeed);
            this.Controls.Add(this.comboBox_miningSpeed);
            this.Controls.Add(this.guna2HtmlLabel2);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.toggle_leader);
            this.Controls.Add(this.guna2PictureBox1);
            this.Controls.Add(this.guna2PictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(666, 233);
            this.Name = "DuckyVIP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DuckyVIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DuckyVIP_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggle_leader;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private Guna.UI2.WinForms.Guna2ComboBox comboBox_miningSpeed;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggle_IsolateMSpeed;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
    }
}
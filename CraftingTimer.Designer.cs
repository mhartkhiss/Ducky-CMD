namespace Ducky_CMD
{
    partial class CraftingTimer
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
            this.components = new System.ComponentModel.Container();
            this.txt_timer = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_water = new Guna.UI2.WinForms.Guna2Button();
            this.btn_ice = new Guna.UI2.WinForms.Guna2Button();
            this.btn_purple = new Guna.UI2.WinForms.Guna2Button();
            this.btn_green = new Guna.UI2.WinForms.Guna2Button();
            this.btn_red = new Guna.UI2.WinForms.Guna2Button();
            this.btn_yellow = new Guna.UI2.WinForms.Guna2Button();
            this.txt_island = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // txt_timer
            // 
            this.txt_timer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_timer.AutoSize = false;
            this.txt_timer.BackColor = System.Drawing.Color.Transparent;
            this.txt_timer.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_timer.ForeColor = System.Drawing.Color.Aqua;
            this.txt_timer.Location = new System.Drawing.Point(0, 10);
            this.txt_timer.Name = "txt_timer";
            this.txt_timer.Size = new System.Drawing.Size(223, 33);
            this.txt_timer.TabIndex = 0;
            this.txt_timer.Text = "1h 48m 2s";
            this.txt_timer.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_water
            // 
            this.btn_water.BorderThickness = 2;
            this.btn_water.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_water.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_water.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_water.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_water.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_water.ForeColor = System.Drawing.Color.White;
            this.btn_water.Location = new System.Drawing.Point(35, 57);
            this.btn_water.Name = "btn_water";
            this.btn_water.Size = new System.Drawing.Size(18, 18);
            this.btn_water.TabIndex = 1;
            this.btn_water.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // btn_ice
            // 
            this.btn_ice.BorderThickness = 2;
            this.btn_ice.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_ice.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_ice.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_ice.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_ice.FillColor = System.Drawing.Color.LightCyan;
            this.btn_ice.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_ice.ForeColor = System.Drawing.Color.White;
            this.btn_ice.Location = new System.Drawing.Point(59, 57);
            this.btn_ice.Name = "btn_ice";
            this.btn_ice.Size = new System.Drawing.Size(18, 18);
            this.btn_ice.TabIndex = 2;
            // 
            // btn_purple
            // 
            this.btn_purple.BorderThickness = 2;
            this.btn_purple.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_purple.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_purple.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_purple.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_purple.FillColor = System.Drawing.Color.Plum;
            this.btn_purple.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_purple.ForeColor = System.Drawing.Color.White;
            this.btn_purple.Location = new System.Drawing.Point(83, 57);
            this.btn_purple.Name = "btn_purple";
            this.btn_purple.Size = new System.Drawing.Size(18, 18);
            this.btn_purple.TabIndex = 3;
            // 
            // btn_green
            // 
            this.btn_green.BorderThickness = 2;
            this.btn_green.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_green.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_green.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_green.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_green.FillColor = System.Drawing.Color.LightGreen;
            this.btn_green.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_green.ForeColor = System.Drawing.Color.White;
            this.btn_green.Location = new System.Drawing.Point(107, 57);
            this.btn_green.Name = "btn_green";
            this.btn_green.Size = new System.Drawing.Size(18, 18);
            this.btn_green.TabIndex = 4;
            // 
            // btn_red
            // 
            this.btn_red.BorderThickness = 2;
            this.btn_red.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_red.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_red.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_red.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_red.FillColor = System.Drawing.Color.IndianRed;
            this.btn_red.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_red.ForeColor = System.Drawing.Color.White;
            this.btn_red.Location = new System.Drawing.Point(131, 57);
            this.btn_red.Name = "btn_red";
            this.btn_red.Size = new System.Drawing.Size(18, 18);
            this.btn_red.TabIndex = 5;
            // 
            // btn_yellow
            // 
            this.btn_yellow.BorderThickness = 2;
            this.btn_yellow.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_yellow.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_yellow.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_yellow.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_yellow.FillColor = System.Drawing.Color.Khaki;
            this.btn_yellow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_yellow.ForeColor = System.Drawing.Color.White;
            this.btn_yellow.Location = new System.Drawing.Point(155, 57);
            this.btn_yellow.Name = "btn_yellow";
            this.btn_yellow.Size = new System.Drawing.Size(18, 18);
            this.btn_yellow.TabIndex = 6;
            // 
            // txt_island
            // 
            this.txt_island.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_island.AutoSize = false;
            this.txt_island.BackColor = System.Drawing.Color.Transparent;
            this.txt_island.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_island.ForeColor = System.Drawing.Color.Silver;
            this.txt_island.Location = new System.Drawing.Point(176, 49);
            this.txt_island.Name = "txt_island";
            this.txt_island.Size = new System.Drawing.Size(110, 33);
            this.txt_island.TabIndex = 7;
            this.txt_island.Text = "#0000";
            this.txt_island.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CraftingTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(292, 89);
            this.Controls.Add(this.txt_island);
            this.Controls.Add(this.btn_yellow);
            this.Controls.Add(this.btn_red);
            this.Controls.Add(this.btn_green);
            this.Controls.Add(this.btn_purple);
            this.Controls.Add(this.btn_ice);
            this.Controls.Add(this.btn_water);
            this.Controls.Add(this.txt_timer);
            this.Name = "CraftingTimer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CraftingTimer";
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel txt_timer;
        private System.Windows.Forms.Timer timer1;
        private Guna.UI2.WinForms.Guna2Button btn_water;
        private Guna.UI2.WinForms.Guna2Button btn_ice;
        private Guna.UI2.WinForms.Guna2Button btn_purple;
        private Guna.UI2.WinForms.Guna2Button btn_green;
        private Guna.UI2.WinForms.Guna2Button btn_red;
        private Guna.UI2.WinForms.Guna2Button btn_yellow;
        private Guna.UI2.WinForms.Guna2HtmlLabel txt_island;
    }
}
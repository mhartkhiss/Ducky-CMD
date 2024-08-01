namespace Ducky_CMD
{
    partial class ToastNotification
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
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label_dcm = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label_ign = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 20;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.DragForm = false;
            this.guna2BorderlessForm1.ResizeForm = false;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = global::Ducky_CMD.Properties.Resources.jackpot__1_;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(12, 12);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(64, 64);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.guna2PictureBox1.TabIndex = 1;
            this.guna2PictureBox1.TabStop = false;
            // 
            // label_dcm
            // 
            this.label_dcm.BackColor = System.Drawing.Color.Transparent;
            this.label_dcm.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_dcm.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label_dcm.Location = new System.Drawing.Point(90, 46);
            this.label_dcm.Name = "label_dcm";
            this.label_dcm.Size = new System.Drawing.Size(226, 20);
            this.label_dcm.TabIndex = 3;
            this.label_dcm.Text = "got 3123 $DCM when mining.";
            // 
            // label_ign
            // 
            this.label_ign.BackColor = System.Drawing.Color.Transparent;
            this.label_ign.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ign.ForeColor = System.Drawing.Color.Brown;
            this.label_ign.Location = new System.Drawing.Point(90, 17);
            this.label_ign.Name = "label_ign";
            this.label_ign.Size = new System.Drawing.Size(57, 25);
            this.label_ign.TabIndex = 2;
            this.label_ign.Text = "Dusk";
            // 
            // ToastNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(342, 90);
            this.Controls.Add(this.label_dcm);
            this.Controls.Add(this.label_ign);
            this.Controls.Add(this.guna2PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToastNotification";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ToastNotification";
            this.TopMost = true;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ToastNotification_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel label_dcm;
        private Guna.UI2.WinForms.Guna2HtmlLabel label_ign;
    }
}
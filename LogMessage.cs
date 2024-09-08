using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ducky_CMD
{
    public partial class LogMessage : UserControl
    {
        private Timer colorTimer; // Timer to change color after 5 seconds
        private Color targetColor; // The color to change to after 5 seconds

        public LogMessage()
        {
            InitializeComponent();
            this.Width = 237;
            this.AutoSize = false;
            messageLabel.AutoSize = true;
            messageLabel.MaximumSize = new Size(this.Width, 0);

            // Set initial background color to orange
            guna2Panel1.BackColor = Color.Orange;

            // Initialize and start the timer
            colorTimer = new Timer();
            colorTimer.Interval = 5000; // 5 seconds
            colorTimer.Tick += ColorTimer_Tick;
            colorTimer.Start();
        }

        public string Date
        {
            get => dateLabel.Text;
            set => dateLabel.Text = value;
        }

        public string Message
        {
            get => messageLabel.Text;
            set
            {
                messageLabel.Text = value;
                AdjustDateLabelPosition();
            }
        }

        public Color MessageColor
        {
            get => targetColor;
            set => targetColor = value;
        }

        private void AdjustDateLabelPosition()
        {
            int padding = 5;
            dateLabel.Top = messageLabel.Bottom + padding;
            this.Height = dateLabel.Bottom + padding;
        }

        private void ColorTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            colorTimer.Stop();

            // Change the background color to the target color
            guna2Panel1.BackColor = targetColor;
        }
    }
}

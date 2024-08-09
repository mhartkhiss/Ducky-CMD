using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ducky_CMD
{
    public partial class LogMessage : UserControl
    {
        public LogMessage()
        {
            InitializeComponent();
            this.Width = 237; // Set the default width of the UserControl
            this.AutoSize = false;

            // Ensure messageLabel auto-sizes vertically based on its content
            messageLabel.AutoSize = true;
            messageLabel.MaximumSize = new Size(this.Width, 0); // Ensure it wraps text within the control's width
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
                AdjustDateLabelPosition(); // Adjust dateLabel position when message is set
            }
        }

        public Color MessageColor
        {
            get => guna2Panel1.BackColor;
            set => guna2Panel1.BackColor = value;
        }

        private void AdjustDateLabelPosition()
        {
            // Move the dateLabel below the messageLabel, with some padding
            int padding = 5; // Adjust the padding as necessary
            dateLabel.Top = messageLabel.Bottom + padding;

            // Optionally adjust the height of the UserControl to fit all content
            this.Height = dateLabel.Bottom + padding;
        }



        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}

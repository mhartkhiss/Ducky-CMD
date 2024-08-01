using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ducky_CMD
{
    public partial class ToastNotification : Form
    {
        private Timer slideInTimer;
        private Timer displayTimer;
        private Timer slideOutTimer;
        private int duration = 60 * 1000; // 60 seconds
        private int slideDuration = 200; // 0.5 second for sliding in and out
        private int step = 20;

        public ToastNotification()
        {
            InitializeComponent();
            InitializeTimers();
            this.Load += ToastNotification_Load;
        }

        public ToastNotification(string ign, string dcm)
        {
            InitializeComponent();
            InitializeTimers();
            this.Load += ToastNotification_Load;
            label_ign.Text = ign;
            label_dcm.Text = dcm;
        }

        private void InitializeTimers()
        {
            slideInTimer = new Timer();
            slideInTimer.Interval = 5; // Adjust this for smoother animation
            slideInTimer.Tick += SlideInTimer_Tick;

            displayTimer = new Timer();
            displayTimer.Interval = duration;
            displayTimer.Tick += DisplayTimer_Tick;

            slideOutTimer = new Timer();
            slideOutTimer.Interval = 5;
            slideOutTimer.Tick += SlideOutTimer_Tick;
        }

        private void ToastNotification_Load(object sender, EventArgs e)
        {
            // Start position off-screen (right)
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height; // Bottom corner
            this.Left = Screen.PrimaryScreen.WorkingArea.Width; // Start off-screen to the right

            slideInTimer.Start();
        }

        private void SlideInTimer_Tick(object sender, EventArgs e)
        {
            if (this.Left > Screen.PrimaryScreen.WorkingArea.Width - this.Width)
            {
                this.Left -= step;
            }
            else
            {
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                slideInTimer.Stop();
                displayTimer.Start();
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            displayTimer.Stop();
            slideOutTimer.Start();
        }

        private void SlideOutTimer_Tick(object sender, EventArgs e)
        {
            if (this.Left < Screen.PrimaryScreen.WorkingArea.Width)
            {
                this.Left += step;
            }
            else
            {
                slideOutTimer.Stop();
                this.Close();
            }
        }

        private void ToastNotification_MouseClick(object sender, MouseEventArgs e)
        {
            slideInTimer.Stop();
            displayTimer.Stop();
            slideOutTimer.Start();
        }
    }
}

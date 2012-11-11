using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyStatus
{
    public partial class MainForm : Form
    {
        InterceptKeys KeyboardHook = new InterceptKeys();

        private bool CtrlPressed = false;
        private bool AltPressed = false;
        private bool ShiftPressed = false;
        private bool WinPressed = false;

        public MainForm()
        {
            InitializeComponent();
        }

        public void LLKeyDown(Keys key)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action<Keys>)(k => LLKeyDown(k)), new object[] { key });
                return;
            }

            switch (key)
            {
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    CtrlPressed = true;
                    UpdateScreen();
                    break;

                case Keys.Alt:
                    AltPressed = true;
                    UpdateScreen();
                    break;

                case Keys.LWin:
                case Keys.RWin:
                    WinPressed = true;
                    UpdateScreen();
                    break;

                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    ShiftPressed = true;
                    UpdateScreen();
                    break;
            }
        }

        public void LLKeyUp(Keys key)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action<Keys>)(k => LLKeyUp(k)), new object[] { key });
                return;
            }

            switch (key)
            {
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    CtrlPressed = false;
                    UpdateScreen();
                    break;

                case Keys.Alt:
                    AltPressed = false;
                    UpdateScreen();
                    break;

                case Keys.LWin:
                case Keys.RWin:
                    WinPressed = false;
                    UpdateScreen();
                    break;

                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    ShiftPressed = false;
                    UpdateScreen();
                    break;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            KeyboardHook.CreateHook((InterceptKeys.KeyPressHandler)LLKeyDown, (InterceptKeys.KeyPressHandler)LLKeyUp);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            KeyboardHook.Dispose();
        }

        private void UpdateScreen()
        {
            StatusPanel.Refresh();
        }

        private void StatusPanel_Paint(object sender, PaintEventArgs e)
        {
            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                Rectangle rect = new Rectangle();
                rect.Y = 0;
                rect.Height = StatusPanel.Size.Height;

                rect.X = (StatusPanel.Size.Width / 4) * 0;
                rect.Width = StatusPanel.Size.Width / 4;
                e.Graphics.FillRectangle(CtrlPressed ? Brushes.Green : Brushes.Red, rect);
                e.Graphics.DrawString("Ctrl", font, Brushes.Black, rect, format);

                rect.X = (StatusPanel.Size.Width / 4) * 1;
                rect.Width = StatusPanel.Size.Width / 4;
                e.Graphics.FillRectangle(AltPressed ? Brushes.Green : Brushes.Red, rect);
                e.Graphics.DrawString("Alt", font, Brushes.Black, rect, format);

                rect.X = (StatusPanel.Size.Width / 4) * 2;
                rect.Width = StatusPanel.Size.Width / 4;
                e.Graphics.FillRectangle(WinPressed ? Brushes.Green : Brushes.Red, rect);
                e.Graphics.DrawString("Win", font, Brushes.Black, rect, format);
                
                rect.X = (StatusPanel.Size.Width / 4) * 3;
                rect.Width = StatusPanel.Size.Width / 4;
                e.Graphics.FillRectangle(ShiftPressed ? Brushes.Green : Brushes.Red, rect);
                e.Graphics.DrawString("Shift", font, Brushes.Black, rect, format);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Draggable window
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {    
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        // /draggable window
    }
}

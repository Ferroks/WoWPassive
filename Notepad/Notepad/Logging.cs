using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace WiNiFiX
{
    public class Logging
    {
        //StreamWriter sw;
        RichTextBox rtbLogWindow;
        public Color ErrorColor = Color.Red;
        public string HorizontalLine = "".PadLeft(312, '-');

        public Logging(RichTextBox rtbLogWindow, Form parent = null)
        {
            //if (!Directory.Exists(Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyy-MMM")))
            //    Directory.CreateDirectory(Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyy-MMM"));

            //sw = new StreamWriter(Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyy-MMM") + "\\" + DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss") + ".txt");
            this.rtbLogWindow = rtbLogWindow;
        }

        public void LogActivity(string Activity)
        {
            LogActivity(Activity, Color.Black);
        }

        public void LogActivity(string Activity, Color c)
        {
            string Time = "[" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + "]";

            try
            {
                if (Activity == string.Empty)
                {
                    AppendTrace(HorizontalLine + "\r\n", Color.LightGray);
                    //sw.WriteLine(HorizontalLine);
                }
                else if (Activity.Trim() == string.Empty)
                {
                    AppendTrace("\r\n", c);
                    //sw.WriteLine("");
                }
                else
                {
                    AppendTrace(Time, Color.DarkGray);
                    AppendTrace(" " + Activity + "\n", c);
                    //sw.WriteLine(Time + " " + Activity);
                }

                //sw.Flush();

                Application.DoEvents();
            }
            catch (Exception execp)
            {
                LogActivity("Exception in LogActivity function\r\nError: " + execp.Message, ErrorColor);
            }
        }

        public void AppendTrace(string text, Color textcolor)
        {
            try
            {
                // trap exception which occurs when processing
                // as application shutdown is occurring

                rtbLogWindow.SelectionStart = rtbLogWindow.Text.Length;
                rtbLogWindow.SelectionLength = 0;
                rtbLogWindow.SelectionColor = textcolor;
                rtbLogWindow.AppendText(text);

                rtbLogWindow.ClearUndo();
            }
            catch (Exception execp)
            {
                if (execp.Message.Contains("System.OutOfMemoryException"))
                {
                    LogActivity("System Logging has run out of memory, clearing logging window\r\nError" + execp.Message, Color.OrangeRed);

                    rtbLogWindow.Clear();
                    rtbLogWindow.ClearUndo();
                }
                else
                {
                    LogActivity("Exception in AppendTrace function\r\nError: " + execp.Message, Color.OrangeRed);
                }
            }
        }
    }

    public static class UIExtensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
        IntPtr hWnd,
        uint Msg,
        IntPtr wParam,
        IntPtr lParam);

        private const int WM_VSCROLL = 277;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_TOP = 6;
        private const int SB_BOTTOM = 7;

        public static void ScrollToBottom(this TextBoxBase tb)
        {
            SendMessage(tb.Handle, WM_VSCROLL, new IntPtr(SB_BOTTOM), new IntPtr(0));
        }

        public static void ScrollToTop(this System.Windows.Forms.TextBoxBase tb)
        {
            SendMessage(tb.Handle, WM_VSCROLL, new IntPtr(SB_TOP), new IntPtr(0));
        }

        public static void ScrollLineDown(this System.Windows.Forms.TextBoxBase tb)
        {
            SendMessage(tb.Handle, WM_VSCROLL, new IntPtr(SB_LINEDOWN), new IntPtr(0));
        }

        public static void ScrollLineUp(this System.Windows.Forms.TextBoxBase tb)
        {
            SendMessage(tb.Handle, WM_VSCROLL, new IntPtr(SB_LINEUP), new IntPtr(0));
        }
    }
}

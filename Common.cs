using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NETRemotingDesktop
{
    public class Common : MarshalByRefObject
    {
        public const int PORT = 5536;

        public Stream GetScreenBitmapBytes()
        {
            using (var b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (var g = Graphics.FromImage(b))
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.Location, Point.Empty, Screen.PrimaryScreen.Bounds.Size);
                var s = new MemoryStream();
                b.Save(s, ImageFormat.Jpeg);
                return s;
            }
        }
        public void SetCursorPostion(float x, float y)
        {
            Cursor.Position = new Point((int)(Screen.PrimaryScreen.Bounds.Width * x), (int)(Screen.PrimaryScreen.Bounds.Height * y));
        }

        public void MouseDownUp(MouseButtons btn, bool up)
        {
            var key = (uint)btn >> 20;
            mouse_event(key << (int)(Math.Log(key, 2) + (up ? 2 : 1)), 0, 0, 0, 0);
        }

        public void KeyPress(int key, bool up)
        {
            keybd_event((byte)key, 0, up ? 2 : 0, 0);
        }

        [DllImport("user32")]
        static extern void mouse_event(uint flag, uint x, uint y, uint btn, uint extra);
        [DllImport("user32")]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, uint extra);
    }
}
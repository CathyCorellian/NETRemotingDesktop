using System;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;

namespace NETRemotingDesktop
{
    static class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(), false);
            var common = (Common)Activator.GetObject(typeof(Common), "tcp://" + args[0] + ":" + Common.PORT + "/Common");

            var form = new Form();
            form.KeyDown += (s, e) => common.KeyPress(e.KeyValue, false);
            form.KeyUp += (s, e) => common.KeyPress(e.KeyValue, true);
            form.MouseDown += (s, e) => common.MouseDownUp(e.Button, false);
            form.MouseUp += (s, e) => common.MouseDownUp(e.Button, true);
            form.MouseMove += (s, e) => common.SetCursorPostion((float)e.X / form.ClientRectangle.Width, (float)e.Y / form.ClientRectangle.Height);
            new Thread(x =>
            {
                for (; true; Thread.Sleep(300))
                    using (var m = common.GetScreenBitmapBytes())
                    using (var i = Image.FromStream(m))
                    using (var g = form.CreateGraphics())
                        form.Invoke(new MethodInvoker(delegate { g.DrawImage(i, form.ClientRectangle); }));
            }).Start();
            form.ShowDialog();
        }
    }
}
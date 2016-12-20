using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace NETRemotingDesktop
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(Common.PORT), false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Common), "Common", WellKnownObjectMode.Singleton);
            Console.ReadKey();
        }
    }
}
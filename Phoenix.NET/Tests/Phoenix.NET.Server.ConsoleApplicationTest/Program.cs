using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.NET.Server.Nodes;
using Phoenix.NET.Common;
using Phoenix.NET.ConsoleApplicationTest;

namespace Phoenix.NET.Server.ConsoleApplicationTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PerfTest();
        }

        private static void PerfTest()
        {
            string channel = "perf";

            PhoenixServer server = new PhoenixServer();
            server.Start(22000);
            Console.WriteLine("Server started");

            PhoenixLocalNode echoNode = new PhoenixLocalNode();
            echoNode.Connect(server.GetServerConfig());
            Console.WriteLine("Echo node connected");

            echoNode.OnDisposed += () =>
            {
                server.Stop();
            };

            System.Threading.Tasks.Task<IDisposableSubscription> asyncSubscription = null;
            asyncSubscription = echoNode.Subscribe(channel, (c, h) =>
            {
                if (!h.Unsubscribing)
                {
                    var echoMessage = $"ECHO: {c}";
                    echoNode.Publish(channel, echoMessage);

                    if ((c as Test)?.Data == null && (c as Test)?.Message == null)
                    {
                        var sub = asyncSubscription.Result;
                        sub.Dispose();
                    }
                }
                else
                {
                    echoNode.Dispose();
                }
            });
        }

        static void GenericContentHandler(object content, PhoenixContextHook hook)
        {
            Console.WriteLine($"Content: {content} [for {hook.TargetChannel}, on {hook.PublicationDateTime}]");
        }
    }
}

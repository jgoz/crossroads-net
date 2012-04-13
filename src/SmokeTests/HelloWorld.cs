namespace CrossroadsIO.SmokeTests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    internal class HelloWorld : ISmokeTest
    {
        public string Name
        {
            get { return "Hello World"; }
        }

        public void Run()
        {
            var client = new Thread(ClientThread);
            var server = new Thread(ServerThread);

            server.Start();
            client.Start();

            server.Join();
            client.Join();
        }

        private static void ClientThread()
        {
            Thread.Sleep(10);

            using (var context = Context.Create())
            using (var socket = context.CreateSocket(SocketType.REQ))
            {
                socket.Connect("tcp://localhost:8989");

                socket.SendFrame(new Frame(Encoding.UTF8.GetBytes("Hello")));

                var buffer = new byte[100];
                int size = socket.Receive(buffer);

                using (var stream = new MemoryStream(buffer, 0, size))
                {
                    Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
                }
            }
        }

        private static void ServerThread()
        {
            using (var context = Context.Create())
            using (var socket = context.CreateSocket(SocketType.REP))
            {
                socket.Bind("tcp://*:8989");

                Frame request = socket.ReceiveFrame();
                Console.WriteLine(Encoding.UTF8.GetString(request));

                socket.SendFrame(new Frame(Encoding.UTF8.GetBytes("World")));
            }
        }
    }
}

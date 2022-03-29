using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //the server is standalone, it needs to be started seperately.
            Console.WindowWidth = 100;
            Console.WindowHeight = 10;
            SocketServer server = new SocketServer();
        }
    }
}

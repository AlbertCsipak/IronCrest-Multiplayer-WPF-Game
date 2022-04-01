namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //the server is standalone, it will run in a cmd.
            SocketServer server = new SocketServer();
            server.Init(args[0], int.Parse(args[1]), int.Parse(args[2]), args[3]);
            server.Session(int.Parse(args[4]), int.Parse(args[1]), int.Parse(args[5]));
            ;
        }
    }
}

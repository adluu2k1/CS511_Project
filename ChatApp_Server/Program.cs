namespace ChatApp_Server
{
    internal static class Program
    {
        internal static Server_Process server = new();

        static void Main(string[] args)
        {
            server.Start();
            while (true) { }
        }
    }
}
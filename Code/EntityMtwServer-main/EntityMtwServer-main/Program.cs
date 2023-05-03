namespace EntityMtwServer 
{
    internal class Program {
        static void Main(string[] args) {
            MasterServerContext masterServerContext = new MasterServerContext();

            Console.WriteLine(masterServerContext.Users.First().Id);
            Console.ReadLine();
        }
    }
}



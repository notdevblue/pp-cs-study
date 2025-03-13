public class Program
{
    public static void Main()
    {
        System.Console.WriteLine($"Current Cozyness: {Home.Cozyness}");
    }



    public class Home
    {
        public const int Cozyness = 11;
    }
}

public class Program
{
    // public static void Main()
    // {
    //     var home = new Home();

    //     System.Console.WriteLine($"Current Cozyness: {Home.Cozyness}");
    //     System.Console.WriteLine($"Current Warmness: {home.Warmness}");
    //     System.Console.WriteLine($"Current Warmness: {Home.StaticWarmness}");
    // }

    // public class Home
    // {
    //     public const int Cozyness = 11;

    //     public readonly int Warmness = 7;
    //     public static readonly int StaticWarmness = 7;
    // }


    public static void Main()
    {
        for (int i = UsefulValues.StartValue; i <= UsefulValues.EndValue; ++i)
        {
            System.Console.WriteLine($"Very Useful value: {i}");
        }
    }
}

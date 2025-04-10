public class MyAwesomeClass : MyClass
{
    public void Today()
    {
        // base.Today();
        System.Console.WriteLine("Payday");
    }
}

public class MyWonderfulClass : MyClass
{
    public new void Today()
    {
        System.Console.WriteLine("Holiday");
    }
}

public class Program
{
    public unsafe static void Main()
    {
        var a = new MyAwesomeClass();
        a.Today();

        var b = new MyWonderfulClass();
        b.Today();
    }
}

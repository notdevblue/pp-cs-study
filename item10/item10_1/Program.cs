public class MyClass
{
    public void Today()
    {
        System.Console.WriteLine("Friday");
    }
}

public class MyAnotherClass : MyClass
{
    public new void Today()
    {
        System.Console.WriteLine("Monday");
    }
}

public class Program
{
    public static object MakeObject() => new MyAnotherClass();


    public unsafe static void Main()
    {
        object c = MakeObject();

        MyClass cl1 = (MyClass)c;
        MyAnotherClass cl2 = (MyAnotherClass)c;
        
        cl1.Today();
        cl2.Today();
    }
}

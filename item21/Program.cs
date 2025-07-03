public static class Program
{
    public class DisposableClass : IDisposable
    {
        public void Dispose()
        {
            System.Console.WriteLine("Disposed");
        }
    }

    public static void GetThingsDone<T>() where T : new()
    {
        T newT = new T();
        using (newT as IDisposable)
        {
            // newT.blahblah...
        }
    }

    public sealed class DisposableClass2<T> : IDisposable where T : new()
    {
        private Lazy<T> _myT = new Lazy<T>(() => new T());

        public void DoSomeWork()
        {
            System.Console.WriteLine($"DoSomeWork({_myT.Value})");
        }

        public void Dispose()
        {
            System.Console.WriteLine("Disposed2");

            if (_myT.IsValueCreated)
            {
                var obj = _myT.Value as IDisposable;
                obj?.Dispose();
            }
        }
    }

    public class MyClass<T>
    {
        public T _myT = default!;

        public MyClass(T myT)
        {
            _myT = myT;
        }

        public void DoSomeWork()
        {
            System.Console.WriteLine($"DoSomeWork({_myT})");
        }
    }

    public static void Main()
    {
        GetThingsDone<DisposableClass>(); // IDisposable 구현
        GetThingsDone<int>(); // 없음

        using var disposableClass = new DisposableClass2<DisposableClass>();
        disposableClass.DoSomeWork();

        using (var c = new DisposableClass())
        {
            var myClass = new MyClass<DisposableClass>(c);
            myClass.DoSomeWork();
        }
    }
}
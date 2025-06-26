/* 구현
public class MyComparableClass : IComparable<MyComparableClass>, IComparable
{
    public int Value { get; set; } = 0;

    public MyComparableClass(int inValue)
    {
        Value = inValue;
    }


    public int CompareTo(MyComparableClass? other) // 최신 .NET API들은 대체로 IComparable<T> 사용
    {
        ArgumentNullException.ThrowIfNull(other);

        return this.Value - other.Value;
    }

    public int CompareTo(object? obj) // 오래된 API들은 IComparable 사용함. (.NET 2.0 이전 코드들)
    {
        ArgumentNullException.ThrowIfNull(obj);
        if (obj is not MyComparableClass)
        {
            throw new ArgumentException();
        }

        var convertedObj = (MyComparableClass)obj;

        return this.Value - convertedObj.Value;
    }
}

public class Program
{
    public static void Main()
    {
        var c1 = new MyComparableClass(1);
        var c2 = new MyComparableClass(54);
        var res = c1.CompareTo(c2);
        if (res > 0)
        {
            System.Console.WriteLine("더 큼");
        }
        if (res == 0)
        {
            System.Console.WriteLine("같음");
        }
        if (res < 0)
        {
            System.Console.WriteLine("더 작음");
        }
    }
}
//*/

/*
public class MyComparableClass : IComparable<MyComparableClass>
{
    public int CompareTo(MyComparableClass? other)
    {
        return 0;
    }
}

public class MyOtherComparableClass : IComparable<MyOtherComparableClass>
{
    public int CompareTo(MyOtherComparableClass? other)
    {
        return 0;
    }
}

public class Program
{
    public static void Main()
    {
        var myCmp = new MyComparableClass();
        var myOtherCmp = new MyOtherComparableClass();

        // myCmp.CompareTo(myOtherCmp); 컴파일 에러
        ((IComparable)myCmp).CompareTo(myOtherCmp); // 런타임 에러 발생함
        
    }
}
//*/

/*
public class MyComparableClass : IComparable<MyComparableClass>
{
    public int Value { get; set; } = 0;

    public MyComparableClass(int inValue)
    {
        Value = inValue;
    }


    public int CompareTo(MyComparableClass? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return this.Value - other.Value;
    }

    public static bool operator <(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) < 0;
    public static bool operator >(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) > 0;
    public static bool operator <=(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) <= 0;
    public static bool operator >=(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) >= 0;
}

public class Program
{
    public static void Main()
    {
        var c1 = new MyComparableClass(1);
        var c2 = new MyComparableClass(54);
        if (c1 > c2)
        {
            System.Console.WriteLine("더 큼");
        }
        if (c1 < c2)
        {
            System.Console.WriteLine("더 작음");
        }
    }
}
//*/

/*
public class MyComparableClass : IComparable<MyComparableClass>
{
    public int Value { get; set; } = 0;

    public int OtherValue { get; set; } = 0;

    public MyComparableClass(int inValue, int inOtherValue)
    {
        Value = inValue;
        OtherValue = inOtherValue;
    }


    public int CompareTo(MyComparableClass? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return this.Value - other.Value;
    }

    public static Comparison<MyComparableClass> CompareByOtherValue => (left, right) => left.OtherValue.CompareTo(right.OtherValue);
}

public class Program
{
    public static void Main()
    {
        var c1 = new MyComparableClass(1, 10);
        var c2 = new MyComparableClass(54, 20);

        List<MyComparableClass> list =
        [
            new MyComparableClass(1, 10),
            new MyComparableClass(1, 25),
            new MyComparableClass(1, 21),
            new MyComparableClass(1, 35),
            new MyComparableClass(1, 12),
            new MyComparableClass(1, 13),
            new MyComparableClass(1, 15),
            new MyComparableClass(1, 16),
        ];

        list.Sort(MyComparableClass.CompareByOtherValue);

        var joinedList = String.Join(", ", list.Select(x => x.OtherValue));
        
        System.Console.WriteLine(joinedList);
        
        
    }
}
//*/

/*
using System.Collections;

public class MyComparableClass : IComparable
{
    public int Value { get; set; } = 0;

    public int OtherValue { get; set; } = 0;

    public MyComparableClass(int inValue, int inOtherValue)
    {
        Value = inValue;
        OtherValue = inOtherValue;
    }


    public int CompareTo(MyComparableClass? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return this.Value - other.Value;
    }

    public int CompareTo(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        if (obj is not MyComparableClass other)
        {
            throw new ArgumentException("Object is not a MyComparableClass");
        }

        return this.Value - other.Value;
    }

    private static OtherValueComparer _otherValueComparer = new OtherValueComparer();
    public static int CompareOtherValue(object? a, object? b) => _otherValueComparer.Compare(a, b);

    private class OtherValueComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            if (x is not MyComparableClass a || y is not MyComparableClass b)
            {
                throw new ArgumentException("Both arguments must be MyComparableClass");
            }

            return a.OtherValue - b.OtherValue;
        }
    }
}

public class Program
{
    public static void Main()
    {
        var c1 = new MyComparableClass(1, 10);
        var c2 = new MyComparableClass(54, 20);

        var res = MyComparableClass.CompareOtherValue(c1, c2);
        if (res > 0)
        {
            System.Console.WriteLine("더 큼");
        }
        if (res == 0)
        {
            System.Console.WriteLine("같음");
        }
        if (res < 0)
        {
            System.Console.WriteLine("더 작음");
        }
        
        
    }
}
//*/
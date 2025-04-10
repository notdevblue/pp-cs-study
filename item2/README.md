### Const 관한 IL

![image](https://github.com/user-attachments/assets/f819fa45-3efd-412f-9056-0926cc78543c)


원본:
```csharp
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

```

IL:
```csharp
// item2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;

public class Program
{
	public class Home
	{
		public const int Cozyness = 11;
	}

	public static void Main()
	{
		Console.WriteLine($"Current Cozyness: {11}"); // Home.Cozyness
	}
}
```


Readonly IL:
```csharp
// item2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;

public class Program
{
	public class Home
	{
		public const int Cozyness = 11;

		public readonly int Warmness = 7;
	}

	public static void Main()
	{
		Home home = new Home();
		Console.WriteLine($"Current Cozyness: {11}");
		Console.WriteLine($"Current Warmness: {home.Warmness}");
	}
}
```

Static readonly IL:
```csharp
// item2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;

public class Program
{
	public class Home
	{
		public const int Cozyness = 11;

		public readonly int Warmness = 7;

		public static readonly int StaticWarmness = 7;
	}

	public static void Main()
	{
		Home home = new Home();
		Console.WriteLine($"Current Cozyness: {11}");
		Console.WriteLine($"Current Warmness: {home.Warmness}");
		Console.WriteLine($"Current Warmness: {Home.StaticWarmness}");
	}
}

```

---

### dll 참조시 

기본:

![image](https://github.com/user-attachments/assets/60371529-cfb3-4d09-8548-519ea805814f)

UsefulValues 만 새로 빌드:

![image](https://github.com/user-attachments/assets/0e0c1804-1d71-4d46-93c1-ae11ffded575)

IL:
```csharp
// item2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;

public class Program
{
	public static void Main()
	{
		for (int i = UsefulValues.StartValue; i <= 15; i++)
		{
			Console.WriteLine($"Very Useful value: {i}");
		}
	}
}
```

전부 새로 빌드 IL:
```csharp
// item2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;

public class Program
{
	public static void Main()
	{
		for (int i = UsefulValues.StartValue; i <= 25; i++)
		{
			Console.WriteLine($"Very Useful value: {i}");
		}
	}
}
```

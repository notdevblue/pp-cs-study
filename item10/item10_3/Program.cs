using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public class Han
{
    // public bool isNeedCoffee = true;
    // public int age = 22;

    // public void DrinkCoffee()
    // {
    //     System.Console.WriteLine("후르륵");
    // }

    public virtual void EatPizza()
    {
        System.Console.WriteLine("옴뇸뇸");
    }
}

public class Han2 : Han
{
    public override void EatPizza()
    {
        System.Console.WriteLine("뮥뮥뭉");
    }
}

delegate void VoidMethodDelegate(IntPtr @this);

public class Program
{
    public unsafe static void Main()
    {
        // var drinkCoffee = typeof(Han).GetMethod("DrinkCoffee")!;
        // RuntimeHelpers.PrepareMethod(drinkCoffee.MethodHandle);
        // IntPtr ptr = drinkCoffee.MethodHandle.GetFunctionPointer();
        
        // delegate*<void> func = (delegate*<void>)ptr;
        // System.Console.WriteLine(ptr.ToString("x"));
        // func();
        // c++ 이었다면 &han + offset 됬지만.. C#에선 clr이 관리함ㅠㅠ

        var han = new Han();
        var handle = GCHandle.Alloc(han, GCHandleType.Pinned);
        byte* ptrHan = (byte*)handle.AddrOfPinnedObject(); // Object Header, Method Table Pointer 뒤 isNeedCoffee 부분
        // bool* ptrBool = (bool*)(ptrHan + 0x00);
        // int* ptrInt = (int*)(ptrHan + 0x04);
        // // Console.WriteLine($"isNeedCoffee offset: {Marshal.OffsetOf<Han>("isNeedCoffee")}");
        // // Console.WriteLine($"age offset: {Marshal.OffsetOf<Han>("age")}");

        // System.Console.WriteLine(*ptrBool);
        // System.Console.WriteLine(*ptrInt);

        byte* ptrRealHan = ptrHan - 0x10;
        IntPtr* ptrMethodTable = (IntPtr*)(ptrRealHan + 0x08);
        IntPtr vtable = *ptrMethodTable;

        var han2 = new Han2();
        var handle2 = GCHandle.Alloc(han2, GCHandleType.Pinned);
        byte* ptrRealHan2 = (byte*)handle2.AddrOfPinnedObject() - 0x10;

        var method = typeof(Han).GetMethod("EatPizza")!;
        RuntimeHelpers.PrepareMethod(method.MethodHandle);

        IntPtr ptrFunc = method.MethodHandle.GetFunctionPointer();

        // System.Console.WriteLine(vtable.ToString("x"));
        // System.Console.WriteLine(((IntPtr*)vtable)[0].ToString("x"));

        // IntPtr ptrFromMethod = method.MethodHandle.GetFunctionPointer();
        // Console.WriteLine($"EatPizza ptr = 0x{ptrFromMethod.ToString("x")}");
        // for (int i = 0; i < 10; i++)
        // {
        //     var funcPtr = ((IntPtr*)vtable)[i];
        //     Console.WriteLine($"vtable[{i}] = 0x{funcPtr.ToString("x")}");
        // }

        // delegate*<IntPtr, void> vfunc = (delegate*<IntPtr, void>)((IntPtr*)vtable)[4];
        // // var vfunc = (VoidMethodDelegate)Marshal.GetDelegateForFunctionPointer(((IntPtr*)vtable)[0], typeof(VoidMethodDelegate));
        // vfunc((IntPtr)ptrRealHan);

        // Virtual stub dispatch 덕분에 직접 타고 호출하기..
        // vtable -> stub -> Han::EatPizza

        delegate*<IntPtr, void> eatPizza = (delegate*<IntPtr, void>)ptrFunc;
        eatPizza((IntPtr)ptrRealHan);
        eatPizza((IntPtr)ptrRealHan2);

        handle.Free();
    }
}

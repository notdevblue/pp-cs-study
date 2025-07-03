# 타입 매개변수가 `IDisposable`을 구현한 경우를 대비하여 제네릭 클래스를 작성하라

제약 조건은 두가지 역할을 함
* 란타임 오류 발생 가능성 있는 부분을 컴파일 타임 오류로 대체
* 타입 매개변수로 사용할 수 있는 타입을 명확히 규정하여 사용자에게 도움을 줌

하지만 무엇을 할 수 있는지만 규정하고, 무엇을 해선 안 되는지 정의 불가능.

대부분은 상관없지만, 타임 배개변수로 지정하는 타입이 `IDisposable`을 구현하고 있다면 특별한 추가 작업이 반드시 필요

```csharp
public void GetThingsDone<T>() where T : new()
{
    T newT = new T();
}
```
`T`가 `IDisposable` 구현한 타입인 경우 리소스 누수 발생 가능.

`T` 타입으로 지역변수를 생성할 때마다 `T`가 `IDisposable`을 구현하고 있는지 확인해야 하고, `IDisposable`을 구현하고 있다면 추가적인 처리를 해야 함.

## 함수 안에서 생성한 경우

```csharp
public void GetThingsDone<T>() where T : new()
{
    T newT = new T();
    using (newT as IDisposable)
    {
        // newT.blahblah...
    }
}
```

이처럼 코드를 작성하면 컴파일러는 `IDisposable`로 형변환된 객체를 저장하기 위해 숨겨진 지역변수를 생성함. `T`가 `IDisposable`을 구현하지 않았다면 이 지역변수는 `null`이 됨.

C# 컴파일러는 이 지역변수의 값이 `null`인지 검사한 후:
* `null`이라면 `Dispose()` 호출하지 않음.
* `null`이 아니라면 `Dispose()` 호출함.

## 타입 매개변수로 전달한 타입으로 맴버 변수를 선언한 경우

타입 매개변수로 전달한 타입을 이용하여 맴버 변수를 선언한 경우, `IDisposable`을 구현했을 가능성이 있는 타입으로 맴버 변수를 선언한 것이기 때문에 제네릭 클래스에서 `IDisposable` 구현하여 해당 리소스를 처리함.

```csharp
public class DisposableClass2<T> : IDisposable where T : new()
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
```

`IDisposable` 인터페이스를 구현하고, 클래스에 `sealed` 를 추가함.

파생 클래스를 만들 가능성이 있는 타입이라면 표준 `Dispose` 패턴 전체를 구현해야 함.

## `new()` 제약조건 제거

`Dispose`의 호출 책임을 제네릭 클래스 외부로 전담시키고, 객체의 소유권을 제네릭 클래스 외부로 옮기면 `new()` 제약조건 제거 가능.

```csharp
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
```

~~에초에 처음부터 이러면 되는거 아닌가...~~

## 결론

* 제네릭 클래스의 타입 매개변수로 객체를 생성하는 경우, `IDisposable`을 구현하고 있는지 확인 필요.
* 에초에 이런 코드가 있으면 안 되는거 아닌가...


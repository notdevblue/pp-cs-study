# 타입 매개변수가 `IDisposable`을 구현한 경우를 대비하여 제네릭 클래스를 작성하라

제약 조건은 두가지 역할을 함
* 란타임 오류 발생 가능성 있는 부분을 컴파일 타임 오류로 대체
* 타입 매개변수로 사용할 수 있는 타입을 명확히 규정하여 사용자에게 도움을 줌

하지만 무엇을 할 수 있는지만 규정하고, 무엇을 해선 안 되는지 정의 불가능.

대부분은 상관없지만, 타임 배개변수로 지정하는 타입이 `IDisposable`을 구현하고 있다면 특별한 추가 작업이 반드시 필요

```csharp
public void GetThingsDone<T>() : where T : new()
{
    T newT = new T();
}
```
`T`가 `IDisposable` 구현한 타입인 경우 리소스 누수 발생 가능.

`T` 타입으로 지역변수를 생성할 때마다 `T`가 `IDisposable`을 구현하고 있는지 확인해야 하고, `IDisposable`을 구현하고 있다면 추가적인 처리를 해야 함.

## 함수 안에서 생성한 경우

```csharp
public void GetThingsDone<T>() : where T : new()
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

## 맴버 변수를 선언한 경우



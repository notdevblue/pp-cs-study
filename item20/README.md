# 아이템 20: `IComparable<T>`와 `IComparer<T>`를 이용하여 객체의 선후 관계를 정의하라

컬랙션 정렬하거나 검색하려면 객체 선후 관계 판단 가능한 기능을 정의해야 함.

.NET Framework 에선 `IComparable<T>`와 `IComparer<T>` 를 제공함.
* `IComparable<T>`: 기본적인 선후 관계 정의
* `IComparer<T>`: 추가적인 선후 관계 정의

더불어 타입 내 관계 연산자 `<`, `>`, `<=`, `>=` 를 재정의하면 최적화된 방식으로 객체 선후 관계 판단 가능하여, 기본 관계 연산자 구현 기능 이용할 떄 발생 가능한 비효율 문제 개선 가능.

## `IComparable<T>`, `IComparable` 인터페이스

`IComparable` 인터페이스는 `CompareTo()` 하나의 메서드만이 정의되어 있음.

C 라이브러리의 strcmp 함수 구현 방식을 따름.
* 현재 객체가 대상 객체보다 작음: 0보다 작은 값
* 현재 객체가 대상 객체보다 같음: 0
* 현재 객체가 대상 객체보다 큼:   0보다 큰 값
```c
int strcmp(const char* s1, const char* s2)
{
    while(*s1 && (*s1 == *s2))
    {
        s1++;
        s2++;
    }
    return *(const unsigned char*)s1 - *(const unsigned char*)s2;
}
```

```csharp
public int CompareTo<T>(T? other) // 최신 .NET API들은 대체로 IComparable<T> 사용
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

    var convertedObj = (MyComparableClass)obj; // 인수 타입 object 라서 변환 해야함

    return this.Value - convertedObj.Value;
}
```

타입 배개변수를 취하지 않는 `IComparable`는 상당히 많은 단점이 있음.
1. `CompareTo()` 에 올바르지 못한 객체 전달하는 경우 아무런 방비가 없음.
2. 비교 위해 박싱/언박싱 필요하므로 매 호출 마다 상당한 비용 발생 (struct 받는 경우)
    1. `CompareTo(1)` <- 전달 시 박싱
    2. `var convertedObj (int)obj)` <- 비교할때 언박싱

제네릭 버전이 아닌 `IComparable` 구현해야 하는 이유
* .NET Framework 2.0 이전 개발 코드에서 사용하는 경우
* 일부 Base class library(윈폼, ASP.NET 웹폼) 에서 .NET Framework 1.0 하위 호환 요구

### `IComparable<T>` 통해 안전한 접근 가능

`IComparable.CompareTo)()` 와 같이 명시적인 방법으로 구현했기 때문에 IComparable 타입 참조 통해서만 메서드 호출 가능.
안전하지 않은 `CompareTo()` 메서드에는 접근 불가능.

```csharp
// IComparable<T> 를 상속받은 서로 다른 두 클래스
Linux pc1;
Windows pc2;

pc1.CompareTo(pc2); // 컴파일 에러
((IComparable)pc1).CompareTo(pc2); // 런타임 에러
```

### 강력한 타입의 오버로드 메서드 구현

`IComparable` 구현할 땐 추가적으로 강력한 타입의 public 오버로드 메서드도 구현해야 함.

강력한 타입의 오버로드 메서드를 사용하면 더 빠르게 비교 연산 수행 가능하며, `CompareTo()` 메서드 오용 가능성도 줄일 수 있음.

```csharp
public class MyComparableClass : IComparable<MyComparableClass>
{
    // ...

    public static bool operator <(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) < 0;
    public static bool operator >(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) > 0;
    public static bool operator <=(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) <= 0;
    public static bool operator >=(MyComparableClass left, MyComparableClass right) => left.CompareTo(right) >= 0;
}
```

## `IComparer<T>` 인터페이스

.NET Framework에 제네릭 기능이 포함된 이후 개발된 대부분의 API는 정렬이 필요한 경우, `Comparison<T>` 델리게이트 작업 위임하도록 작성됨.

```csharp
public static Comparison<MyComparableClass> CompareByOtherValue => (left, right) => left.OtherValue.CompareTo(right.OtherValue);
```

오래된 라이브러리는 `Comparison<T>` 와 유사한 기능을 `IComparer` 인터페이스 통해 제공함.

`IComparer` 는 제네릭 사용하지 않고 `IComparable` 에서 정의한 선후 관계 외 추가적인 선후 관계 논리 제공해야 하는 경우 위해 표준화된 방법.

.NET Framework 1.x 버전 클래스 라이브러리 메소드 중 `IComparable` 인터페이스 매개변수 받는 모든 메서드들은 `IComparer` 매개변수로 받는 오버로드 메서드도 제공함.

```csharp
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
```

## 결론

* 객체간 비교 자주 한다면 `IComparable<T>` 구현하면 되나?
* `IComparable<T>` 를 구현하는 경우 `<`, `>`, `<=`, `>=` 요친구들도 같이 구현?

# 아이템 28: 확장 메서드를 이용하여 구체화된 제네릭 타입을 개선하라

개발하다 보면 `List<DtoPoint>`, `Dictionary<Type, string>` 처럼 제네릭 컬랙션에 타입 매개변수 지정하여 사용하게 됨.

컬랙션 사용하는 이유는 특정 타입의 집합을 다루거나, 컬랙션이 제공하는 고유의 기능을 사용하기 위함일 것.

기존 컬랙션 타입에 영향을 주지 않으면서 새로운 기능을 추가하고 싶다면 구체화된 컬렉션 타입에 대해 확장 메서드를 작성하면 됨.

## 특정 타입 매개변수용 메서드

아이탬 27에선 `IEnumerable<T>` 에 공통 적용 가능한 메서드 구현한 예를 살펴봄, 그 외에도 `IEnumerable<T>` 의 타입 매개변수로 특정 타입이 전달되었을 때 사용되는 메서드들도 있음.

구채적으로는 `IEnumerable<int>`, `IEnumerable<double>`, `IEnumerable<long>`, `IEnumerable<float>` 등.. 타입 매개변수 지정된 경우에만 사용되는 특화된 메서드가 있음.

```cs
public static class Enumerable
{
    public static int Average(this IEnumerable<int> sequence);
    public static int Max(this IEnumerable<int> sequence);
    public static int Min(this IEnumerable<int> sequence);
    public static int Sum(this IEnumerable<int> sequence);
}
```

이 패턴은 타입 매개변수로 특정 타입이 주어질 때, 해당 타이벵 대해 가장 효과적으로 동작하도록 코드를 분리하여 구현하는 방법임.

## 예시

`Dictionary.Inc()` 같은거 필요한경우:

```cs
// 그런데 놀랍게도 Dict.TryAdd 가 존재하는군요..
public static void Inc<TKey>(this Dictionary<TKey, int> self, TKey inKey, int inAmount) where TKey : notnull
{
    if (self.ContainsKey(inKey))
    {
        self[inKey] += inAmount;
    }
    else
    {
        self.Add(inKey, inAmount);
    }
}
```

그냥 10개의 10을 가진 `IEnumerable<int>` 반환을 원하는 경우:
```cs
public static IEnumerable<int> JustGiveMe10Tens(this IEnumerable<int> self) =>
    [10, 10, 10, 10, 10, 10, 10, 10, 10, 10];
```

---

확장 메서드를 사용하지 않았다면 구체화된 제네릭 타입을 상속하여 새로운 타입을 만들어야 함.

```cs
public class MySomeList : List<int>
{
    public IEnumerable<int> JustGiveMe10Tens() =>
        [10, 10, 10, 10, 10, 10, 10, 10, 10, 10];
}
```

이렇게 써도 문제는 없지만, `IEnumerable<int>` 에 대해 확장 메서드를 구현한 것에 비해 제약이 훨씬 많아짐.

확장 메서드는 `IEnumerable<int>` 를 기반으로 작성되었지만, 파생 클래스는 `List<int>` 를 기반으로 함.
새롭게 구현한 `MySomeList` 는 더 이상 이터레이터 메서드들을 사용할 수가 없음. (*아이탬 31: 시퀀스에 사용할 수 있는 조합 가능한 API를 작성해라 참조*)

## 결론

구체화된 제네릭 타입 상속하여 메서드 추가해서 쓰는것 보단 확장 메서드를 만들어 사용.
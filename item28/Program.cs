using System.Text.Json;

public static class CollectionExtensions
{
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

    public static IEnumerable<int> JustGiveMe10Tens(this IEnumerable<int> self) =>
        [10, 10, 10, 10, 10, 10, 10, 10, 10, 10];

    public static IEnumerable<int> JustGiveMeOddNumber(this IEnumerable<int> self)
    {
        IEnumerable<int> result =
            from item in self
            where item % 2 == 0
            select item;

        return result;
    }
}

public class MySomeList : List<int>
{
    public IEnumerable<int> JustGiveMe10Tens() =>
        [10, 10, 10, 10, 10, 10, 10, 10, 10, 10];
}

public static class Program
{
    public static void Main()
    {
        var list = new List<int>()
        {
            14,512,235,1625,1234,4326,5,6,12,3,54,1234,5,123,41,236,452,6,435,63,45,72,346,4,256
        };

        var listAverageNum = list.Average();
        var listMaxNum = list.Max();
        var listMinNum = list.Min();
        var listSumNum = list.Sum();
        System.Console.WriteLine("Avg:" + listAverageNum);
        System.Console.WriteLine("Max:" + listMaxNum);
        System.Console.WriteLine("Min:" + listMinNum);
        System.Console.WriteLine("Sum:" + listSumNum);

        var dict = new Dictionary<string, int>();

        dict.Inc("wa!", 10);
        System.Console.WriteLine(dict["wa!"]);
        dict.Inc("wa!", 20);
        System.Console.WriteLine(dict["wa!"]);

        var mylist = new List<int>();
        var newList = mylist.JustGiveMe10Tens();
        var jsonList = JsonSerializer.Serialize(newList);
        System.Console.WriteLine(jsonList);

        var myResList = list.JustGiveMeOddNumber();
        var jsonResList = JsonSerializer.Serialize(myResList);
        System.Console.WriteLine(jsonResList);

    }
}
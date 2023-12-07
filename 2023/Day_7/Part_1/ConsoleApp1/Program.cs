using Hand = string;
using Bid = int;
using Card = char;
using System.Reflection.Metadata;

var data = File.ReadAllLines(@"../../../input.txt").Select(HandWithBid);
var ordered = data.Order(new HandComparer()).ToList();
foreach (var line in data)
    Console.WriteLine($"{line}: {Type(line.Item1)}");

Console.WriteLine();
foreach (var line in ordered)
    Console.WriteLine($"{line}: {Type(line.Item1)}");

var result = 0;
for (int i = 0; i < ordered.Count; i++)
    result += (i + 1) * ordered[i].Bid;
Console.WriteLine(result);


HandType Type(Hand hand)
{
    var cardCounts = new Dictionary<Card, int>();
    foreach (Card card in hand)
    {
        if (cardCounts.ContainsKey(card)) continue;
        cardCounts.Add(card, hand.Count(c2 => c2 == card));
    }
    var ordered = cardCounts.OrderBy(x => -x.Value).ToArray();

    return ordered[0].Value switch
    {
        5 => HandType.FiveOfAKind,
        4 => HandType.FourOfAKind,
        3 => ordered[1].Value == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
        2 => ordered[1].Value == 2 ? HandType.TwoPair : HandType.OnePair,
        1 => HandType.HighCard,
        _ => throw new ArgumentException(),
    };
}

int CompareHands(Hand hand1, Hand hand2)
{
    // compare type
    if (Type(hand1).CompareTo(hand2) != 0)
    {
        return Type(hand1).CompareTo(hand2);
    }

    // compare cards
    int i = -1;
    while (++i < 5 && CompareCards(hand1[i], hand2[i]) == 0);

    return CompareCards(hand1[i], hand2[i]);
}
int CompareCards(Card c1, Card c2)
{
    string order = "AKQJT98765432";
    return order.IndexOf(c1).CompareTo(order.IndexOf(c2));
}

(Hand Hand, Bid Bid) HandWithBid(string input)
{
    var strs = input.Split(' ');
    return (strs[0], int.Parse(strs[1]));
}

enum HandType
{
    HighCard,           // 23456
    OnePair,            // A23A4
    TwoPair,            // 23432
    ThreeOfAKind,       // TTT98
    FullHouse,          // 23332
    FourOfAKind,        // AA8AA
    FiveOfAKind         // AAAAA
}
class HandComparer : IComparer<(Hand Hand, Bid Bid)>
{
    public int Compare((Hand Hand, Bid Bid) x, (Hand Hand, Bid Bid) y)
    {
        // compare type
        int typeComp = Type(x.Hand).CompareTo(Type(y.Hand));
        if (typeComp != 0) return typeComp;

        // compare cards
        int i = -1;
        while (++i < 4 && CompareCards(x.Hand[i], y.Hand[i]) == 0) ;
        return CompareCards(x.Hand[i], y.Hand[i]);
    }

    HandType Type(Hand hand)
    {
        var cardCounts = new Dictionary<Card, int>();
        foreach (Card card in hand)
        {
            if (cardCounts.ContainsKey(card)) continue;
            cardCounts.Add(card, hand.Count(c2 => c2 == card));
        }
        var ordered = cardCounts.OrderBy(x => -x.Value).ToArray();

        return ordered[0].Value switch
        {
            5 => HandType.FiveOfAKind,
            4 => HandType.FourOfAKind,
            3 => ordered[1].Value == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
            2 => ordered[1].Value == 2 ? HandType.TwoPair : HandType.OnePair,
            1 => HandType.HighCard,
            _ => throw new ArgumentException(),
        };
    }

    int CompareCards(Card c1, Card c2)
    {
        string order = "AKQJT98765432";
        return -order.IndexOf(c1).CompareTo(order.IndexOf(c2));
    }
}
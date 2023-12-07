using Hand = string;
using Bid = int;
using Card = char;
using System.Reflection.Metadata;

var data = File.ReadAllLines(@"../../../input.txt").Select(x => (Hand: x.Split(' ')[0], Bid: int.Parse(x.Split(' ')[1])));
var ordered = data.Order(new HandComparer()).ToList();

var result = 0;
for (int i = 0; i < ordered.Count; i++)
    result += (i + 1) * ordered[i].Bid;

Console.WriteLine(result);


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
        int typeComp = GetType(x.Hand).CompareTo(GetType(y.Hand));
        if (typeComp != 0) return typeComp;

        // compare cards
        var ccomp = new CardComparer();
        int i = -1;
        while (++i < 4 && ccomp.Compare(x.Hand[i], y.Hand[i]) == 0);
        return ccomp.Compare(x.Hand[i], y.Hand[i]);
    }

    public static HandType GetType(in Hand hand)
    {
        var cardCounts = new Dictionary<Card, int>();
        foreach (Card card in hand)
        {
            if (cardCounts.ContainsKey(card)) continue;
            cardCounts.Add(card, hand.Count(c2 => c2 == card));
        }

        // handle jokers
        if (cardCounts.ContainsKey('J') && cardCounts['J'] < 5)
        {
            var numJokers = cardCounts['J'];
            cardCounts.Remove('J');
            var max = cardCounts.MaxBy(x => x.Value);
            cardCounts[max.Key] += numJokers;
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
}

class CardComparer : IComparer<Card>
{
    public int Compare(Card x, Card y)
    {
        string order = "J23456789TJQKA";
        return order.IndexOf(x).CompareTo(order.IndexOf(y));
    }
}
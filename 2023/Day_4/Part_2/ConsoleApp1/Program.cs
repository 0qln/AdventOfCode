using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Card = (System.Collections.Generic.List<int> WinningNumbers, System.Collections.Generic.List<int> AquiredNumbers);


var data = @"";


var reNums = new Regex(@"[0-9]+");
var reWins = new Regex(@"(?<=: ).+(?=\|)");
var reGets = new Regex(@"(?<=\|).+");

var Value = (Match x) => x.Value;
var wins = reWins.Matches(data).Select(x => reNums.Matches(x.Value).Select(Value).Select(int.Parse).ToList());
var gets = reGets.Matches(data).Select(x => reNums.Matches(x.Value).Select(Value).Select(int.Parse).ToList());
List<Card> cards = wins.Zip(gets).ToList();


int CountCards(List<Card> cards)
{
    var totalCards = new int[cards.Count];
    Array.Fill(totalCards, 1);

    for (int i = 0; i < cards.Count; i++)
    {
        var matchC = cards[i].AquiredNumbers.Count(x => cards[i].WinningNumbers.Contains(x));
        
        for (int j = i + 1; j <= i + cards[i].WinningNumbers.Intersect(cards[i].AquiredNumbers).Count(); j++)
        {
            totalCards[j] += totalCards[i];
        }
    }

    return totalCards.Sum();
}

Console.WriteLine(CountCards(cards));

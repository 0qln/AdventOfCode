

using System.Text.RegularExpressions;

var data = @"";

var reNums = new Regex(@"[0-9]+");
var reWins = new Regex(@"(?<=: ).+(?=\|)");
var reGets = new Regex(@"(?<=\|).+");

var wins = reWins.Matches(data).Select(x => reNums.Matches(x.Value).Select(x => int.Parse(x.Value)).ToList()).ToList();
var gets = reGets.Matches(data).Select(x => reNums.Matches(x.Value).Select(x => int.Parse(x.Value)).ToList()).ToList();

var result = 0.0;
for (int i = 0; i < wins.Count; i++)
{
    var win = 0;
    for (int j = 0; j < wins[i].Count; j++)
    {
        if (gets[i].Contains(wins[i][j]))
        {
            win++;
        }
    }
    result += win != 0 ? Math.Pow(2, win-1) : 0;
}

Console.WriteLine(result);

using System.Text.RegularExpressions;
using Position = (int X, int Y);


var data = @"";

var lineWidth = new Regex(@"\n").Match(data).Index + 1/*newline symbol*/;
var numbers = new Regex(@"([0-9]+)").Matches(data);
var symbols = new Regex(@"[^0-9.\n]").Matches(data);

var sum = 0;

for (int i = 0; i < numbers.Count; i++)
{
    if (symbols.Any(s => Touches(numbers[i], s)))
    {
        Console.WriteLine(numbers[i]);
        sum += int.Parse(numbers[i].Value);
    }
}

Console.WriteLine(sum);

bool Touches(Match number, Match symbol)
{
    Position symPos = (symbol.Index % lineWidth, symbol.Index / lineWidth);
    Position numPos = (number.Index % lineWidth, number.Index / lineWidth);
    var numWidth = number.Length;

    bool possibleY = Math.Abs(symPos.Y - numPos.Y) <= 1;
    bool possibleX = false;
    while (!(possibleX = symPos.X - numPos.X - numWidth == 0) && numWidth-- >= 0) ;

    return possibleX && possibleY;
}

using System.Text.RegularExpressions;
using Position = (int X, int Y);


var data = @"";


var lineWidth = new Regex(@"\n").Match(data).Index + 1/*newline symbol*/;
var numbers = new Regex(@"([0-9]+)").Matches(data);
var symbols = new Regex(@"[^0-9.\n]").Matches(data);


var result = 0;

foreach (Match symbol in symbols)
{
    if (TouchesTwo(symbol, out List<Match> nums))
    {
        result += GearRatio(nums[0].Value, nums[1].Value);
    }
}

Console.WriteLine(result);


bool TouchesTwo(Match symbol, out List<Match> nums)
{
    nums = new List<Match>();
    foreach (Match num in numbers)
    {
        if (Touches(num, symbol))
        {
            nums.Add(num);
        }
    }
    return nums.Count == 2;
}
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

int GearRatio(string x, string y) => int.Parse(x) * int.Parse(y);


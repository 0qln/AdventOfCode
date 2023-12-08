using System.Diagnostics;
using System.Text.RegularExpressions;

var data = @"";

var pattern = @"zero|one|two|three|four|five|six|seven|eight|nine|0|1|2|3|4|5|6|7|8|9";
var regexForward = new Regex(pattern);
var regexBackward = new Regex(Reverse(pattern));

var result = data
    .Split('\n')
    .Sum(line => 
    {
        var first = regexForward.Match(line);
        var last = regexBackward.Match(Reverse(line));
        return int.Parse(GetNumeric(first.Value) + GetNumeric(Reverse(last.Value)));
    });

Console.WriteLine(result);


string? GetNumeric(string s)
{
    return s.ToLower() switch
    {
        "zero" => "0",
        "one" => "1",
        "two" => "2",
        "three" => "3",
        "four" => "4",
        "five" => "5",
        "six" => "6",
        "seven" => "7",
        "eight" => "8",
        "nine" => "9",
        _ => s
    };
}

string Reverse(string s)
{
    char[] charArray = s.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
}

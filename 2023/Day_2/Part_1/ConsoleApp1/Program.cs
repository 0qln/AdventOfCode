using System.Text.RegularExpressions;

var data = @"";


var trueContents = new Set(12, 13, 14);
var result = 0;

foreach (var line in data.Split('\n'))
{
    var game = new Game(line.Split(';').Select(x => new Set(Red(x), Green(x), Blue(x))).ToList(), GameID(line));

    if (game.Reveals.Any(reveal => reveal.Green > trueContents.Green || reveal.Blue > trueContents.Blue || reveal.Red > trueContents.Red))
    {
        continue;
    }

    result += game.ID;
}

Console.WriteLine(result);



int GameID(string line) => int.Parse(new Regex(@"(?<=Game )[0-9]+").Match(line).Value);
int Red(string line) => line.Contains("red") ? int.Parse(new Regex(@"[0-9]+(?= red)").Match(line).Value) : 0;
int Green(string line) => line.Contains("green") ? int.Parse(new Regex(@"[0-9]+(?= green)").Match(line).Value) : 0;
int Blue(string line) => line.Contains("blue") ? int.Parse(new Regex(@"[0-9]+(?= blue)").Match(line).Value) : 0;



record struct Set(int Red, int Green, int Blue);
record struct Game(List<Set> Reveals, int ID);

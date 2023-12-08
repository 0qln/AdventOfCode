
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;


var data = @"";



Console.WriteLine(data
    .Split('\n')
    .Select(ParseGame)
    .Select(MinimumSet)
    .Select(Power)
    .Sum());


Game ParseGame(string data) => new(data.Split(';').Select(ParseSet).ToList(),
                                   GameID(data));

Set ParseSet(string data) => new(Red(data), Green(data), Blue(data));

Set MinimumSet(Game game) => new(game.Reveals.MaxBy(x => x.Red).Red,
                                 game.Reveals.MaxBy(x => x.Green).Green,
                                 game.Reveals.MaxBy(x => x.Blue).Blue);

int Power(Set set) => set.Red * set.Blue * set.Green;
int GameID(string line) => int.Parse(new Regex(@"(?<=Game )[0-9]+").Match(line).Value);
int Red(string line) => line.Contains("red") ? int.Parse(new Regex(@"[0-9]+(?= red)").Match(line).Value) : 0;
int Green(string line) => line.Contains("green") ? int.Parse(new Regex(@"[0-9]+(?= green)").Match(line).Value) : 0;
int Blue(string line) => line.Contains("blue") ? int.Parse(new Regex(@"[0-9]+(?= blue)").Match(line).Value) : 0;



record struct Set(int Red, int Green, int Blue);
record struct Game(List<Set> Reveals, int ID);

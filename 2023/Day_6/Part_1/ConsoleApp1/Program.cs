using System.Diagnostics;
using System.Text.RegularExpressions;
using Race = (int Time, int Distance);


var data = File.ReadAllText(@"../../../input.txt");
var lines = data.Split('\n');
var times = Regex.Matches(lines[0], @"\d+");
var dists = Regex.Matches(lines[1], @"\d+");
Debug.Assert(times.Count == dists.Count);

List<Race> races = new List<Race>();
for (int i = 0; i < times.Count; i++)
    races.Add((int.Parse(times[i].Value), int.Parse(dists[i].Value)));

List<int> results = races.Select(GetNumWins).ToList();
int result = results.Count > 0 ? results[0] : 0;
for (int i = 1; i < results.Count; i++)
{
    result *= results[i];
}


int GetNumWins(Race race)
{
    int wins = 0;
    
    for (int timeWaited = 0; timeWaited < race.Time; timeWaited++)
    {
        int speed = timeWaited;
        int timeLeft = race.Time - timeWaited;
        int distance = speed * timeLeft;

        if (distance > race.Distance)
            wins++;
    }

    return wins;
}


Console.WriteLine(result);
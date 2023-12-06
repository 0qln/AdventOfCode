using System.Diagnostics;
using System.Text.RegularExpressions;
using Race = (ulong Time, ulong Distance);


var data = File.ReadAllText(@"../../../input.txt");
var lines = data.Split('\n');
var times = Regex.Matches(lines[0].Replace(" ", ""), @"\d+");
var dists = Regex.Matches(lines[1].Replace(" ", ""), @"\d+");
Debug.Assert(times.Count == dists.Count);



List<Race> races = new List<Race>();
for (ulong i = 0; i < (ulong)times.Count; i++)
    races.Add((ulong.Parse(times[(int)i].Value), ulong.Parse(dists[(int)i].Value)));

List<ulong> results = races.Select(GetNumWins).ToList();
ulong result = results.Count > 0 ? results[0] : 0;
for (ulong i = 1; i < (ulong)results.Count; i++)
{
    result *= results[(int)i];
}


ulong GetNumWins(Race race)
{
    ulong wins = 0;

    for (ulong timeWaited = 0; timeWaited < race.Time; timeWaited++)
    {
        ulong speed = timeWaited;
        ulong timeLeft = race.Time - timeWaited;
        ulong distance = speed * timeLeft;

        if (distance > race.Distance)
            wins++;
    }

    return wins;
}


Console.WriteLine(result);
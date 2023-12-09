using System.Text.RegularExpressions;
using History = long[];


var reNum = new Regex(@"[\d-]+");
var histories = File
    .ReadAllLines(@"../../../input.txt")
    .Select(x => reNum.Matches(x).Select(x => long.Parse(x.Value)).ToArray());


long results = histories.Sum(history =>
{
    History original = (History)history.Clone();
    List<long> ends = new();

    do
    {
        ends.Add(history[^1]);
        history = Project(history, (x, y) => y - x).ToArray();
    }
    while (history.Any(x => x != 0));

    ends.Add(0);

    List<long> newEnds = [ 0 ];

    for (int i = 1; i < ends.Count; i++)
    {
        newEnds.Add(ends[i] + newEnds[i - 1]);
    }

    return newEnds[^1] + original[^1];
});

IEnumerable<long> Project(IEnumerable<long> sequence, Func<long, long, long> func)
{
    var nums = sequence.ToList();
    for (int i = 0; i < nums.Count - 1; i++)
    {
        yield return func(nums[i], nums[i + 1]);
    }
}


Console.WriteLine(results);
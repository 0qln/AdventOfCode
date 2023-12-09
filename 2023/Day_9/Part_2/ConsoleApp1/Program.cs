using System.Text.RegularExpressions;
using History = long[];


var reNum = new Regex(@"[\d-]+");
var histories = File
    .ReadAllLines(@"../../../input.txt")
    .Select(x => reNum.Matches(x).Select(x => long.Parse(x.Value)).ToArray());


long results = histories.Sum(history =>
{
    History original = (History)history.Clone();
    List<long> begins = new();

    do
    {
        begins.Add(history[0]);
        history = Project(history, (x, y) => y - x).ToArray();
    }
    while (history.Any(x => x != 0));

    begins.Add(0);

    var newBegins = new long[begins.Count];

    newBegins[0] = 0;

    for (int i = 1; i < newBegins.Length; i++)
    {
        newBegins[i] = begins[^i] - newBegins[i - 1];
    }

    return begins[0] - newBegins[^1];
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
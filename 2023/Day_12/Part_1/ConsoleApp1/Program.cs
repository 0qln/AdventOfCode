

using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines(@"../../../input.txt").Sum(PossibleArrangements));


int PossibleArrangements(string row)
{
    List<int> operationals = Regex.Matches(row, @"\d+").Select(MatchValue).Select(int.Parse).ToList();
    string machines = Regex.Match(row, @".+(?= )").Value;


    int result = 0;



    return result;
}


string MatchValue(Match m) => m.Value;


int Factorial(int n) => Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);
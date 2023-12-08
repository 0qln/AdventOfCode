using System.Text.RegularExpressions;


var data = File.ReadAllText(@"../../../input.txt").Split("\r\n\r\n");
var instructions = data[0];
var map = data[1].Split('\n');

Console.WriteLine(TraverseTree(new Instructions(instructions), BuildTree(map)));


Node BuildTree(string[] lines)
{
    Dictionary<string, (string left, string right)> nodes = new(); 
    foreach (var line in lines)    
        nodes.Add(NodeName(line), (LeftNode(line), RightNode(line)));

    return CreateNode(nodes, NodeName(lines[0]));
}

Node CreateNode(Dictionary<string, (string left, string right)> nodes, string name)
{
    Console.WriteLine(name);
    if (name == "ZZZ")
    {
        return new Node(name, null, null);
    }
    else
    {
        return new Node(name, 
            CreateNode(nodes, nodes[name].left), 
            CreateNode(nodes, nodes[name].right));
    }
}

int TraverseTree(Instructions instructions, Node head)
{
    Node? current = head;
    int steps = 0;
    while (current is not null && head.Name != "ZZZ")
    {
        current = current.NextChild(instructions.Next());
        steps++;
    }
    return steps;
}


string NodeName(string node) => Regex.Match(node, @"^[A-Z]+(?= )").Value;
string LeftNode(string node) => Regex.Match(node, @"(?<=\()[A-Z]+(?=,)").Value;
string RightNode(string node) => Regex.Match(node, @"(?<=, )[A-Z]+(?=\))").Value;

record class Node(string Name, Node? Left, Node? Right)
{
    public Node? NextChild(bool left) => left ? Left : Right;
}


record class Instructions(string Raw)
{
    private int _idx;

    /// <returns>Left</returns>
    public bool Next()
    {
        if (++_idx == Raw.Length) _idx = 0;
        return Raw[_idx] == 'R';
    }
}
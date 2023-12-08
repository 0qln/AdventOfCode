using System.Text.RegularExpressions;


var data = File.ReadAllText(@"../../../input.txt").Split("\r\n\r\n");
Console.WriteLine(AllInOneBcChallangeGoofy(data[0], data[1].Split("\n")));

//Console.WriteLine(
//    TraverseTree(
//        BuildTree(data[1].Split('\n'), new Instructions(data[0]))));


int AllInOneBcChallangeGoofy(string instructions, string[] lines)
{
    // Build Tree
    Dictionary<string, (string left, string right)> nodes = new();
    foreach (var line in lines)
        nodes.Add(NodeName(line), (LeftNode(line), RightNode(line)));

    string currentNode = "AAA";
    int currentInstruction = 0;
    int steps = 0;
    while (currentNode != "ZZZ")
    {
        var left = instructions[currentInstruction] == 'L';
        currentNode = left ? nodes[currentNode].left : nodes[currentNode].right;

        currentInstruction++;
        currentInstruction %= instructions.Length;

        steps++;
    }

    return steps;
}


Node BuildTree(string[] lines, Instructions instructions)
{
    Dictionary<string, (string left, string right)> nodes = new(); 
    foreach (var line in lines) 
        nodes.Add(NodeName(line), (LeftNode(line), RightNode(line)));

    return CreateNode(nodes, NodeName(lines[0]), instructions);
}

Node CreateNode(Dictionary<string, (string left, string right)> nodes, string name, Instructions instructions)
{
    if (name == "ZZZ")
    {
        return new Node(name, null);
    }
    var nextNode = instructions.Next() ? nodes[name].left : nodes[name].right;
    return new Node(name, CreateNode(nodes, nextNode, instructions));    
}

int TraverseTree(Node head)
{
    Node? current = head;
    int steps = 1;
    while ((current = current?.NextChild()) is not null && current.Name != "ZZZ")
        steps++;
   
    return steps;
}


string NodeName(string node) => Regex.Match(node, @"^[A-Z]+(?= )").Value;
string LeftNode(string node) => Regex.Match(node, @"(?<=\()[A-Z]+(?=,)").Value;
string RightNode(string node) => Regex.Match(node, @"(?<=, )[A-Z]+(?=\))").Value;

record class Node(string Name, Node? Next)
{
    public Node? NextChild() => Next;
}


record class Instructions(string Raw)
{
    private int _idx = 0;

    /// <returns>Left</returns>
    public bool Next()
    {
        _idx++;
        _idx %= Raw.Length;
        return Raw[_idx] == 'L';
    }
}
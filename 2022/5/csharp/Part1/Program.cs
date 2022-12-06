using System.Text;

if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var allText = await File.ReadAllTextAsync(filePath);

var allTextSplit = allText.Split($"{Environment.NewLine}{Environment.NewLine}").Select(x=>x.Split(Environment.NewLine)).ToArray();

// parse state
var state = allTextSplit[0];
var numStacks = state.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Max();

if (numStacks > 9)
{
    throw new ApplicationException("All this breaks if there are double digit stack numbers");
}

var stacks = new Stack<char>[numStacks];

for (var i = 0; i < numStacks; i++)
{
    var myStack = new Stack<char>();
    var myOffset = 1 + (i * 4);
    for (var j = state.Length - 2; j >= 0; j--)
    {
        var value = state[j][myOffset];
        if (value != ' ')
        {
            myStack.Push(value);
        }
    }

    stacks[i] = myStack;
}

// directions
var directions = allTextSplit[1];
foreach (var line in directions)
{
    var tokens = line.Split(' ');
    var num = int.Parse(tokens[1]);
    var source = int.Parse(tokens[3]) - 1;
    var target = int.Parse(tokens[5]) - 1;

    for (var i = 0; i < num; i++)
    {
        stacks[target].Push(stacks[source].Pop());
    }
}

var sb = new StringBuilder();
foreach (var s in stacks)
{
    sb.Append(s.Peek());
}

var result = sb.ToString();

Console.WriteLine($"Result: {result}");
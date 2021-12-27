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

var lines = await File.ReadAllLinesAsync(filePath);

var scores = new List<long>();
foreach (var line in lines)
{
    Console.WriteLine(line);
    var stack = new Stack<char>();
    var corrupted = false;
    foreach (var token in line.ToCharArray())
    {
        switch (token)
        {
            case '(':
                stack.Push(')');
                break;
            case '[':
                stack.Push(']');
                break;
            case '{':
                stack.Push('}');
                break;
            case '<':
                stack.Push('>');
                break;
            case ')':
            case ']':
            case '}':
            case '>':
                var p = stack.Pop();
                if (p != token)
                {
                    Console.WriteLine($"\tExpected {p}, but found {token} instead");
                    corrupted = true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException("adsf");
        }

        if (corrupted)
        {
            break;
        }
    }
    if (!corrupted)
    {
        var remaining = new StringBuilder();
        while (stack.Any())
        {
            remaining.Append(stack.Pop());
        }

        Console.WriteLine($"\tRemaining: {remaining}");

        long lineScore = 0;
        foreach(var c in remaining.ToString().ToCharArray())
        {
            lineScore *= 5;
            lineScore += c switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => throw new ArgumentOutOfRangeException("adsf"),
            };
        }

        Console.WriteLine($"\tScore: {lineScore}");

        scores.Add(lineScore);
    }
}

var result = scores.OrderBy(x => x).Skip(scores.Count()/2).Take(1).Single();

Console.WriteLine($"Result: {result}");
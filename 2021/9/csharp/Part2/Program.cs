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

var numLines = lines.Length;
var lineLength = lines.First().Length;

var arr = new int[numLines, lineLength];
var visited = new bool[numLines, lineLength];

for (var i = 0; i < numLines; i++)
{
    var tokens = lines[i].ToCharArray()
        .Select(x => (int)char.GetNumericValue(x))
        .ToArray();

    for (var j = 0; j < lineLength; j++)
    {
        arr[i, j] = tokens[j];
    }
}

var localMinima = new List<Tuple<int, int>>();

for (var i = 0; i < numLines; i++)
{
    for (var j = 0; j < lineLength; j++)
    {
        var value = arr[i, j];

        if (i != 0)
        {
            if (arr[i - 1, j] <= value)
            {
                continue;
            }
        }

        if (i != numLines - 1)
        {
            if (arr[i + 1, j] <= value)
            {
                continue;
            }
        }

        if (j != 0)
        {
            if (arr[i, j - 1] <= value)
            {
                continue;
            }
        }

        if (j != lineLength - 1)
        {
            if (arr[i, j + 1] <= value)
            {
                continue;
            }
        }

        localMinima.Add(new Tuple<int, int>(i, j));
    }
}


var result = localMinima.Select(m => FindNeighbors(m.Item1, m.Item2))
    .OrderByDescending(x => x)
    .Take(3)
    .Aggregate((i, x) => i * x);

Console.WriteLine($"Result: {result}");

int FindNeighbors(int i, int j)
{
    if (i == -1 || j == -1 || i == numLines || j == lineLength)
    {
        return 0;
    }

    if (visited[i, j])
    {
        return 0;
    }

    visited[i, j] = true;

    if (arr[i, j] == 9)
    {
        return 0;
    }

    return 1 + 
           FindNeighbors(i - 1, j) + 
           FindNeighbors(i + 1, j) + 
           FindNeighbors(i, j - 1) + 
           FindNeighbors(i, j + 1);
}
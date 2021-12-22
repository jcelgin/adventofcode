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

var localMinima = new List<int>();

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

        if (i != numLines-1)
        {
            if (arr[i + 1, j] <= value)
            {
                continue;
            }
        }

        if (j != 0)
        {
            if (arr[i, j-1] <= value)
            {
                continue;
            }
        }

        if (j != lineLength-1)
        {
            if (arr[i, j + 1] <= value)
            {
                continue;
            }
        }

        localMinima.Add(value);
    }
}

var result = localMinima.Sum() + localMinima.Count;

Console.WriteLine($"Result: {result}");
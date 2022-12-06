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
var totalPriority = 0;

foreach (var line in lines)
{
    var back = line[(line.Length / 2)..].ToHashSet();

    var k = line.Length / 2;
    for (var i = 0; i < k; i++)
    {
        if (back.Contains(line[i]))
        {
            var val = line[i] > 90 ? line[i] - 96 : line[i] - 38;

            totalPriority += val;
            Console.WriteLine($"Common: {line[i]}, {val}");
            break;
        }
    }
}

Console.WriteLine($"Result: {totalPriority}");
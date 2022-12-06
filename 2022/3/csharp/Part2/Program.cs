int GetPriorityOf(char a)
{
    var val1 = a > 90 ? a - 96 : a - 38;
    return val1;
}

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

const int chunkSize = 3;

var totalPriority = lines.Chunk(chunkSize)
    .Select(chunk => chunk[0].Intersect(chunk[1]).Intersect(chunk[2]))
    .Select(intersection => GetPriorityOf(intersection.Single()))
    .Sum();

Console.WriteLine($"Result: {totalPriority}");
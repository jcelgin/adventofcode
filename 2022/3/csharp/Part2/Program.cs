int GetPriorityOf(char a)
{
    return a > 90 ? a - 96 : a - 38;
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

var totalPriority = lines
    .Select(x => x.ToCharArray())
    .Chunk(chunkSize)
    .Select(x => x.Aggregate(Array.Empty<char>(), (s, s1) => !s.Any() ? s1 : s.Intersect(s1).ToArray()))
    .Select(intersection => GetPriorityOf(intersection.Single()))
    .Sum();

Console.WriteLine($"Result: {totalPriority}");
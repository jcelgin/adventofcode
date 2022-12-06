
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

var linesWithOverlap = 0;

bool IsInRange(int test, int min, int max) => min <= test && test <= max;

foreach (var line in lines)
{
    var elfLoad = line.Split(',')
        .Select(x => x.Split('-').Select(int.Parse).ToArray())
        .ToArray();

    var e1 = elfLoad[0];
    var e2 = elfLoad[1];

    if (IsInRange(e1[0], e2[0], e2[1]) || 
        IsInRange(e1[1], e2[0], e2[1]) || 
        IsInRange(e2[0], e1[0], e1[1]) ||
        IsInRange(e2[1], e1[0], e1[1]))
    {
        linesWithOverlap++;
        Console.WriteLine(line);
    }
}

Console.WriteLine($"Result: {linesWithOverlap}");
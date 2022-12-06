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

foreach (var line in lines)
{
    var elfLoad = line.Split(',')
        .Select(x => x.Split('-').Select(int.Parse).ToArray())
        .ToArray();

    var e1 = elfLoad[0];
    var e2 = elfLoad[1];

    var r1 = Enumerable.Range(e1[0], e1[1] - e1[0] + 1);

    if (r1.Contains(e2[0]) || r1.Contains(e2[1]))
    {
        linesWithOverlap++;
        Console.WriteLine(line);
        continue;
    }

    var r2 = Enumerable.Range(e2[0], e2[1] - e2[0] + 1);
    if (r2.Contains(e1[0]) || r2.Contains(e1[1]))
    {
        linesWithOverlap++;
        Console.WriteLine(line);
        continue;
    }
}

Console.WriteLine($"Result: {linesWithOverlap}");
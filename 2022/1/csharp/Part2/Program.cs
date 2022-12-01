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

var list = new List<int>();
var runningTotal = 0;
foreach (var line in lines)
{
    if (line == string.Empty)
    {
        list.Add(runningTotal);
        runningTotal = 0;
        continue;
    }

    runningTotal += int.Parse(line);
}

list.Add(runningTotal);

Console.WriteLine($"Result: {list.OrderByDescending(x=>x).Take(3).Sum(x=>x)}");
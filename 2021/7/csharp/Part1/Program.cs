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

var values = lines.Single().Split(',').Select(int.Parse).ToArray();

var min = values.Min();
var max = values.Max();

var minFuel = int.MaxValue;

for (var i = min; i <= max; i++)
{
    var k = values.Sum(x => Math.Abs(x - i));
    if (k < minFuel)
    {
        minFuel = k;
    }
}

Console.WriteLine($"Result: {minFuel}");
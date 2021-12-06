if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var lines = await File.ReadAllLinesAsync(filePath);

if (lines.Length != 1)
{
    throw new Exception("Expected exactly one line from input file.");
}

var generations = lines.First()
    .Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .GroupBy(x => x)
    .Select(x => new LanternFishGeneration(x.Count(), x.Key))
    .ToList();

const int numDays = 256;
for (var i = 0; i < numDays; i++)
{
    Int64 numNewFish = 0;
    foreach(var g in generations)
    {
        if (g.AdvanceDayAnd())
        {
            numNewFish += g.PopulationSize;
        }
    }

    if (numNewFish != 0)
    {
        generations.Add(new LanternFishGeneration(numNewFish, 8));
    }
}

var result = generations.Sum(x => x.PopulationSize);

Console.WriteLine($"Total fish after {numDays}: {result}");

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

const int numDays = 80;

var inputData = lines.First()
    .Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse);

var fishTimers = new List<int>(inputData);

for(var i = 0; i < numDays; i++)
{
    var newFish = new List<int>();

    for(var j = 0; j < fishTimers.Count(); j++)
    {
        if (fishTimers[j] == 0)
        {
            fishTimers[j] = 6;
            newFish.Add(8);
        }
        else
        {
            fishTimers[j]--;
        }
    }

    fishTimers.AddRange(newFish);
}

Console.WriteLine($"Num fish after {numDays}: {fishTimers.Count()}");
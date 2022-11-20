using System.Diagnostics;

void Display(int[,] grid, int gridSize)
{
    for (var y = 0; y < gridSize; y++)
    {
        for (var x = 0; x < gridSize; x++)
        {
            var value = grid[x, y] == default ? "_" : grid[x, y].ToString();
            Console.Write(value);
        }

        Console.Write(Environment.NewLine);
    }
    Console.Write(Environment.NewLine);
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

const int multiplier = 5;
var inputLineLength = lines[0].Length;
var gridSize = lines[0].Length* multiplier;

var grid = new int[gridSize, gridSize];

for (var y = 0; y < inputLineLength; y++)
{
    var line = lines[y];
    for (var x = 0; x < inputLineLength; x++)
    {
        var numericValue = (int)char.GetNumericValue(line[x]);
        grid[x, y] = numericValue;


        for (var zy = 0; zy < multiplier; zy++)
        {
            var yPos = y + (zy * inputLineLength);
            for (var zx = 0; zx < multiplier; zx++)
            {
                var xPos = x + (zx * inputLineLength);
                var zValue = numericValue + zx + zy;
                if (zValue > 9)
                {
                    zValue -= 9;
                }
                grid[xPos, yPos] = zValue;
            }
        }

    }
}

Display(grid, gridSize);

var destination = new Tuple<int, int>(gridSize - 1, gridSize - 1);

var initialPath = new Path();
initialPath.Advance(new Tuple<int, int>(0, 0), grid[0, 0]);

var paths = new HashSet<Path> { initialPath };
var visited = new HashSet<Tuple<int, int>> { initialPath.Position };

var sw = Stopwatch.StartNew();
while (true)
{
    // find shortest path, and advance it
    var shortestPath = paths.GroupBy(x => x.Weight)
        .OrderBy(x => x.Key).First() // shortest weight
        .OrderByDescending(x => x.Position.Item1 + x.Position.Item2).First(); // nearest the exit.... ish
    paths.Remove(shortestPath);

//    Console.WriteLine($"Weight {shortestPath.Weight}, pos {shortestPath.Position.Item1}, {shortestPath.Position.Item2}");

    var candidates = new List<Tuple<int, int>>
    {
        new(shortestPath.Position.Item1, shortestPath.Position.Item2 - 1),
        new(shortestPath.Position.Item1, shortestPath.Position.Item2 + 1),
        new(shortestPath.Position.Item1 - 1, shortestPath.Position.Item2),
        new(shortestPath.Position.Item1 + 1, shortestPath.Position.Item2),
    };

    foreach (var candidate in candidates)
    {
        if (candidate.Equals(destination))
        {
            sw.Stop();
            // we did it!
            Console.WriteLine($"Weight: {shortestPath.Weight}, {sw.ElapsedMilliseconds} ms");
            return;
        }

        try
        {
            var candidateWeight = grid[candidate.Item1, candidate.Item2];
            if (!visited.Add(candidate))
            {
                // we've already been here, and it was cheaper
                continue;
            }
            var newPath = new Path { Position = shortestPath.Position, Weight = shortestPath.Weight };
            newPath.Advance(candidate, candidateWeight);
            paths.Add(newPath);
        }
        catch (IndexOutOfRangeException)
        {
            continue;
        }
    }
}

public record Path
{
    public int Weight = 0;

    public Tuple<int, int> Position = null!;

    public void Advance(Tuple<int, int> node, int nodeWeight)
    {
        Weight += nodeWeight;
        Position = node;
    }

    /*
    public Path Clone()
    {
        return new Path { Position = Position, Weight = Weight };
    }
    */
}
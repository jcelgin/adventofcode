int CountVisibleDots(bool[,] bools)
{
    var visibleDots = 0;

    for (var y = 0; y < bools.GetLength(1); y++)
    {
        for (var x = 0; x < bools.GetLength(0); x++)
        {
            if (bools[x, y])
            {
                visibleDots++;
            }
        }
    }

    return visibleDots;
}

void Display(bool[,] g)
{
    for (var y = 0; y < g.GetLength(1); y++)
    {
        for (var x = 0; x < g.GetLength(0); x++)
        {
            var display = g[x, y] ? "#" : ".";

            Console.Write(display);
        }

        Console.Write(Environment.NewLine);
    }

    Console.WriteLine(Environment.NewLine);
}

bool[,] FoldLeft(bool[,] g, int foldLine)
{
    var newGrid = new bool[foldLine, g.GetLength(1)];

    for (var x = 0; x < foldLine; x++)
    {
        for (var y = 0; y < g.GetLength(1); y++)
        {
            newGrid[x, y] = g[x, y];
        }
    }

    for (var y = 0; y < g.GetLength(1); y++)
    {
        for (var x = 1; x < g.GetLength(0) - foldLine; x++)
        {
            newGrid[foldLine - x, y] |= g[foldLine + x, y];
        }
    }

    return newGrid;
}

bool[,] FoldUp(bool[,] g, int foldLine)
{
    var newGrid = new bool[g.GetLength(0), foldLine];

    for (var x = 0; x < g.GetLength(0); x++)
    {
        for (var y = 0; y < foldLine; y++)
        {
            newGrid[x, y] = g[x, y];
        }
    }

    for (var y = 1; y < g.GetLength(1) - foldLine; y++)
    {
        for (var x = 0; x < g.GetLength(0); x++)
        {
            newGrid[x, foldLine - y] |= g[x, foldLine + y];
        }
    }

    return newGrid;
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

var maxX = 0;
var maxY = 0;

foreach (var line in lines)
{
    var tokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

    if (tokens.Length == 0)
    {
        break;
    }

    if (tokens[0] > maxX)
    {
        maxX = tokens[0];
    }

    if (tokens[1] > maxY)
    {
        maxY = tokens[1];
    }
}

var grid = new bool[maxX + 1, maxY + 1];
var folds = new List<string>();

foreach (var line in lines)
{
    var tokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

    switch (tokens.Length)
    {
        case 0:
            break;
        case 1:
            folds.Add(line[11..]);
            break;
        default:
            var values = tokens.Select(int.Parse).ToArray();
            grid[values[0], values[1]] = true;
            break;
    }
}

// Display(grid);

foreach (var fold in folds)
{
    var tokens = fold.Split('=');

    if (tokens.Length != 2)
    {
        throw new ApplicationException($"Expected 2 tokens on fold line {fold}.");
    }

    var dimension = tokens[0];
    var position = int.Parse(tokens[1]);

    grid = dimension.ToLower() switch
    {
        "x" => FoldLeft(grid, position),
        "y" => FoldUp(grid, position),
        _ => throw new ArgumentOutOfRangeException($"Unexpected dimension {dimension}")
    };

//    Display(grid);
    var visibleDots = CountVisibleDots(grid);
    Console.WriteLine($"Visible Dots: {visibleDots}");
}

Display(grid);


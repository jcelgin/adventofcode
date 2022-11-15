if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

const int gridSize = 10;

var lines = await File.ReadAllLinesAsync(filePath);

if (lines.Length != gridSize)
{
    throw new ApplicationException($"Expected {gridSize} lines.");
}

var grid = new int[gridSize, gridSize];

for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    if (line.Length != gridSize)
    {
        throw new ApplicationException($"Expected lines with length {gridSize}.");
    }

    for (var j = 0; j < gridSize; j++)
    {
        grid[i, j] = (int)char.GetNumericValue(line[j]);
    }
}

const int maxSteps = 1000;

var numFlashes = 0;

void Display(int step)
{
    for (var i = 0; i < gridSize; i++)
    {
        for (var j = 0; j < gridSize; j++)
        {
            Console.Write(grid[i, j]);
        }
        Console.Write(Environment.NewLine);
    }
    Console.WriteLine($"Step: {step},\tNumFlashes: {numFlashes}");
}

void Increment(int i, int j)
{
    if (i < 0 || j < 0)
    {
        return;
    }

    if (i >= gridSize || j >= gridSize)
    {
        return;
    }

    if (grid[i, j] == -1)
    {
        return;
    }

    if (grid[i, j] == 9)
    {
        grid[i, j] = -1;
        numFlashes++;

        Increment(i - 1, j - 1);
        Increment(i - 1, j);
        Increment(i - 1, j + 1);
        Increment(i, j - 1);
        Increment(i, j + 1);
        Increment(i + 1, j - 1);
        Increment(i + 1, j);
        Increment(i + 1, j + 1);
    }
    else
    {
        grid[i, j]++;
    }
}

for (var step = 0; step < maxSteps; step++)
{
    for (var i = 0; i < gridSize; i++)
    {
        for (var j = 0; j < gridSize; j++)
        {
            Increment(i, j);
        }
    }

    var thisIsIt = true;
    for (var i = 0; i < gridSize; i++)
    {
        for (var j = 0; j < gridSize; j++)
        {
            if (grid[i, j] == -1)
            {
                grid[i, j] = 0;
            }
            else
            {
                thisIsIt = false;
            }
        }
    }

    if (thisIsIt)
    {
        Display(step+1);
        Console.WriteLine($"Result: {step+1}");
        return;
    }
}

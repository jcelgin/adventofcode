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

var myScore = 0;

foreach (var line in lines)
{
    var opMove = line[0];
    var myMove = line[2] - 23;

    var matchScore = myMove - 64;

    if (myMove == opMove)
    {
        matchScore += 3;
    }
    else if (opMove + 1 == myMove || opMove - 2 == myMove)
    {
        matchScore += 6;
    }

    myScore += matchScore;
}

Console.WriteLine($"Result: {myScore}");
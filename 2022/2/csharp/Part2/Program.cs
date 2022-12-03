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
    var idealOutcome = line[2];

    var matchScore = 0;

    var myMove = 0;
    // determine my move
    switch (idealOutcome)
    {
        case 'X':
            // need to lose
            myMove = opMove switch
            {
                'A' => 'C',
                'B' => 'A',
                'C' => 'B',
                _ => myMove
            };
            break;
        case 'Y':
            // equal to the other
            myMove = opMove;
            matchScore += 3;
            break;
        case 'Z':
            // need to win
            myMove = opMove switch
            {
                'A' => 'B',
                'B' => 'C',
                'C' => 'A',
                _ => myMove
            };
            matchScore += 6;
            break;
    }

    var score = myMove - 64;
    matchScore += score;

    myScore += matchScore;
}

Console.WriteLine($"Result: {myScore}");
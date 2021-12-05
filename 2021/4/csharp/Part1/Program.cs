using Part1;

if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var lines = (await File.ReadAllLinesAsync(filePath)).ToArray();
const int boardDimensions = 5;

var numbers = lines[0].Split(',').Select(int.Parse);
var numboards = (lines.Length - 1) / (boardDimensions+1);

var boards = new BingoBoard[numboards];

for (var i = 0; i < numboards; i++)
{
    var boardLines = lines.Skip(1 + (i * (boardDimensions + 1)) + 1).Take(boardDimensions).ToArray(); ;
    int[,] boardSquares = new int[boardDimensions, boardDimensions];
    for(var j = 0; j < boardDimensions; j++)
    {
        var lineValues = boardLines[j].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var k = 0; k < boardDimensions; k++)
        {
            boardSquares[j, k] = int.Parse(lineValues[k]);
        }
    }
    boards[i] = new BingoBoard(boardSquares, boardDimensions);
}

foreach(var number in numbers)
{
    foreach(var board in boards)
    {
        if (board.MarkNumber(number))
        {
            Console.WriteLine(board);

            var sum = board.GetSumUnmarkedSqures();

            Console.WriteLine($"bingo on {number}! sum {sum}, product {sum*number}");
            return;
        }
    }
}

Console.WriteLine("No winners found.");

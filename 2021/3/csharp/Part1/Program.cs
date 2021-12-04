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

var bitsPerLine = lines.First().Length;

int[] onesPerBit = new int[bitsPerLine];

foreach (var line in lines)
{
    for(var i = 0; i < onesPerBit.Length; i++)
    {
        if (line[i] == '1')
        {
            onesPerBit[i]++;
        }
    }
}

var majorityThreshold = lines.Length / 2;
char[] epsilonBits = new char[bitsPerLine];

for (var i = 0; i < bitsPerLine; i++)
{
    epsilonBits[i] = onesPerBit[i] > majorityThreshold ? '1' : '0';
}

var epsilon = Convert.ToInt16(new string(epsilonBits), 2);
var epsilonInvertedBits = Convert.ToString(~epsilon, 2).Remove(0, 32 - bitsPerLine);
var gamma = Convert.ToInt16(epsilonInvertedBits, 2);

Console.WriteLine($"epsilon: {epsilon}, gamma: {gamma}, product: {epsilon*gamma}");

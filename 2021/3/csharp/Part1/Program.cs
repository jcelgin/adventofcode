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
char[] gammaBits = new char[bitsPerLine];

for (var i = 0; i < bitsPerLine; i++)
{
    if (onesPerBit[i] > majorityThreshold)
    {
        epsilonBits[i] = '1';
        gammaBits[i] = '0';
    }
    else
    {
        epsilonBits[i] = '0';
        gammaBits[i] = '1';
    }
}

var epsilon = Convert.ToInt16(new string(epsilonBits), 2);
var gamma = Convert.ToInt16(new string(gammaBits), 2);

Console.WriteLine($"epsilon: {epsilon}, gamma: {gamma}, product: {epsilon*gamma}");

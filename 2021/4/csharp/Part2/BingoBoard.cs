using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part1
{
    internal class BingoBoard
    {
        internal BingoBoard(int[,] squareValues, int dimensions)
        {
            _squareValues = squareValues;
            _dimensions = dimensions;

            _numberLookup = new Dictionary<int, Tuple<int, int>>();

            for (var x = 0; x < _dimensions; x++)
            {
                for (var y = 0; y < _dimensions; y++)
                {
                    if (!_numberLookup.TryAdd(squareValues[x, y], new Tuple<int, int>(x, y)))
                    {
                        throw new Exception($"Board with duplicate value {squareValues[x, y]}");
                    }
                }
            }
        }

        private readonly int _dimensions;

        private readonly int[,] _squareValues;

        private readonly Dictionary<int, Tuple<int, int>> _numberLookup = new Dictionary<int, Tuple<int, int>>();

        private const int _sentinalValue = -99;

        public bool HasBingo { get; private set; } = false;

        // returns true if marked number triggers bingo.  Avoids evaluation if board has already achieved bingo.
        public bool MarkNumber(int number)
        {
            if (HasBingo)
            {
                return false;
            }

            if (!_numberLookup.TryGetValue(number, out var coordinates)){
                return false;
            }

            _squareValues[coordinates.Item1, coordinates.Item2] = _sentinalValue;

            // see if row is bingo
            var rowBingo = true;
            for(var i = 0; i < _dimensions; i++)
            {
                if (_squareValues[i, coordinates.Item2] != _sentinalValue)
                {
                    rowBingo = false;
                    break;
                }
            }

            if (rowBingo)
            {
                HasBingo = true;
                return true;
            }

            for (var i = 0; i < _dimensions; i++)
            {
                if (_squareValues[coordinates.Item1, i] != _sentinalValue)
                {
                    return false;
                }
            }

            HasBingo = true;
            return true;
        }

        public int GetSumUnmarkedSqures()
        {
            var sum = 0;
            foreach(var k in _squareValues)
            {
                if (k != _sentinalValue)
                {
                    sum += k;
                }
            }
            return sum;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(var i = 0; i < _dimensions; i++)
            {
                for (var j = 0; j< _dimensions; j++)
                {
                    sb.Append(_squareValues[i, j]);
                    sb.Append("\t");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}

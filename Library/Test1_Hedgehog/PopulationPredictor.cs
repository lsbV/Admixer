using System.Diagnostics;

namespace Library.Test1_Hedgehog;

public class PopulationPredictor
{
    private long _desirable;
    private long _undesirable1;
    private long _undesirable2;

    public int CountMeetsForWholeConversion(int desirableColor, int[] initPopulation)
    {
        try
        {
            ValidateInput(desirableColor, initPopulation);
        }
        catch (ArgumentException)
        {
            return -1;
        }

        var color = (HedgehogColor)desirableColor;
        var desiredPopulation = initPopulation[desirableColor];
        var undesirablePopulationsTuple = color switch
        {
            HedgehogColor.Red => (initPopulation[(int)HedgehogColor.Green], initPopulation[(int)HedgehogColor.Blue]),
            HedgehogColor.Green => (initPopulation[(int)HedgehogColor.Red], initPopulation[(int)HedgehogColor.Blue]),
            HedgehogColor.Blue => (initPopulation[(int)HedgehogColor.Red], initPopulation[(int)HedgehogColor.Green]),
            _ => throw new ArgumentOutOfRangeException(nameof(desirableColor), desirableColor, null)
        };

        _desirable = desiredPopulation;
        _undesirable1 = undesirablePopulationsTuple.Item1;
        _undesirable2 = undesirablePopulationsTuple.Item2;


        try
        {
            var countOfSteps1 = MakeDesiredPopulationTheBiggest();
            var countOfSteps2 = MakeSmallestPopulationEmpty();
            var countOfSteps3 = MakeUndesirablePopulationsEqual();
            var countOfSteps4 = ConvertUndesirablePopulationsToDesirable();
            return countOfSteps1 + countOfSteps2 + countOfSteps3 + countOfSteps4;
        }
        catch (PopulationCalculationException)
        {
            return -1;
        }

    }

    private int MakeSmallestPopulationEmpty()
    {
        if (_undesirable1 == 0 || _undesirable2 == 0)
        {
            return 0;
        }
        if (_undesirable1 > _undesirable2)
        {
            _desirable += _undesirable2 * 2;
            _undesirable1 -= _undesirable2;
            _undesirable2 = 0;
        }
        else
        {
            _desirable += _undesirable1 * 2;
            _undesirable2 -= _undesirable1;
            _undesirable1 = 0;
        }

        return 1;
    }


    private static void ValidateInput(int desirableColor, int[] initPopulation)
    {
        if (initPopulation.Length != 3)
        {
            throw new ArgumentException("initPopulation must have 3 elements");
        }
        if (initPopulation.Any(i => i < 0))
        {
            throw new ArgumentException("initPopulation can't contain negative values");
        }
        if (initPopulation.All(i => i == 0))
        {
            throw new ArgumentException("Population is empty");
        }
        if (desirableColor is < 0 or > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(desirableColor));
        }
        if (initPopulation.Count(i => i == 0) == 2)
        {
            throw new ArgumentException("all have the same color");
        }

    }
    private int ConvertUndesirablePopulationsToDesirable()
    {
        if (_undesirable1 == 0 && _undesirable2 == 0)
        {
            return 0;
        }
        _desirable += _undesirable1 + _undesirable2;
        _undesirable1 = _undesirable2 = 0;
        return 1;
    }
    private int MakeDesiredPopulationTheBiggest()
    {
        var countOfSteps = 0;
        while (_desirable < (_undesirable1 + _undesirable2))
        {
            countOfSteps++;
            if (_desirable == 0)
            {
                var smaller = Math.Min(_undesirable1, _undesirable2);
                _desirable += smaller * 2;
                _undesirable1 -= smaller;
                _undesirable2 -= smaller;
            }
            else
            {
                if (_undesirable1 > _undesirable2)
                {
                    _undesirable1 -= _desirable;
                    _undesirable2 += _desirable * 2;
                    _desirable = 0;
                }
                else
                {
                    _undesirable1 += _desirable * 2;
                    _undesirable2 -= _desirable;
                    _desirable = 0;
                }

            }
        }

        return countOfSteps;
    }
    private int MakeUndesirablePopulationsEqual()
    {
        var countOfSteps = 0;

        while (_undesirable1 != _undesirable2)
        {
            countOfSteps++;

            var difference = Math.Abs(_undesirable1 - _undesirable2);
            if (difference is 1 or 2)
            {
                throw new PopulationCalculationException(
                    "Impossible to calculate if the difference between undesirable populations is 1 or 2");
            }

            var bigger = Math.Max(_undesirable1, _undesirable2);
            var third = bigger / 3;
            if (_undesirable1 > _undesirable2)
            {
                _undesirable1 -= third;
                _undesirable2 += third * 2;
            }
            else
            {
                _undesirable2 -= third;
                _undesirable1 += third * 2;
            }

            _desirable -= third;

        }

        return countOfSteps;
    }
    private enum HedgehogColor
    {
        Red,
        Green,
        Blue
    }
    private class PopulationCalculationException(string message) : Exception(message);
}
using Library.Test1_Hedgehog;

namespace LibraryTests;

public class PopulationPredictorTests
{
    [Theory]
    [InlineData(2, new[] { 2, 2, 4 }, 1)]
    [InlineData(2, new[] { 1234567890, 222333444, 2111999333 }, 3)]
    [InlineData(2, new[] { 1234567891, 22, 999333 }, 12)]
    [InlineData(0, new[] { 0, 1, 100 }, 9)]
    [InlineData(0, new[] { 123, 544, 1000000 }, 20)]
    public void Test(int color, int[] populations, int expectedMeets)
    {
        //act
        var result = new PopulationPredictor().CountMeetsForWholeConversion(color, populations);
        //assert
        Assert.Equal(expectedMeets, result);
    }

    [Theory]
    [InlineData(0, new[] { 8, 1, 9 })]
    [InlineData(-1, new[] { 2, 2, 4 })] // invalid color
    [InlineData(3, new[] { 2, 2, 4 })] // invalid color
    [InlineData(2, new[] { 2, 2 })] // invalid population
    [InlineData(2, new[] { 2, 2, 4, 5 })] // invalid population
    [InlineData(2, new[] { 10, 0, 1 })] // impossible to calculate result
    public void CountMeetsForWholeConversion_ShouldReturnMinusOne_WhenItIsImpossibleToCalculateResult(int color, int[] populations)
    {
        var result = new PopulationPredictor().CountMeetsForWholeConversion(color, populations);
        //assert
        Assert.Equal(-1, result);
    }
}
namespace Library.Test2_Sudoku;

public class SudokuValidator
{
    private const int LengthOfSubArray = 9;
    private const int ForbiddenValueForNode = 0;
    private const int CountOfUniqueValuesInCombination = 9;
    private const int AreaSize = 3;

    // ReSharper disable once InconsistentNaming
    public bool validSolution(int[][] data)
    {
        if (data.Any(subArray => subArray.Length != LengthOfSubArray || subArray.Any(element => element == ForbiddenValueForNode)))
        {
            return false;
        }

        if (!RowsAreValid(data)) return false;
        if (!ColumnsAreValid(data)) return false;
        if (!AreasAreValid(data)) return false;

        return true;
    }

    private static bool RowsAreValid(int[][] data)
    {
        return data.Select(row => new HashSet<int>(row)).All(set => set.Count == CountOfUniqueValuesInCombination);
    }

    private static bool ColumnsAreValid(int[][] data)
    {
        var columns = Enumerable.Range(0, LengthOfSubArray - 1)
            .Select(colIndex => data
                .Where(row => row.Length > colIndex)
                .Select(row => row[colIndex]));

        return columns.Select(colum => new HashSet<int>(colum)).All(set => set.Count == CountOfUniqueValuesInCombination);
    }

    private static bool AreasAreValid(int[][] data)
    {
        var subGrids = Enumerable.Range(0, AreaSize)
            .SelectMany(rowBlock => Enumerable.Range(0, AreaSize)
                .Select(colBlock => Enumerable.Range(0, AreaSize)
                    .Select(rowOffset => Enumerable.Range(0, AreaSize)
                        .Select(colOffset => GetDataByOffset(rowBlock, colBlock, rowOffset, colOffset, data))
                        .ToArray())
                    .ToArray()))
        .ToList();

        return subGrids.Select(area => new HashSet<int>(area.SelectMany(item => item))).All(set => set.Count == 9);
    }

    private static int GetDataByOffset(int row, int column, int rowOffset, int columnOffset, int[][] data)
    {
        return data[row * AreaSize + rowOffset][column * AreaSize + columnOffset];
    }
}
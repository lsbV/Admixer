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
            .Select(colIndex => data.Select(row => row[colIndex]));

        return columns.Select(colum => new HashSet<int>(colum)).All(set => set.Count == CountOfUniqueValuesInCombination);
    }

    private static bool AreasAreValid(int[][] data)
    {
        var areas = Enumerable.Range(0, AreaSize)
            .SelectMany(rowAreaIndex => Enumerable.Range(0, AreaSize)
                .Select(columnAreaIndex => Enumerable.Range(0, AreaSize)
                    .Select(rowIndexInsideArea => Enumerable.Range(0, AreaSize)
                        .Select(columnIndexInsideArea =>
                            GetDataByOffset(rowAreaIndex, columnAreaIndex, rowIndexInsideArea, columnIndexInsideArea, data)))
                )).Select(area => area.SelectMany(item => item));

        return areas.Select(area => new HashSet<int>(area)).All(set => set.Count == CountOfUniqueValuesInCombination);
    }

    private static int GetDataByOffset(int rowAreaIndex, int columnAreaIndex, int rowIndexInsideArea, int columnIndexInsideArea, int[][] data)
    {
        var row = rowAreaIndex * AreaSize + rowIndexInsideArea;
        var column = columnAreaIndex * AreaSize + columnIndexInsideArea;
        return data[row][column];
    }
}
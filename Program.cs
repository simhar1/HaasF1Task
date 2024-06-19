// Iterate over the elements. For each element, pick the element and move ahead recursively and add the subset to our result.
// Using backtracking, remove the element and continue finding the subsets and adding them to the result.
// Complexity: O(N * log(2N) * 2N) = O(N2 * 2N)

// Note: this is a recursive approach, iterative & bitmask approaches are also valid.

// Find all unique subsets
using System.Diagnostics;

static void FindSubSet(in List<int> inputData, int index, List<int> subset, List<List<int>> result)
{
    result.Add(new List<int>(subset));

    // Iterate over every element in the array
    for (var j = index; j < inputData.Count; j++)
    {
        // Pick the element and move ahead
        subset.Add(inputData[j]);
        FindSubSet(inputData, j + 1, subset, result);

        // Backtrack to drop the element
        subset.RemoveAt(subset.Count - 1);
    }
}

// Return all unique subsets
static List<List<int>> GetAllUniqueSubSets(in List<int> inputData)
{
    // Store the subsets results
    var result = new List<List<int>>();
    var subset = new List<int>();

    // Find the subsets
    FindSubSet(inputData, 0, subset, result);

    // Ensure items are distinct (in case original data set does not have unique values, e.g. {1,2,2} etc...)
    var newList = result.Distinct(ListComparer<int>.Default).ToList();
    return newList;
}

// Print a subset list
static void PrintList(in IReadOnlyList<int> v)
{
    Console.Write($"{{{string.Join(',', v)}}} ");
}

// Print a list of lists
static void PrintListOfLists(in IReadOnlyList<IReadOnlyList<int>> listOfLists)
{
    Console.Write("{ ");
    foreach (IReadOnlyList<int> v in listOfLists)
    {
        PrintList(v);
    }

    Console.Write(" }");
}

// Input Data
var inputArray = new List<int> { 1, 2, 3 };

// Start Stop watch
var timer = new Stopwatch();
timer.Start();

// Get all the unique subsets
var result = GetAllUniqueSubSets(inputArray);

// Stop the timer
timer.Stop();

// Print all the results
PrintListOfLists(result);

TimeSpan timeTaken = timer.Elapsed;

// Print out how long this took
Console.WriteLine("\r\nTime taken: " + timeTaken.ToString(@"m\:ss\.fff"));

#region ListComparer
internal sealed class ListComparer<T>
    : IEqualityComparer<List<T>>
{
    private readonly IEqualityComparer<T> comparer;

    public ListComparer()
        : this(null)
    {
    }

    public ListComparer(
        IEqualityComparer<T>? itemEqualityComparer)
    {
        this.comparer = itemEqualityComparer ?? EqualityComparer<T>.Default;
    }

    public static readonly ListComparer<T> Default = new();

    public bool Equals(List<T>? x, List<T>? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (ReferenceEquals(x, null)
            || ReferenceEquals(y, null))
            return false;
        return x.Count == y.Count && !x.Except(y, this.comparer).Any();
    }

    public int GetHashCode(List<T> list)
    {
        return 16 + list.Select(x => this.comparer.GetHashCode(x))
            .OrderBy(h => h)
            .Sum(itemHash => 31 * itemHash);
    }
}

#endregion

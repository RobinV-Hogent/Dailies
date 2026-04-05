// Robin Vermeir | Started: 05-04-2026

using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailySolutions
{
    public class Solutions
    {

        // TargetSum | 05-04-2026 | Day 1
        // Given an array of integers arr[] and an integer target.We need to build an expression out of arr[] by adding one of the symbols '+' or  '-' before each integer in arr[] and then concatenate all the integers.
        // For example : if arr[] = [2, 1], you can add a '+' before 2 and a '-' before 1 and concatenate them to build the expression "+2-1".
        // Return the number of different expressions that can be built, which evaluates to target.

        // Note : An expression is considered different from another if the placement of '+' and '-' operators differs, even if the resulting value is the same. 
        // Note : An expression is considered different from another if the placement of '+' and '-' operators differs, even if the resulting value is the same. 
        public static int totalWays(int[] arr, int target)
        {
            int totalSolutions = 0;
            int options = 1 << arr.Length;

            for (int i = 0; i < options; i++)
            {
                bool[] binary = Convert.ToString(i, 2).PadLeft(arr.Length, '0').Select(e => e == '1').ToArray();
                int sum = arr.Select((e, index) => binary[index] ? e : -e).Sum();

                if (sum == target) totalSolutions++;

            }

            return totalSolutions;
        }
    }
}

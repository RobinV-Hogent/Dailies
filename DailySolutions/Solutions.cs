// Robin Vermeir | Started: 05-04-2026

using System.Linq.Expressions;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
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



        // Huffman Codes | 06-04-2026 | Day 2
        // Given a string s of distinct characters and their corresponding frequency f[] i.e.character s[i] has f[i] frequency.
        // You need to build the Huffman tree and return all the huffman codes in preorder traversal of the tree.
        // Note: While merging if two nodes have the same value, then the node which occurs at first 
        // will be taken on the left of Binary Tree and the other one to the right, otherwise Node with less value will be 
        // taken on the left of the subtree and other one to the right.
        public static List<string> huffmanCodes(string s, int[] f)
        {
            int totalFrequencies = f.Sum();
            int total = -1;

            var currentElements = new List<(int Total, object Small, object Big, string? Letter)>();
            for (int i = 0; i < s.Length; i++)
            {
                currentElements.Add((Total: f[i], Small: null!, Big: null!, Letter: s[i].ToString()));
            }

            var sorted = currentElements.OrderBy(e => e.Total).ToList();
            do
            {
                var smallest = sorted[0];
                var bigger = sorted[1];

                total = smallest.Total + bigger.Total;

                var node = (Total: total, Small: smallest, Big: bigger, Letter: "");

                sorted = sorted[2..].ToList();
                sorted.Add(node);
                sorted = sorted.OrderBy(e => e.Total).ToList();

            } while (total != totalFrequencies);

            var codeMap = new Dictionary<string, string>();

            void ReadNode((int Total, object Small, object Big, string? Letter) node, string currentCode)
            {
                if (node.Small == null && node.Big == null)
                {
                    if (!string.IsNullOrEmpty(node.Letter))
                    {
                        codeMap[node.Letter] = currentCode;
                    }
                    return;
                }

                if (node.Small != null)
                {
                    var smallNode = ((int Total, object Small, object Big, string? Letter))node.Small;
                    ReadNode(smallNode, currentCode + "0");
                }

                if (node.Big != null)
                {
                    var bigNode = ((int Total, object Small, object Big, string? Letter))node.Big;
                    ReadNode(bigNode, currentCode + "1");
                }
            }

            ReadNode(sorted[0], "");

            var sortedLetters = new List<(string Letter, int Frequency)>();
            for (int i = 0; i < s.Length; i++)
            {
                sortedLetters.Add((s[i].ToString(), f[i]));
            }

            var finalOrder = sortedLetters.OrderByDescending(x => x.Frequency).ToList();

            List<string> codes = new List<string>();
            foreach (var item in finalOrder)
            {
                if (codeMap.TryGetValue(item.Letter, out string? code))
                {
                    codes.Add(code);
                }
            }

            Console.WriteLine(string.Join(", ", codes));

            return codes;
        }
    }
}

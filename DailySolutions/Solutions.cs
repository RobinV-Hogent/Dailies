// Robin Vermeir | Started: 05-04-2026

using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailySolutions
{
    public class Solutions
    {
        // Sorted subsequence of size 3 | 10-04-2026 | Day 6
        // Given an array arr[], find any subsequence of three elements such that, arr[i] < arr[j] < arr[k] and(i<j<k).
           
        // If such a subsequence exists, return the three elements as an array.Otherwise, return an empty array.
           
        // Note:
           
        // The driver code will print 1 if the returned subsequence is valid and present in the array.
        // The driver code will print 0 if no such subsequence exists.
        // If the returned subsequence does not satisfy the required format or conditions, the output will be -1.
        public static List<int> find3Numbers(int[] arr)
        {
            // voor elk getal kijken naar de getallen rechts ervan
            List<int> result = new List<int>();

            for (int i = 0; i < arr.Length; i++)
            {
                int currentNumber = arr[i];
                result.Add(currentNumber);
                
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] > currentNumber)
                    {
                        currentNumber = arr[j];
                        result.Add(arr[j]);
                        if(result.Count == 3) {
                            Console.WriteLine(1);
                            return result;
                        }
                    }
                }

                result.Clear();
            }


            Console.WriteLine(0);
            return result;
        }


        // Intersection of Two Sorted Arrays | 09-04-2026 | Day 5
        // Given two sorted arrays a[] and b[], where each array may contain duplicate elements, return the elements in the intersection
        // of the two arrays in sorted order.
        // Note: Intersection of two arrays can be defined as the set containing distinct common elements that are present in both of the arrays.
        public static List<int> intersection(int[] a, int[] b)
        {
            HashSet<int> result = new HashSet<int>();

            bool expression = a.Length < b.Length;
            int[] largest = expression ? a : b;
            HashSet<int> smallest = new HashSet<int>(expression ? b : a);

            foreach (int x in largest)
            {
                //if (result.Contains(x)) continue;

                //foreach (int y in smallest)
                //{
                //    if (y != x) continue;
                //    result.Add(x);
                //    break;
                //}

                if (smallest.Contains(x))
                {
                    result.Add(x);
                }
            }

            return result.ToList();
        }

        // Segregate 0s and 1s | 08-04-2026 | Day 4
        // Given an array arr[] consisting of only 0's and 1's.Modify the array in-place to segregate 0s 
        // onto the left side and 1s onto the right side of the array.
        public static void segregate0and1(int[] arr)
        {
            // code here    
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1 || i == 0) continue;

                // if arr[i] = 0
                int index = i;
                while (!(index - 1 < 0) && arr[index - 1] == 1)
                {
                    int current = arr[index];
                    int left = arr[index - 1];

                    arr[index] = left;
                    arr[index - 1] = current;

                    index--;
                }
            }

            Console.WriteLine(string.Join(", ", arr));
        }

        // Stable Marriage Problem | 07-04-2026 | Day 3
        // Could not solve
        // YT video: https://www.youtube.com/watch?v=Qcv1IqHWAzg


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

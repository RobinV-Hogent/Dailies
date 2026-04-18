// Robin Vermeir | Started: 05-04-2026

using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailySolutions
{
    public class Solutions
    {
        // Flip to Maximize 1s | 18-04-2026 | Day 13
        // Given an array arr[] consisting of 0’s and 1’s.
        // A flip operation involves changing all 0's to 1's and all 1's to 0's within a contiguous subarray.
        // Formally, select a range(l, r) in the array arr[], such that(0 ≤ l ≤ r<n) holds and flip the elements in this range.
        // Return the maximum number of 1's you can get in the array after doing at most 1 flip operation.
        // OVERCOMLICATED, but I loved the thought process and the solution I came up with, but its still wrong. 
        
        public static int maxOnes(int[] arr)
        {
            Stack<int> stack = new Stack<int>(arr);
            if (stack.Count == 1) return arr.Count(e => e == 1);

            int[] indexes = new int[2];
            int currentIndex = 0;
            for (int i = 0; i < 2; i++)
            {

                int nextNum = stack.Pop();
                currentIndex++;
                int proceidingNum = stack.Peek();

                while (nextNum == 1 || ((nextNum == 1 && proceidingNum == 0) || (nextNum == 0 && proceidingNum == 1)))
                {
                    if (((nextNum == 1 && proceidingNum == 0) || (nextNum == 0 && proceidingNum == 1)) && nextNum != 1)
                    {
                        stack.Pop();
                        currentIndex++;
                    }

                    nextNum = stack.Pop();
                    currentIndex++;
                    if (stack.Count == 0) break;
                    proceidingNum = stack.Peek();
                }

                stack.Push(nextNum);
                indexes[i] = currentIndex - 1;
                currentIndex = 0;

                Array.Reverse(arr);
                stack = new Stack<int>(arr);
            }

            // Because the stack reverses the array
            indexes[0] = arr.Length - indexes[0];


            int[] count = new int[2];
            if (indexes[0] < indexes[1])
            {
                // Something went wrong, have to look at the edges
                for (int i = 0; i < 2; i++)
                {
                    int idx = 0;
                    while (arr[idx] == 0)
                    {
                        count[i]++;
                        idx++;
                    }

                    Array.Reverse(arr);
                }

                // change left
                if (count[0] > count[1])
                {
                    indexes[0] = 1;
                    indexes[1] = 0;
                }
                else
                {
                    indexes[0] = arr.Length;
                    indexes[1] = arr.Length - 1;
                }
            }

            // Convert to bool, easier to flip
            bool[] boolArray = arr.Select(e => e == 1).ToArray();

            // Flip numbers
            for (int i = indexes[1]; i < indexes[0]; i++) boolArray[i] = !boolArray[i];



            return boolArray.Count(true);
        }




        // Anagram Palindrome | 17-04-2026 | Day 12
        // Given a string s, determine whether its characters can be rearranged to form a palindrome.
        // Return true if it is possible to rearrange the string into a palindrome; otherwise, return false.
        public static bool canFormPalindrome(string s)
        {
            Dictionary<char, int> charCounts = new Dictionary<char, int>();

            foreach (char ch in s)
            {
                if (charCounts.ContainsKey(ch)) charCounts[ch] = (charCounts[ch] + 1) % 2;
                else charCounts[ch] = 1;
            }

            return charCounts.Values.Sum() <= 1;
        }



        // MyAtoi | 16-04-2026 | Day 11
        // Given a string s, convert it into a 32-bit signed integer(similar to the atoi() function) without using any built-in conversion functions.
        // The conversion follows these rules:

        // Ignore Leading Whitespaces: Skip all leading whitespace characters.
        // Check Sign: If the next character is either '+' or '-', take it as the sign of the number.If no sign is present, assume the number is positive.
        // Read Digits: Read the digits and ignore any leading zeros. Stop reading when a non-digit character is encountered or the end of the string is reached.If no digits are found, return 0.
        // Handle Overflow: If the number exceeds the range of a 32-bit signed integer:
        // Return 2³¹ − 1 (i.e., 2147483647) if it is greater than the maximum value.
        // Return −2³¹ (i.e., -2147483648) if it is smaller than the minimum value.
        public static int myAtoi(string s)
        {
            char[] allowedChars = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            // This list does not show a 0;
            int index = s.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (index == -1) return 0; // No digits found, return 0

            int negativeIndex = s.IndexOf('-');
            bool isNegative = negativeIndex != -1 && negativeIndex < index;

            List<char> correct = new List<char>();
            char currentChar = s[index];

            while (allowedChars.Contains(currentChar))
            {
                correct.Add(currentChar);
                index++;
                if (index > s.Length - 1) break;
                currentChar = s[index];
            }

            char[] final = correct.ToArray();
            long total = 0;

            for (int i = 1; i <= final.Length; i++)
            {
                long multiplier = (long)Math.Pow(10, i);
                long value = long.Parse(final[final.Length - i].ToString());
                total += multiplier * value;
            }

            total /= 10;
            if (isNegative) total *= -1;

            if (total < 0 && total < (long)int.MinValue) total = int.MinValue;
            if (total > 0 && total > (long)int.MaxValue) total = int.MaxValue;

            return (int)total;
        }

        // URLify a given string | 15-04-2026 | Day 10
        // Given a string s, replace all the spaces in the string with '%20'.
        public static string URLify(string s)
        {
            string newText = "";

            foreach (var ch in s)
            {
                if (ch == ' ') newText += "%20";
                else newText += ch;
            }

            return newText;
        }

        // Remove Spaces | 14-04-2026 | Day 9
        // Given a string s, remove all the spaces from the string and return the modified string.
        public static string RemoveSpaces(string s)
        {
            string newText = "";

            foreach (var ch in s)
            {
                if (!(ch == ' ')) newText += ch;
            }

            return newText;
        }

        // Next Smallest Palindrome | 13-04-2026 | Day 8
        // Given a number, in the form of an array num[] containing digits from 1 to 9(inclusive). 
        // Find the next smallest palindrome strictly larger than the given number.
        public static int[] nextPalindrome(int[] num)
        {
            // Voor elk getal in de array wil ik kijken naar de arr.length - i - 1
            // Als getal groter is dan getal links van laatste getal + 1
            // anders verander getal naar wat links staat
            for (int i = 0; i < num.Length; i++)
            {
                if (i - num.Length == 0) break;

                int left = num[i];
                int right = num[num.Length - 1 - i];

                if (right > left)
                {
                    num[num.Length - 1 - i] = left;
                    num[num.Length - 1 - i - 1] = num[num.Length - 1 - i - 1] + 1;
                }
                else
                {
                    num[num.Length - 1 - i] = left;
                }
            }

            Console.WriteLine(string.Join(",", num));

            return [];
        }


        // Toeplitz matrix | 12-04-2026 | Day 7
        // A Toeplitz matrix(also known as a diagonal-constant matrix) is a matrix in which every descending diagonal from left to right contains the same element.

        // Given a rectangular matrix mat, determine whether it is a Toeplitz matrix or not.
        // Implement the function isToeplitz(mat) that returns:

        // true if the matrix is a Toeplitz matrix
        // false otherwise
        public static bool isToeplitz(int[][] mat)
        {
            // Niet alle opvolgende arrays moeten gecheckt worden, alleen die na elkaar komen
            for (int i = 0; i < mat.Length; i++)
            {
                if (i + 1 > mat.Length - 1) break;

                int[] primary = mat[i];
                int[] secondary = mat[i + 1];

                for (int j = 0; j < primary.Length - 1; j++)
                {
                    if (primary[j] != secondary[j + 1]) return false;
                }
            }

            return true;
        }




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
                        if (result.Count == 3)
                        {
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

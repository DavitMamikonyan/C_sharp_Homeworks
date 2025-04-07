using System;
using System.Diagnostics;


namespace StrategySortApp
{
// Strateg interface
    public interface ISortStrategy
    {
        void Sort(int[] array);
    }

//Insertion
    public class InsertionSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int key = array[i];
                int j = i - 1;
                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = key;
            }
        }
    }

//selection
    public class SelectionSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[minIndex])
                        minIndex = j;
                }
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }
        }
    }

//merge
    public class MergeSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            MergeSortRecursive(array, 0, array.Length - 1);
        }

        private void MergeSortRecursive(int[] array, int left, int right)
        {
            if (left >= right) return;

            int mid = (left + right) / 2;
            MergeSortRecursive(array, left, mid);
            MergeSortRecursive(array, mid + 1, right);
            Merge(array, left, mid, right);
        }

        private void Merge(int[] array, int left, int mid, int right)
        {
            int[] temp = new int[right - left + 1];
            int i = left, j = mid + 1, k = 0;

            while (i <= mid && j <= right)
                temp[k++] = array[i] < array[j] ? array[i++] : array[j++];

            while (i <= mid) temp[k++] = array[i++];
            while (j <= right) temp[k++] = array[j++];

            for (int t = 0; t < temp.Length; t++)
                array[left + t] = temp[t];
        }
    }

//shell
    public class ShellSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            int n = array.Length;
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = array[i];
                    int j;
                    for (j = i; j >= gap && array[j - gap] > temp; j -= gap)
                        array[j] = array[j - gap];
                    array[j] = temp;
                }
            }
        }
    }

// Strategy
    public class SortContext
    {
        private ISortStrategy _strategy;

        public void SetStrategy(ISortStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Sort(int[] array)
        {
            _strategy.Sort(array);
        }
    }

// Main()
    class Program
    {
        static void Main()
        {
            int[] original = GenerateRandomArray(10000);

            TestSort("Insertion Sort", new InsertionSort(), original);
            TestSort("Selection Sort", new SelectionSort(), original);
            TestSort("Merge Sort", new MergeSort(), original);
            TestSort("Shell Sort", new ShellSort(), original);

            Console.ReadLine(); // Ждём ввод, чтобы окно не закрылось сразу
        }

        static void TestSort(string name, ISortStrategy strategy, int[] original)
        {
            int[] array = (int[])original.Clone();
            var context = new SortContext();
            context.SetStrategy(strategy);

            var sw = Stopwatch.StartNew();
            context.Sort(array);
            sw.Stop();

            Console.WriteLine($"{name} took {sw.ElapsedMilliseconds} ms");
        }

        static int[] GenerateRandomArray(int size)
        {
            Random rand = new Random();
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = rand.Next(0, 10000);
            return array;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

class Week02Program
{
    // Homework1. 获取指定数据的所有素数因子
    static List<int> GetPrimeFactors(int number)
    {
        List<int> factors = new List<int>();
        for (int i = 2; i <= number; i++)
        {
            while (number % i == 0)
            {
                factors.Add(i);
                number /= i;
            }
        }
        return factors.Distinct().ToList();
        //todistinct 是去重函数
    }

    // Homework2. 计算数组的最大值、最小值、平均值和元素总和
    static void CalculateArrayStats(int[] arr, out int max, out int min, out double average, out int sum)
    {
        //补充：out用于多个输出
        max = arr.Max();
        min = arr.Min();
        sum = arr.Sum();
        average = arr.Average();
    }

    // Homework3. 使用“埃氏筛法”求 2~100 以内的所有素数
    static List<int> SieveOfEratosthenes(int limit)
    {
        bool[] isPrime = new bool[limit + 1];
        for (int i = 2; i <= limit; i++)
            isPrime[i] = true;

        for (int i = 2; i * i <= limit; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= limit; j += i)
                    isPrime[j] = false;
            }
        }

        List<int> primes = new List<int>();
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i])
                primes.Add(i);
        }
        return primes;
    }

    // Homework4. 判断矩阵是否为托普利茨矩阵
    static bool IsToeplitzMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int row = 0; row < rows - 1; row++)
        {
            for (int col = 0; col < cols - 1; col++)
            {
                if (matrix[row, col] != matrix[row + 1, col + 1])
                    return false;
            }
        }
        return true;
    }

    static void Main()
    {
        // 测试1
        Console.WriteLine("输入一个整数:");
        int num = int.Parse(Console.ReadLine());
        Console.WriteLine("素数因子: " + string.Join(", ", GetPrimeFactors(num)));

        // 测试2
        int[] arr = { 1, 2, 3, 4, 5 };
        CalculateArrayStats(arr, out int max, out int min, out double avg, out int sum);
        Console.WriteLine($"最大值: {max}, 最小值: {min}, 平均值: {avg}, 总和: {sum}");

        // 测试3
        List<int> primes = SieveOfEratosthenes(100);
        Console.WriteLine("2~100 以内的素数: " + string.Join(", ", primes));

        // 测试4
        int[,] matrix = {
            { 1, 2, 3 },
            { 4, 1, 2 },
            { 5, 4, 1 }
        };
        Console.WriteLine("该矩阵是否为托普利茨矩阵: " + IsToeplitzMatrix(matrix));
    }
}

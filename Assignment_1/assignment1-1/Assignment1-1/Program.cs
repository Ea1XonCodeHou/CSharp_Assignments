using System;
namespace AssignmentApplication
{
    class CalculatorApplication
    {
        static void Main()
        {
            Console.WriteLine("Welcome to use calculator completed by C#!");
            Console.WriteLine("Please input number-1:");
            int operatorA=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please input number-2:");
            int operatorB=Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input Operator!");
            char operatorSign=Convert.ToChar(Console.ReadLine());
            int Result;
            switch (operatorSign)
            {
                case '+':
                    Result = operatorA + operatorB;
                    Console.WriteLine(Convert.ToString(operatorA) + operatorSign + 
                        Convert.ToString(operatorB) + "=" + Convert.ToString(Result));
                    break;
                case '-':
                    Result = operatorA - operatorB;
                    Console.WriteLine(Convert.ToString(operatorA) + operatorSign + 
                        Convert.ToString(operatorB) + "=" + Convert.ToString(Result));
                    break;
                case '*':
                    Result = operatorA * operatorB;
                    Console.WriteLine(Convert.ToString(operatorA) + operatorSign + 
                        Convert.ToString(operatorB) + "=" + Convert.ToString(Result));
                    break;
                case '/':
                    Result = operatorA / operatorB;
                    Console.WriteLine(Convert.ToString(operatorA) + operatorSign + 
                        Convert.ToString(operatorB) + "=" + Convert.ToString(Result));
                    break;
                default:
                    Console.Error.WriteLine("Something Wrong!");
                    break;
            } 
        }
    }
}

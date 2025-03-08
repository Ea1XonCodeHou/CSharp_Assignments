using System;
using System.Collections.Generic;

// 抽象基类 Shape
public abstract class Shape
{
    public abstract double CalculateArea();
    public abstract bool IsValid();
}

// 长方形类 Rectangle
public class Rectangle : Shape
{
    public double Length { get; set; }
    public double Width { get; set; }

    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }

    public override double CalculateArea()
    {
        return Length * Width;
    }

    public override bool IsValid()
    {
        return Length > 0 && Width > 0;
    }
}

// 正方形类 Square
public class Square : Shape
{
    public double Side { get; set; }

    public Square(double side)
    {
        Side = side;
    }

    public override double CalculateArea()
    {
        return Side * Side;
    }

    public override bool IsValid()
    {
        return Side > 0;
    }
}

// 三角形类 Triangle
public class Triangle : Shape
{
    public double SideA { get; set; }
    public double SideB { get; set; }
    public double SideC { get; set; }

    public Triangle(double sideA, double sideB, double sideC)
    {
        SideA = sideA;
        SideB = sideB;
        SideC = sideC;
    }

    public override double CalculateArea()
    {
        // 使用海伦公式计算三角形的面积
        double s = (SideA + SideB + SideC) / 2;
        return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
    }

    public override bool IsValid()
    {
        // 判断三角形是否有效（任意两边之和大于第三边）
        return SideA + SideB > SideC && SideA + SideC > SideB && SideB + SideC > SideA;
    }
}

// 简单工厂类 ShapeFactory
public static class ShapeFactory
{
    private static Random _random = new Random();

    public static Shape CreateShape()
    {
        int shapeType = _random.Next(1, 4); // 随机生成形状类型

        switch (shapeType)
        {
            case 1: // 创建长方形
                return new Rectangle(_random.Next(1, 10), _random.Next(1, 10));
            case 2: // 创建正方形
                return new Square(_random.Next(1, 10));
            case 3: // 创建三角形
                return new Triangle(_random.Next(1, 10), _random.Next(1, 10), _random.Next(1, 10));
            default:
                throw new ArgumentException("Invalid shape type.");
        }
    }
}

public class ProgramSolutionWeek03
{
    public static void Main(string[] args)
    {
        double totalArea = 0;
        List<Shape> shapes = new List<Shape>();

        // 随机生成10个形状对象
        for (int i = 0; i < 10; i++)
        {
            Shape shape = ShapeFactory.CreateShape();
            if (shape.IsValid())
            {
                shapes.Add(shape);
                totalArea += shape.CalculateArea();
            }
        }

        // 输出计算的面积总和
        Console.WriteLine("Total Area of Valid Shapes: " + totalArea);
    }
}

using System;
using System.Net;
namespace Week04CSharpLearning
{
    public class Node<T>
    {
        public Node<T> Next { get; set; }
        public T Data { get; set; }

        public Node(T t)
        {
            this.Next = null;
            this.Data = t;
        }
    }

    public class GenericList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public GenericList()
        {
            tail = head = null;
        }

        public Node<T> Head
        {
            get=> head;
        }

        public void Add(T t)
        {
            Node<T> n=new Node<T>(t);
            if (tail == null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next= n;
                tail = n;
            }
        }

        public void forEach()
        {
            Node<T> n=head;
            while (n != null)
            {
                Console.Write(n.Data + " ");
                n=n.Next;
            }
            //Console.WriteLine(tail.Data);
        }
    }
    class program
    {
        static void Main()
        {
            GenericList<String> list = new GenericList<String>();
            list.Add("Hello");
            list.Add("C#");
            list.Add("Generic");
            list.Add("LinkList");

            list.forEach();
        }
    }
}
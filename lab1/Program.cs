using System;
using static System.Console;
namespace lab1
{
    class MyMath : IDisposable
    {
        protected bool disposed;
        static protected readonly double pi;
        static protected readonly double e;
        static MyMath()
        {
            pi = Math.PI; 
            e = Math.E;
        }
        public virtual void Print() 
        {
            WriteLine("I like to calculate.");
        }
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            CleanUp(true);
            GC.ReRegisterForFinalize(this);
        }
        void CleanUp(bool disposing)
        {
            if(!this.disposed)
            {
                if (disposing)
                {
                    WriteLine("Disposing managed resourses.");
                }
                WriteLine("Disposing unmanaged resourses.");
                disposed = true;
            }
        }
        ~MyMath()
        {
            CleanUp(false);
            WriteLine("MyMath destructed");
        }
    }
    class Calculator : MyMath 
    { 
        private static string createdOn;
        private static Calculator instance = null; 
        private Calculator() 
        { 
            createdOn = DateTime.Now.ToLongTimeString();
        } 
        public static Calculator GetInstance() 
        { 
            if (instance == null) 
            { 
                instance = new Calculator(); 
            } 
            return instance; 
        } 
        public override void Print() 
        {
            base.Print();
            WriteLine($"I'm created at {createdOn}.");
        }
        ~Calculator()
        {
            WriteLine("Calculator destructed");
        }
    }
    class Add : MyMath
    {
        protected int a;
        protected int b;
        public Add(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public Add(int a)
        {
            this.a = a;
            this.b = Convert.ToInt32(pi);
        }
        public Add()
        {
            this.a = Convert.ToInt32(pi);
            this.b = Convert.ToInt32(e);
        }
        private int Calculate()
        {
            return a + b;
        }
        public override void Print()
        {
            WriteLine("Result is: " + Calculate());
        }
        ~Add()
        {
            WriteLine("Add destructed");
        }
    }
    class Sub : Add
    {
        public Sub(int a, int b) : base(a, b)
        {}
        public Sub(int a) : base(a)
        {}
        public Sub() : base()
        {}
        private int Calculate()
        {
            return a - b;
        }
        public override void Print()
        {
            WriteLine("Result is: " + Calculate());
        }
        public override void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }
        void CleanUp(bool disposing)
        {
            if(!this.disposed)
            {
                if (disposing)
                {}
                disposed = true;
            }
        }
        ~Sub()
        {
            CleanUp(false);
            WriteLine("Sub destructed");
        }
    }
    class SquereRoot : MyMath
    {
        protected double a;
        public double A 
        {
            get
            {
                if(this.a != default) return Math.Sqrt(a);
                else return 0;
            }
            set 
            {
                if (value > 0)
                {
                    this.a = value;
                }
            }
        }
        ~SquereRoot()
        {
            WriteLine("SquereRoot destructed");
        }
    } 
    class Root : SquereRoot
    {
        private double b;
        public double B 
        {
            get
            {
                if(this.b != default) return Math.Pow(a, 1/b);
                else return 0;
            }
            set 
            {
                if (value > 0)
                {
                    this.b = value;
                }
            }
        }
        ~Root()
        {
            WriteLine("Root destructed");
        }    
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc1 = Calculator.GetInstance();
            Calculator calc2 = Calculator.GetInstance();
            calc1.Print();
            calc2.Print();
            WriteLine(calc1 == calc2);
            WriteLine("");

            Add add1 = new Add(45, 76);
            add1.Print();
            Add add2 = new Add(42);
            add2.Print();
            Add add3 = new Add();
            add3.Print();
            WriteLine("");

            Sub sub = new Sub(42);
            sub.Print();
            WriteLine("");

            SquereRoot sqrt = new SquereRoot();
            sqrt.A = 9;
            sqrt.A = -12;
            WriteLine("Squere root: " + sqrt.A);
            WriteLine("");

            Root rt = new Root();
            rt.A = 16;
            rt.B = 4;
            WriteLine("Root: " + rt.B);
            WriteLine("");

            WriteLine("Memory before collect: " + GC.GetTotalMemory(false));
            WriteLine("Generation: " + GC.GetGeneration(sub));
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            WriteLine("Memory after collect: " + GC.GetTotalMemory(false));
            WriteLine("Generation: " + GC.GetGeneration(sub));
            WriteLine("");

            MyMath math = new MyMath();
            math.Dispose();

            WriteLine("");
            for(int i = 0; i < 50000; i++)
            {
                new Sub(i);
            } 
        }
    }
}
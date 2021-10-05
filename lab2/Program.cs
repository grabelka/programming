using System;
using static System.Console;

namespace lab2
{
    delegate void CalcHandle(Calculator calculator, CalcEventArgs args);
    class CalcEventArgs : EventArgs
    {
        public string name;
        public int skills;
        public CalcEventArgs(string name, int skills)
        {
            this.name = name;
            this.skills = skills;
        }
    }
    interface IMath
    {
        void Print();
        void Calculate();
        void GetConsts();
    }
    interface IRoot
    {
        double GetNum();
        double PowNum();
        void CreateCalc(Calculator calculator, CalcEventArgs args);
    }
    abstract class MyMath : IMath, IDisposable
    {
        protected bool disposed;
        static protected readonly double pi;
        static protected readonly double e;
        static MyMath()
        {
            pi = Math.PI; 
            e = Math.E;
        }
        public void GetConsts()
        {
            WriteLine($"pi is {pi}, e is {e}.");
        }
        void IMath.GetConsts()
        {
            WriteLine("I know no constants");
        }
        public abstract void Print();
        public abstract void Calculate();
        public abstract void Dispose();
        protected void CleanUp(bool disposing)
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
        public event CalcHandle CalculatorEvent;
        private static string createdOn;
        public Calculator() 
        { 
            createdOn = DateTime.Now.ToLongTimeString();
        } 
        public override void Print() 
        {
            WriteLine($"I'm created at {createdOn}.");
        }
        public override void Calculate() 
        {
            CalcEventArgs args = new CalcEventArgs("Bimo", 10);
            if(CalculatorEvent != null) CalculatorEvent(this, args);
        }
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            CleanUp(true);
            GC.ReRegisterForFinalize(this);
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
        private int Calc()
        {
            return a + b;
        }
        public override void Calculate() 
        {
            WriteLine($"{a} + {b} = " + Calc());
        }
        public override void Print()
        {
            WriteLine("Result is: " + Calc());
        }
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            CleanUp(true);
            GC.ReRegisterForFinalize(this);
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
        public int Calc()
        {
            return a - b;
        }
        public override void Calculate() 
        {
            WriteLine($"{a} - {b} = " + Calc());
        }public override void Print()
        {
            WriteLine("Result is: " + Calc());
        }
        public override void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }
        ~Sub()
        {
            CleanUp(false);
            WriteLine("Sub destructed");
        }
    }
    class SquereRoot : MyMath, IRoot
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
        public double GetNum()
        {
            return a;
        }
        public virtual double PowNum()
        {
            return 2;
        }
        public virtual void CreateCalc(Calculator calculator, CalcEventArgs args)
        {
            if(args.skills >= 10)
            {
                WriteLine($"{args.name} can calculate. My skills is {args.skills}");
                calculator.GetConsts();
            }
            else
            {
                WriteLine($"{args.name} can't calculate. My skills is {args.skills}, but must be bigger than 10");
                calculator.Print();
            }
        }
        public override void Calculate() 
        {
            WriteLine(A);
        }
        public override void Print()
        {
            WriteLine($"Squere root of {a} is {A}");
        }
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            CleanUp(true);
            GC.ReRegisterForFinalize(this);
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
        public override double PowNum()
        {
            return b;
        }
        public override void Calculate() 
        {
            WriteLine(B);
        }
        public override void CreateCalc(Calculator calculator, CalcEventArgs args)
        {
            if(args.skills >= 15)
            {
                WriteLine($"{args.name} can calculate. My skills is {args.skills}");
                calculator.GetConsts();
            }
            else
            {
                WriteLine($"{args.name} can't calculate. My skills is {args.skills}, but must be bigger than 15");
                calculator.Print();
            } 
        }
        public override void Print()
        {
            WriteLine($"Root of {a} is {B}");
        }
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            CleanUp(true);
            GC.ReRegisterForFinalize(this);
        }
        ~Root()
        {
            WriteLine("Root destructed");
        }    
    }
    class Space 
    {
        IRoot[] roots;
        public Space(Calculator calculator)
        {
            roots = new IRoot[2];
            roots[0] = new SquereRoot();
            roots[1] = new Root();
            foreach(IRoot r in roots)
            {
                calculator.CalculatorEvent += new CalcHandle(r.CreateCalc);
            }
        }
    }
    class Person
    {
        public string name;
        public int age;
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
        public void Drink()
        {
            if(age < 18) throw new PersonException(this);
            WriteLine($"{this.name} drink vodka.");
        }
    }
    class PersonException : Exception
    {
        public Person args;
        public PersonException(Person args) : base()
        {
            this.args = args;
        }
        public override string Message => $"Person {args.name} is too young. Come back in {18 - args.age}";
    }
    static class MyExtentions
    {
        public static void Announce(this Sub sub)
        {
            WriteLine("This is extention method");
        }
        public static int SubTwice(this Sub sub, int c)
        {
            return sub.Calc() - c;
        }
        public static int SubAdd(this Sub sub, int c)
        {
            return sub.Calc() + c;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Add add = new Add(42);
            ((IMath)add).GetConsts();
            add.GetConsts();
            WriteLine("");

            Calculator calculator = new Calculator();
            Space space = new Space(calculator);
            calculator.Calculate();
            WriteLine("");

            CalcEventArgs calcArgs = new CalcEventArgs("Pipi", 17);
            CalcHandle calcHandle = delegate(Calculator calculator, CalcEventArgs calcAgs)
            {
                if(calcArgs.skills >= 15)
                {
                    WriteLine($"{calcArgs.name} can calculate. My skills is {calcArgs.skills}");
                    calculator.GetConsts();
                }
                else
                {
                    WriteLine($"{calcArgs.name} can't calculate. My skills is {calcArgs.skills}, but must be bigger than 15");
                    calculator.Print();
                }
            };
            calcHandle(calculator, calcArgs);
            WriteLine("");

            Action<string> line = name => Console.WriteLine("{0} invoked an action", name);
            Func<int, int, string> result = (a, b) => $"{a} + {b} is {a+b}";
            line("Anastasia");
            WriteLine(result(3,8));
            WriteLine("");
        
            Sub sub = new Sub(15, 7);
            sub.Announce();
            WriteLine(sub.SubTwice(2));
            WriteLine(sub.SubAdd(2));
            WriteLine("");

            Person p1 = new Person("John", 16);
            try
            {
                p1.Drink();
            }
            catch(PersonException e)
            {
                WriteLine(e.Message);
            }
            finally
            {
                Person p2 = new Person("Mike", 19);
                p2.Drink();
            }
        }
    }
}

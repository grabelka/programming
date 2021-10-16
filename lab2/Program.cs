using System;
using static System.Console;

namespace lab2
{
    delegate void PacmanHandle(Pacman pacman, PacmanEventArgs args);
    class PacmanEventArgs : EventArgs
    {
        public int seconds;
        public PacmanEventArgs(int seconds)
        {
            this.seconds = seconds;
        }
    }
    abstract class AbstrackPacman
    {
        public string color;
        protected int skils;
        public abstract void Eat(IFruit fruit);
        public abstract void Info();
    }
    interface IFruit
    {
        int GetPower();
        void Disappear();
        void WasEaten(Pacman pacman, PacmanEventArgs args);
    }
    interface ICoords
    {
        int GetX();
        int GetY();
    }
    class Pacman : AbstrackPacman, ICoords
    {
        public event PacmanHandle PacmanEatEvent;
        int x;
        int y;
        public Pacman(string color, int x, int y)
        {
            this.color = color;
            this.skils = 0;
            this.x = x;
            this.y = y;
        }
        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public override void Eat(IFruit fruit)
        {
            skils+=fruit.GetPower();
            PacmanEventArgs args = new PacmanEventArgs(13);
            PacmanEatEvent((Pacman)this, args);
        }
        public override void Info()
        {
            WriteLine($"Packman has {color} color and skills {skils}");
        }
    }
    class Apple : IFruit, ICoords
    {
        protected int power;
        protected int x;
        protected int y;
        public Apple(int x, int y)
        {
            this.power = 10;
            this.x = x;
            this.y = y;
        }
        public virtual void WasEaten(Pacman pacman, PacmanEventArgs args)
        {
            if (args.seconds < 10)
                Console.WriteLine("Apple was eaten very quickly by " + pacman.color + " pacman.");
            else
                Console.WriteLine("Apple wasn't eaten very quickly by " + pacman.color + " pacman.");
        }
        public int GetPower()
        {
            return this.power;
        }
        public int GetX()
        {
            return this.x;
        }
        public int GetY()
        {
            return this.y;
        }
        public virtual void Disappear()
        {
            WriteLine($"Apple with power {power} was disappeared in coordinates ({x}, {y}).");
        }
        void IFruit.Disappear()
        {
            WriteLine("Some fruit was disappeared.");
        }
    }
    class Peach : Apple
    {
        public Peach(int x, int y) : base(x, y)
        {
            this.power = 30;
        }
        public override void WasEaten(Pacman pacman, PacmanEventArgs args)
        {
            if (args.seconds < 20)
                Console.WriteLine("Peach was eaten very quickly by " + pacman.color + " pacman.");
            else
                Console.WriteLine("Peach wasn't eaten very quickly by " + pacman.color + " pacman.");
        }
        public override void Disappear()
        {
            WriteLine($"Peach with power {power} was disappeared in coordinates ({x}, {y}).");
        }
    }
    class Game
    {
        IFruit[] fruits;
        public Game(Pacman pacman)
        {
            fruits = new IFruit[2];
            fruits[0] = new Apple(3, 7);
            fruits[1] = new Peach(1, 2);
            foreach (IFruit f in fruits)
                pacman.PacmanEatEvent += new PacmanHandle(f.WasEaten);
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
            WriteLine($"{this.name} drinks vodka.");
        }
    }
    class PersonException : Exception
    {
        public Person args;
        public PersonException(Person args) : base()
        {
            this.args = args;
        }
        public override string Message => $"{args.name} is too young. Come back in {18 - args.age} years.";
    }
    static class MyExtentions
    {
        public static double VectorLength(this Pacman pacman)
        {
            return Math.Sqrt(pacman.GetX()*pacman.GetX()+pacman.GetY()*pacman.GetY());
        }
        public static void GetCoords(this Pacman pacman)
        {
            WriteLine($"This is an extention method. Pacman has coordinates ({pacman.GetX()}, {pacman.GetY()}).");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //1
            Apple a = new Apple(4, 2);
            ((IFruit)a).Disappear();
            a.Disappear();
            Peach p = new Peach(7, 8);
            ((IFruit)p).Disappear();
            p.Disappear();
            WriteLine("");

            //2-4
            Pacman pacman = new Pacman("red", 7, 5);
            Game game = new Game(pacman);
            pacman.Eat(a);
            WriteLine("");

            //4
            PacmanEventArgs pacmanArgs = new PacmanEventArgs(30);
            PacmanHandle pacmanHandle = delegate(Pacman pacman, PacmanEventArgs pacmanArgs)
            {
                if (pacmanArgs.seconds < 50)
                    Console.WriteLine("Anonymous method by " + pacman.color + " pacman was very quickly.");
                else
                    Console.WriteLine("Anonymous method by " + pacman.color + " pacman wasn't very quickly.");
            };
            pacmanHandle(pacman, pacmanArgs);
            WriteLine("");

            Action<string> line = name => Console.WriteLine("{0} invoked an action", name);
            Func<int, int, string> result = (a, b) => $"{a} + {b} is {a+b}";
            line("Anastasia");
            WriteLine(result(3,8));
            WriteLine("");

            //5
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
            WriteLine("");

            //6
            pacman.GetCoords();
            WriteLine(pacman.VectorLength());
        }
    }
}

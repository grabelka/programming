using System;
using static System.Console;

namespace lab1
{
    abstract class Human
    {
        public void Speak() {}
    }
    class StupidHuman : Human
    {

    }
    class NormalHuman : StupidHuman
    {
        
    }
    class CleverHuman : NormalHuman
    {
        
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}

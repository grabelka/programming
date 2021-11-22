using System;

namespace lab5
{
    class Program
    {
        //task1
        abstract class State
        {
            protected Time time;
            public State(Time time)
            {
                this.time = time;
            }
            public abstract void Move();
        }

        class Stairs : State
        {
            public Stairs(Time time) : base(time)
            {}
            public override void Move()
            {
                Console.WriteLine($"Move by stairs. Time is {time.Hours}:{time.Minutes}, lift and escalator is closed.");
            }
        }
        class Lift : State
        {
            public Lift(Time time) : base(time)
            {}
            public override void Move()
            {
                Console.WriteLine($"Move by lift. Time is {time.Hours}:{time.Minutes}, escalator is closed.");
            }
        }
        class Escalator : State
        {public Escalator(Time time) : base(time)
            {}
            public override void Move()
            {
                Console.WriteLine($"Move by escalator. Time is {time.Hours}:{time.Minutes}, lift is closed.");
            }
        }
        class Context
        {
            State state;
            Time time;
            public Context(Time time)
            {
                this.time = time;
                if(time.Hours <= 10 || time.Hours >= 22) state = new Stairs(time);
                if(time.Hours > 10 && time.Hours < 14) state = new Lift(time);
                if(time.Hours >= 14 && time.Hours < 22) state = new Escalator(time);
            }
            public void MovePeople()
            {
                this.state.Move();
            }
        }

        //task2
        abstract class Approver
        {
            protected Approver successor;

            public void SetSuccessor(Approver successor)
            {
                this.successor = successor;
            }

            public abstract void ProcessRequest(Time time);
        }
        class FirstShift : Approver
        {
            public override void ProcessRequest(Time time)
            {
                if (time.Hours >= 6 && time.Hours < 14)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request. This request came at {time.Hours}:{time.Minutes}.");
                }
                else if (successor != null)
                {
                    successor.ProcessRequest(time);
                }
            }
        }
        class SecondShift : Approver
        {
            public override void ProcessRequest(Time time)
            {
                if (time.Hours >= 14 && time.Hours < 22)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request. This request came at {time.Hours}:{time.Minutes}.");
                }
                else if (successor != null)
                {
                    successor.ProcessRequest(time);
                }
            }
        }
        class ThirdShift : Approver
        {
            public override void ProcessRequest(Time time)
            {
                if (time.Hours >= 22 || time.Hours < 6)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request. This request came at {time.Hours}:{time.Minutes}.");
                }
                else if (successor != null)
                {
                    successor.ProcessRequest(time);
                }
            }
        }
        class Time
        {
            public int Hours {get; private set;}
            public int Minutes {get; private set;}
            public Time(int hours, int minutes)
            {
                Hours = hours;
                Minutes = minutes;
            }
        }
        static void Main(string[] args)
        {
            Time time1 = new Time(4, 20);
            Time time2 = new Time(11, 54);
            Time time3 = new Time(19, 12);
            Time time4 = new Time(16, 55);
            //task1
            Context context1 = new Context(time1);
            Context context2 = new Context(time2);
            Context context3 = new Context(time3);
            context1.MovePeople();
            context2.MovePeople();
            context3.MovePeople();

            Console.WriteLine();

            //task2
            Approver firstShift = new FirstShift();
            Approver secondShift = new SecondShift();
            Approver thirdShift = new ThirdShift();
            firstShift.SetSuccessor(secondShift);
            secondShift.SetSuccessor(thirdShift);
            thirdShift.SetSuccessor(firstShift);
            firstShift.ProcessRequest(time1);
            secondShift.ProcessRequest(time3);
            thirdShift.ProcessRequest(time4);
        }
    }
}

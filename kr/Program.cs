//https://docs.google.com/document/d/1obQE9d0rhFduVgo6JWLfWsSGG-dlyrcmzoQOQ7Oc8d8/edit#

using System;
using System.Drawing;


class Program
{
    static void Main(string[] args)
    {
        Originator originator = new Originator(null);
        Caretaker caretaker = new Caretaker(originator);
        caretaker.Backup(new Bitmap("img.jpg"));

        Context context = new Context();
        bool cont = true;
        while(cont)
        {
            Console.WriteLine("Print module name, input file and command:");
            string[] command = Console.ReadLine().Split(' ');
            if(command.Length > 0 && !String.IsNullOrEmpty(command[0])) 
            {
                if(command[0] == "exit") cont = false;
                else if(command[0] == "undo") 
                {
                    caretaker.Undo();
                }
                else
                {
                    if (command[0] == "fast")
                    {
                        context.SetStrategy(Fast.GetInstance());

                    }
                    else if (command[0] == "pixel")
                    {
                        context.SetStrategy(Pixel.GetInstance());
                    }
                    else
                    {
                        Console.Error.WriteLine($"Module '{args[0]}' doesn't exist.");
                        break;
                    }
                    if(command.Length > 1) 
                    {
                        Approver fileNotExists = new FileNotExists();
                        Approver fileExists = new FileExists();
                        fileNotExists.SetSuccessor(fileExists);
                        fileExists.SetSuccessor(fileNotExists);
                        fileNotExists.ProcessRequest(caretaker, command[1], context, command);
                    }
                }
            }
        }
    }
}

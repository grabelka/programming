using System;

class FileNotExists : Approver
{
    public override void ProcessRequest(Caretaker caretaker, string file, Context context, string[] command)
    {
        if (!System.IO.File.Exists(file))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("File does not exist.");
        }
        else if (successor != null)
        {
            successor.ProcessRequest(caretaker, file, context, command);
        }
    }
}
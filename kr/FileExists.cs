using System;

class FileExists : Approver
{
    public override void ProcessRequest(Caretaker caretaker, string file, Context context, string[] command)
    {
        if (System.IO.File.Exists(file))
        {
            switch (command[2])
            {
                case "rotate":
                    context.Rotate(caretaker, command[1]);
                    break;
                case "grayscale":
                    context.Grayscale(caretaker, command[1]);
                    break;
                case "invertcolors":
                    context.InvertColors(caretaker, command[1]);
                    break;
                case "crop":
                    if (command.Length != 4)
                    {
                        Environment.ExitCode = 1;
                        Console.Error.WriteLine("Wrong input command.");
                        return;
                    }
                    context.Crop(caretaker, command[1], command[3]);
                    break;
                case "changehue":
                    if (command.Length != 4 || !Int32.TryParse(command[3], out int hue))
                    {
                        Environment.ExitCode = 1;
                        Console.Error.WriteLine("Wrong input command.");
                        return;
                    }
                    if (hue < -180 || hue > 180) 
                    {
                        Environment.ExitCode = 1;
                        Console.Error.WriteLine("Hue must be less than 180 and bigger than -180.");
                        return;
                    }
                    context.ChangeHue(caretaker, command[1], hue);
                    break;
                default:
                    Environment.ExitCode = 1;
                    Console.Error.WriteLine($"Command '{command[2]}' doesn't exist.");
                    return;
            }
        }
        else if (successor != null)
        {
            successor.ProcessRequest(caretaker, file, context, command);
        }
    }
}
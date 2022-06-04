public interface IModule
{
    public void Cropp(Caretaker caretaker, string inputFile, string outputFile, string arg);
    public void RotateRight90(Caretaker caretaker, string inputFile, string outputFile);
    public void Grayscale(Caretaker caretaker, string inputFile, string outputFile);
    public void InvertColors(Caretaker caretaker, string inputFile, string outputFile);
    public void ChangeHue(Caretaker caretaker, string inputFile, string outputFile, int hue);
}
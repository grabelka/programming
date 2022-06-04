public interface ITarget
{
    public void ChangeHue(Caretaker caretaker, string img, int hue);
    public void Rotate(Caretaker caretaker, string img);
    public void Crop(Caretaker caretaker, string img, string paramethers);
    public void Grayscale(Caretaker caretaker, string img);
    public void InvertColors(Caretaker caretaker, string img);
}
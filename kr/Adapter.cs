class Adapter : ITarget
{
    private readonly IModule adaptee;

    public Adapter(IModule adaptee)
    {
        this.adaptee = adaptee;
    }
    public void ChangeHue(Caretaker caretaker, string img, int hue)
    {
        adaptee.ChangeHue(caretaker, img, "result.jpg", hue);
    }
    public void Rotate(Caretaker caretaker, string img)
    {
        adaptee.RotateRight90(caretaker, img, "result.jpg");
    }
    public void Crop(Caretaker caretaker, string img, string paramethers)
    {
        adaptee.Cropp(caretaker, img, "result.jpg", paramethers);
    }
    public void Grayscale(Caretaker caretaker, string img)
    {
       adaptee.Grayscale(caretaker, img, "result.jpg");
    }
    public void InvertColors(Caretaker caretaker, string img)
    {
        adaptee.InvertColors(caretaker, img, "result.jpg");
    }
}
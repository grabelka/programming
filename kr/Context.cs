class Context
{
    private IModule module;
    private ITarget target;
    public Context() { }
    public Context(IModule module)
    {
        this.module = module;
        this.target = new Adapter(module);
    }

    public void SetStrategy(IModule module)
    {
        this.module = module;
        this.target = new Adapter(module);
    }
    public void ChangeHue(Caretaker caretaker, string img, int hue)
    {
        this.target.ChangeHue(caretaker, img, hue);
    }
    public void Rotate(Caretaker caretaker, string img)
    {
        this.target.Rotate(caretaker, img);
    }
    public void Crop(Caretaker caretaker, string img, string paramethers)
    {
        this.target.Crop(caretaker, img, paramethers);
    }
    public void Grayscale(Caretaker caretaker, string img)
    {
        this.target.Grayscale(caretaker, img);
    }
    public void InvertColors(Caretaker caretaker, string img)
    {
        this.target.InvertColors(caretaker, img);
    }
}

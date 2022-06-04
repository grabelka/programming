using System;
using System.Drawing;

public class Originator
{
    private Bitmap _state;

    public Originator(Bitmap state)
    {
        this._state = state;
    }
    public void ChangeState(Bitmap bmp)
    {
        this._state = bmp;
    }
    public IMemento Save()
    {
        return new Memento(this._state);
    }
    public void Restore(IMemento memento)
    {
        if (!(memento is Memento))
        {
            throw new Exception("Unknown memento class " + memento.ToString());
        }
        this._state = memento.GetState();
    }
}
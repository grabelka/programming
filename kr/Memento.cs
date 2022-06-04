using System;
using System.Drawing;

public class Memento : IMemento
{
    private Bitmap _state;

    private DateTime _date;

    public Memento(Bitmap state)
    {
        this._state = state;
        this._date = DateTime.Now;
    }
    public Bitmap GetState()
    {
        return this._state;
    }
    public DateTime GetDate()
    {
        return this._date;
    }
}
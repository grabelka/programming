using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

public class Caretaker
{
    private List<IMemento> _mementos = new List<IMemento>();

    private Originator _originator;

    public Caretaker(Originator originator)
    {
        this._originator = originator;
    }
    public void Backup(Bitmap bmp)
    {
        this._mementos.Add(this._originator.Save());
        _originator.ChangeState(bmp);
    }
    public void Undo()
    {
        if (this._mementos.Count == 0)
        {
            Console.WriteLine("Image wasn't restored. There is no backups.");
            return;
        }
        var memento = this._mementos.Last();
        this._mementos.Remove(memento);
        try
        {
            this._originator.Restore(memento);
            Console.WriteLine("Image was restored.");
            memento.GetState().Save("result.jpg");
        }
        catch (Exception)
        {
            this.Undo();
        }
    }
}
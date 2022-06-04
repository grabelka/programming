using System;
using System.Drawing;

public interface IMemento
{
    Bitmap GetState();
    DateTime GetDate();
}
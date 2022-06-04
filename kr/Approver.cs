using System;

abstract class Approver
{
    protected Approver successor;

    public void SetSuccessor(Approver successor)
    {
        this.successor = successor;
    }

    public abstract void ProcessRequest(Caretaker caretaker, string file, Context context, string[] command);
}
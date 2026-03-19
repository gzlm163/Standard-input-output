using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Caretaker
{
    private object memento;
    public void SaveState(IOriginator originator)
    {
        memento = originator.GetMemento();
    }

    public void RestoreState(IOriginator originator)
    {
        originator.SetMemento(memento);
    }
}
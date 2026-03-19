using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Memento {
  public string fileContent { get; set; }

}
public interface IOriginator {
  object GetMemento();
  void SetMemento(object memento);
}

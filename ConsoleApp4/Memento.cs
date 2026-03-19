using System;

class Memento {
  public string fileContent { get; set; }

}
public interface IOriginator {
  object GetMemento();
  void SetMemento(object memento);
}

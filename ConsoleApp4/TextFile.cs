using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class TextFile : IOriginator {
  public string FilePath;
  public string FileContent;
  public TextFile(string path) {
    FilePath = path;
    FileContent = "";
  }
  public void ReadFromFile() {
    try {
      FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read);
      StreamReader reader = new StreamReader(file);
      FileContent = reader.ReadToEnd();
      reader.Close();
    }
    catch {
      Console.WriteLine(" Error reading file. ");

      FileContent = "";
    }
  }

  public void BinarySerialize(FileStream fileStream) {
    BinaryFormatter formatter = new BinaryFormatter();
    formatter.Serialize(fileStream, this);
    fileStream.Flush();
    fileStream.Close();
  }

  public void BinaryDeserialize(FileStream fileStream) {
    BinaryFormatter formatter = new BinaryFormatter();
    TextFile deserialized = (TextFile)formatter.Deserialize(fileStream);
    FilePath = deserialized.FilePath;
    FileContent = deserialized.FileContent;
    fileStream.Close();
  }

  public void XmlSerialize(FileStream fileStream) {
    XmlSerializer xml = new XmlSerializer(this.GetType());
    xml.Serialize(fileStream, this);
    fileStream.Flush();
    fileStream.Close();
  }

  public void XmlDeserialize(FileStream fileStream) {
    XmlSerializer xml = new XmlSerializer(this.GetType());
    TextFile deserialized = (TextFile)xml.Deserialize(fileStream);
    FilePath = deserialized.FilePath;
    FileContent = deserialized.FileContent;
    fileStream.Close();
  }

  object IOriginator.GetMemento() {
    return new Memento { fileContent = this.FileContent };
  }
  void IOriginator.SetMemento(object memento) {
    if (memento is Memento) {
      Memento mem = memento as Memento;
      FileContent = mem.fileContent;
    }
  }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

[Serializable]
public class TextFile
{
    public string filePath;
    public string fileContent;
    public TextFile(string path)
    {
        filePath = path;
        fileContent = "";
    }
    public void ReadFromFile()
    {
        try
        {
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            fileContent = reader.ReadToEnd();
            reader.Close();
        }
        catch
        {
            Console.WriteLine(" Error reading file. ");
            fileContent = "";
        }
    }

    public void SaveToFile()
    {
        FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(file);
        writer.Write(fileContent);
        writer.Close();
    }

    public void BinarySerialize(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, this);
        fs.Flush();
        fs.Close();
    }

    public void BinaryDeserialize(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        TextFile deserialized = (TextFile)bf.Deserialize(fs);
        filePath = deserialized.filePath;
        fileContent = deserialized.fileContent;
        fs.Close();
    }

    public void XmlSerialize(FileStream fs)
    {
        XmlSerializer xml = new XmlSerializer(this.GetType());
        xml.Serialize(fs, this);
        fs.Flush();
        fs.Close();
    }

    public void XmlDeserialize(FileStream fs)
    {
        XmlSerializer xml = new XmlSerializer(this.GetType());
        TextFile deserialized = (TextFile)xml.Deserialize(fs);
        filePath = deserialized.filePath;
        fileContent = deserialized.fileContent;
        fs.Close();
    }
}
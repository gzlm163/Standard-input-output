using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class FileSearch {
  public List<string> Search(List<TextFile> files, string[] keywords) {
    List<string> result = new List<string>();

    for (int fileIndex = 0; fileIndex < files.Count; ++fileIndex) {
      TextFile file = files[fileIndex];
      file.ReadFromFile();

      bool wordFound = false;

      for (int wordIndex = 0; wordIndex < keywords.Length; ++wordIndex) {
        if (file.FileContent.Contains(keywords[wordIndex])) {
          wordFound = true;
          break;
        }
      }

      if (wordFound) {
        string fullPath = file.FilePath;
        string[] pathParts = fullPath.Split('\\');
        string fileName = pathParts[pathParts.Length - 1];
        result.Add(fileName);
      }
    }

    return result;
  }
  public Dictionary<string, List<string>> BuildIndex(List<TextFile> files, string[] keywords) {

    Dictionary<string, List<string>> index = new Dictionary<string, List<string>>();

    for (int wordIndex = 0; wordIndex < keywords.Length; ++wordIndex) {
      index[keywords[wordIndex]] = new List<string>();
    }

    for (int fileIndex = 0; fileIndex < files.Count; ++fileIndex) {
      TextFile file = files[fileIndex];
      file.ReadFromFile();

      string[] pathParts = file.FilePath.Split('\\');
      string fileName = pathParts[pathParts.Length - 1];

      for (int wordIndex = 0; wordIndex < keywords.Length; ++wordIndex) {
        string currentWord = keywords[wordIndex];

        if (file.FileContent.Contains(currentWord)) {
          if (!index[currentWord].Contains(fileName)) {
            index[currentWord].Add(fileName);
          }
        }
      }
    }

    return index;
  }
}

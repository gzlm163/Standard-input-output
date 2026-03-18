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
        if (file.fileContent.Contains(keywords[wordIndex])) {
          wordFound = true;
          break;
        }
      }

      if (wordFound) {
        string fullPath = file.filePath;
        string[] pathParts = fullPath.Split('\\');
        string fileName = pathParts[pathParts.Length - 1];
        result.Add(fileName);
      }
    }

    return result;
  }
}
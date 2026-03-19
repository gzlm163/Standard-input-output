using System;
using System.Collections.Generic;
using System.IO;

class Program {
  static void Main(string[] args) {
    while (true) {
      Console.WriteLine(" \n--- MAIN MENU --- ");
      Console.WriteLine(" 1. Text editor ");
      Console.WriteLine(" 2. File indexer ");
      Console.WriteLine(" 3. Search files ");
      Console.WriteLine(" 4. Exit ");
      Console.Write(" Choose action: ");

      switch (Console.ReadLine()) {
        case "1": {
            Console.WriteLine(" Enter file path: ");
            string path = Console.ReadLine();

            TextFile file = new TextFile(path);

            Console.WriteLine(" Choose format for this session: ");
            Console.WriteLine(" 1 - text ");
            Console.WriteLine(" 2 - binary ");
            Console.WriteLine(" 3 - XML ");
            Console.Write(" Choose: ");

            try {
              switch (Console.ReadLine()) {
                case "1":
                  file.ReadFromFile();
                  Console.WriteLine(" File loaded as plain text. ");
                  break;
                case "2": {
                    FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    file.BinaryDeserialize(fileStream);
                    fileStream.Close();
                    Console.WriteLine(" File loaded from binary format. ");
                    break;
                  }
                case "3": {
                    FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    file.XmlDeserialize(fileStream);
                    fileStream.Close();
                    Console.WriteLine(" File loaded from XML format. ");
                    break;
                  }
              }
            }
            catch {
              Console.WriteLine(" Error loading file. Starting with empty content. ");
              file.FileContent = "";
            }

            Caretaker caretaker = new Caretaker();
            bool exitEditor = false;

            while (!exitEditor) {
              Console.WriteLine(" \n--- EDITOR MENU --- ");
              Console.WriteLine(" 1. Show content ");
              Console.WriteLine(" 2. Undo last addition ");
              Console.WriteLine(" 3. Add lines ");
              Console.WriteLine(" 4. Save ");
              Console.WriteLine(" 5. Back to main menu ");
              Console.Write(" Choose action: ");

              switch (Console.ReadLine()) {
                case "1":
                  Console.WriteLine(" \n--- File content --- ");
                  Console.WriteLine(file.FileContent);
                  Console.WriteLine(" --------------------- ");
                  break;

                case "2":
                  caretaker.RestoreState(file);
                  Console.WriteLine(" Last addition undone. ");
                  break;

                case "3":
                  while (true) {
                    caretaker.SaveState(file);

                    Console.WriteLine(" \n--- ADD MODE --- ");
                    Console.WriteLine(" Enter text to add: ");
                    string input = Console.ReadLine();
                    file.FileContent += input + "\n";
                    Console.WriteLine(" Line added. ");

                    Console.WriteLine(" 1. Add another line ");
                    Console.WriteLine(" 2. Back to editor menu ");
                    Console.Write(" Choose: ");

                    if (Console.ReadLine() == "2") {
                      break;
                    }
                  }

                  break;

                case "4":
                  switch (Console.ReadLine()) {
                    case "1":
                      File.WriteAllText(path, file.FileContent);
                      Console.WriteLine(" File saved as plain text. ");
                      break;
                    case "2": {
                        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                        file.BinarySerialize(fs);
                        fs.Close();
                        Console.WriteLine(" File saved in binary format. ");
                        break;
                      }
                    case "3": {
                        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                        file.XmlSerialize(fs);
                        fs.Close();
                        Console.WriteLine(" File saved in XML format. ");
                        break;
                      }
                  }
                  break;

                case "5":
                  exitEditor = true;
                  break;

                default:
                  Console.WriteLine(" Invalid choice. Try again. ");
                  break;
              }
            }
        }
        break;

        case "2": {
            Console.WriteLine(" Enter folder path: ");
            string folderPath = Console.ReadLine();

            Console.WriteLine(" Enter keywords (separated by space): ");
            string keywordsInput = Console.ReadLine();
            string[] keywords = keywordsInput.Split(' ');

            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            List<TextFile> files = new List<TextFile>();
            for (int fileIndex = 0; fileIndex < filePaths.Length; ++fileIndex) {
              files.Add(new TextFile(filePaths[fileIndex]));
            }

            FileSearch searcher = new FileSearch();
            Dictionary<string, List<string>> index = searcher.BuildIndex(files, keywords);

            Console.WriteLine(" \n--- INDEX --- ");
            for (int wordIndex = 0; wordIndex < keywords.Length; ++wordIndex) {
              string word = keywords[wordIndex];
              Console.Write($" {word}: ");

              if (index.ContainsKey(word) && index[word].Count > 0) {
                for (int fileListIndex = 0; fileListIndex < index[word].Count; ++fileListIndex) {

                if (fileListIndex > 0) {
                  Console.Write(", ");
                }

                Console.Write(index[word][fileListIndex]);
                }
                Console.WriteLine();
              }
              else {
                Console.WriteLine(" not found ");
              }
            }

            break;
          }

        case "3": {
            Console.WriteLine(" Enter folder path: ");
            string folderPath = Console.ReadLine();

            Console.WriteLine(" Enter keywords (separated by space): ");
            string keywordsInput = Console.ReadLine();
            string[] keywords = keywordsInput.Split(' ');
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            List<TextFile> files = new List<TextFile>();
            for (int fileIndex = 0; fileIndex < filePaths.Length; ++fileIndex) {
              files.Add(new TextFile(filePaths[fileIndex]));
            }

            FileSearch searcher = new FileSearch();
            List<string> result = searcher.Search(files, keywords);

            Console.WriteLine(" \n--- SEARCH RESULTS --- ");
            if (result.Count > 0) {
              for (int resultIndex = 0; resultIndex < result.Count; ++resultIndex) {
                Console.WriteLine(result[resultIndex]);
              }
            }
            else {
              Console.WriteLine(" No files found. ");
            }

            break;
          }

        case "4":
          Console.WriteLine(" Exit. ");
          return;

        default:
          Console.WriteLine(" Invalid choice. Try again. ");
          break;
      }
    }
  }
}
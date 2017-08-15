using System;
using System.IO;

namespace TagsCloudContainer
{
    public class FileTextLoader :ITextLoader
    {
        string fileName;
        public FileTextLoader(string fileName)
        {
            this.fileName = fileName;
        }
        public string LoadText()
        {
            var text = File.ReadAllText(fileName);
            return text;
        }
    }
}
using System;
using System.IO;

namespace TagsCloudContainer
{
    public class TxtFileTextLoader : ITextLoader
    {
        private readonly FileFormat format;
        private string fileName;
        
        public string LoadText(string fileName)
        {
            var text = File.ReadAllText(fileName);
            return text;
        }
    }
}
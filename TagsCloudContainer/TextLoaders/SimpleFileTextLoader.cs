using System.IO;

namespace TagsCloudContainer.TextLoaders
{
    public class SimpleFileTextLoader :ITextLoader
    {
        
        public string LoadText(string fileName)
        {
            var text = File.ReadAllText(fileName);
            return text;
        }
    }
}
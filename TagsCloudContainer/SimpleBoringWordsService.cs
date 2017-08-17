using System.Collections.Generic;

namespace TagsCloudContainer
{
    public class SimpleBoringWordsService : IBoringWordsService
    {
        public HashSet<string> GetBoringWords()
        {
            return new HashSet<string>(new[] {"и", "в", "с", "над", "по","не","или","к","на"});
        }
    }
}
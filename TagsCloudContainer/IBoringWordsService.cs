using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IBoringWordsService
    {
        HashSet<string> GetBoringWords();
    }
}
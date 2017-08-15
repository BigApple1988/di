using System;
using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ITagsParser
    {
        IEnumerable<Tuple<string, int>> ParseTags();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class SimpleTagsParser :ITagsParser
    {
        private readonly ITextLoader textLoader;

        public SimpleTagsParser(ITextLoader textLoader)
        {
            this.textLoader = textLoader;
        }

        public IEnumerable<Tuple<string, int>> ParseTags()
        {
            var text = textLoader.LoadText();
            var splitted = text.Split(new []{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            var tuple = splitted.GroupBy(s => s.ToLower()).Select(s => Tuple.Create(s.Key, s.Count()));
            return tuple;
        }


    }
}
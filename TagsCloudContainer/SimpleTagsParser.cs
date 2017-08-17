using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class SimpleTagsParser :ITagsParser
    {
        private readonly IBoringWordsService boringWordsService;

        public SimpleTagsParser(IBoringWordsService boringWordsService)
        {
            this.boringWordsService = boringWordsService;
        }

//        public SimpleTagsParser(ITextLoader textLoader)
//        {
//            this.textLoader = textLoader;
//        }

        public IEnumerable<Tuple<string, int>> ParseTags(string text)
        {
            var splitted = text.Split(new []{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            var boringWords = boringWordsService.GetBoringWords();
            var withoutBoring = splitted.Select(s => s).Where(s => !boringWords.Contains(s));
            var tuple = withoutBoring.GroupBy(s => s.ToLower()).Select(s => Tuple.Create(s.Key, s.Count()));
            return tuple;
        }


    }

}
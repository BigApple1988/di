using System;

namespace TagsCloudContainer.TextLoaders
{
    public interface ITextLoaderFactory
    {
        ITextLoader Create(FileFormat format);
    }
    public class SimpleFileTextLoaderFactory :ITextLoaderFactory
    {
        private readonly Func<FileFormat, SimpleFileTextLoader> factory;

        public SimpleFileTextLoaderFactory(Func<FileFormat, SimpleFileTextLoader> factory)
        {
            this.factory = factory;
        }

        public ITextLoader Create(FileFormat format)
        {
            return factory(format);
        }
    }
    public class TxtFileTextLoaderFactory :ITextLoaderFactory
    {
        private readonly Func<FileFormat, TxtFileTextLoader> factory;

        public TxtFileTextLoaderFactory(Func<FileFormat, TxtFileTextLoader> factory)
        {
            this.factory = factory;
        }

        public ITextLoader Create(FileFormat format)
        {
            return factory(format);
        }
       
    }
}
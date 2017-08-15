using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace TagsCloudContainer
{
    class Program
    {
        static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileTextLoader>().As<ITextLoader>().WithParameter("fileName", @"C:\Users\BigApple\Source\Repos\di\text.txt");
            builder.RegisterType<SimpleTagsParser>().As<ITagsParser>();
            builder.RegisterType<TagsCloudVizualizer>().As< TagsCloudVizualizer>();

            return builder.Build();
        }

        
        static void Main(string[] args)
        {
            var container = CreateContainer();
            container.Resolve<TagsCloudVizualizer>().GenerateCloud();

        }
    }
}

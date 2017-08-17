using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using CommandLine;
using NUnit.Framework.Constraints;
using TagsCloudContainer.TextLoaders;

namespace TagsCloudContainer
{
    class Program
    {
        static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
//            builder.RegisterType<SimpleFileTextLoader>().As<ITextLoader>().WithParameter("fileName", @"C:\Users\BigApple\Source\Repos\di\text.txt");
            builder.RegisterType<SimpleTagsParser>().As<ITagsParser>();
            builder.RegisterType<TagsCloudVizualizer>().As<TagsCloudVizualizer>();
            builder.RegisterType<SimpleBoringWordsService>().As<IBoringWordsService>();
            builder.RegisterType<SimpleFileTextLoader>()
                .As<ITextLoader>()
               .Keyed<ITextLoader>(FileFormat.Unspecified);
            builder.RegisterType<TxtFileTextLoader>().As<ITextLoader>()
                .Keyed<ITextLoader>(FileFormat.Txt);
            return builder.Build();
        }

        
        static void Main(string[] args)
        {
            var container = CreateContainer();
            var parser = new CommandLine.Parser();
            var settings = new Settings();
            parser.ParseArguments(args, settings);
            container.Resolve<TagsCloudVizualizer>()
                .GenerateCloud( settings.TextColor,settings.BackgroundColor,new Font(settings.FontName,10),new Size(10000,10000), settings.InputFileName, settings.OutputFileName);

        }
    }

    public class Settings
    {
        [Option("inputfilename", DefaultValue = @"C:\Users\BigApple\Source\Repos\di\text.txt", HelpText = "Input filename", Required = true)]
        public string InputFileName { get; set; }
        [Option("outputfilename", DefaultValue = @"abc.png", HelpText = "Output filename with extension", Required = false)]
        public string OutputFileName { get; set; }
        [Option("textcolor", DefaultValue = @"black", HelpText = "Text font color", Required = false)]
        public string TextColorName { get; set; }
        [Option("backcolor", DefaultValue = @"White", HelpText = "Background color", Required = false)]
        public string BackgroundColorName { get; set; }
        [Option("imagewidth", DefaultValue = 10000, HelpText = "image width", Required = false)]
        public int Width { get; set; }
        [Option("imageheight", DefaultValue = 10000, HelpText = "image height", Required = false)]
        public int Height{ get; set; }
        [Option("fontname", DefaultValue = "Arial", HelpText = "Font name", Required = false)]
        public string FontName { get; set; }

        public Color TextColor => Color.FromName(TextColorName);
        public Color BackgroundColor => Color.FromName(BackgroundColorName);
    }
    public enum FileFormat
    {
        Txt,Unspecified
    }
}

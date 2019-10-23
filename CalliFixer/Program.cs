using CalliFixer.Protections;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalliFixer
{
    class Program
    {

        public static bool veryVerbose = false;
        public static string Asmpath;
        public static ModuleDefMD module;
        public static Assembly asm;
        private static string path = null;
        public static int MathsAmount;
        public static ModuleDefMD AsmethodMdOriginal;
        public static int SizeOFAmount;
        public static string directory;
        static void Main(string[] args)
        {


            Console.Title = "RzyDesintegrator";
            Console.ForegroundColor = ConsoleColor.Yellow;
            string directory = args[0];

            try
            {
                Program.module = ModuleDefMD.Load(directory);
                Program.asm = Assembly.LoadFrom(directory);
                Program.Asmpath = directory;
            }
            catch (Exception)
            {
                Logger.Write("Not a .NET Assembly...", Logger.Type.Error);
                Console.ReadKey();
                Environment.Exit(0);
            }
            AssemblyDef assembly = AssemblyDef.Load(directory);
            try { Calli.run(module); }
            catch (Exception e) { Logger.Write($"Error while trying to remove Calli Protection." + e, Logger.Type.Error); }


            string text = Path.GetDirectoryName(directory);
            if (!text.EndsWith("\\"))
            {
                text += "\\";
            }
            string filename = string.Format("{0}{1}-Desintegrated{2}", text, Path.GetFileNameWithoutExtension(directory), Path.GetExtension(directory));
            ModuleWriterOptions writerOptions = new ModuleWriterOptions(module);
            writerOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            writerOptions.Logger = DummyLogger.NoThrowInstance;
            NativeModuleWriterOptions NativewriterOptions = new NativeModuleWriterOptions(module);
            NativewriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            NativewriterOptions.Logger = DummyLogger.NoThrowInstance;
            if (module.IsILOnly) { module.Write(filename, writerOptions); } else { module.NativeWrite(filename, NativewriterOptions); }

            Logger.Write($"File saved at: {filename}", Logger.Type.Done);
            Console.ReadKey();
            Environment.Exit(0);


        }
    }
}
// Made By RZY#7797
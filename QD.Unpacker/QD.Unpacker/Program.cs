using System;
using System.IO;

namespace QD.Unpacker
{
    class Program
    {
        private static String m_Title = "Quantic Dream BigFile Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    QD.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of IDX file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    QD.Unpacker E:\\Games\\Detroit Become Human\\BigFile_PC.idx D:\\Unpacked");
                Console.WriteLine("    QD.Unpacker E:\\Games\\HEAVY RAIN\\Resources\\BigFile_WIN.idx D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_IndexFile = Utils.iCheckIndexFile(args[0]);
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_IndexFile))
            {
                Utils.iSetError("[ERROR]: Input index file -> " + m_IndexFile + " <- does not exist");
                return;
            }

            BigFileUnpack.iDoIt(m_IndexFile, m_Output);
        }
    }
}

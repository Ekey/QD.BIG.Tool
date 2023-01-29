using System;
using System.IO;
using System.Reflection;

namespace QD.Unpacker
{
    class Utils
    {
        public static String iGetApplicationPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static String iGetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void iSetInfo(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m_String);
            Console.ResetColor();
        }

        public static void iSetError(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static void iSetWarning(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static String iCheckArgumentsPath(String m_Arg)
        {
            if (m_Arg.EndsWith("\\") == false)
            {
                m_Arg = m_Arg + @"\";
            }
            return m_Arg;
        }

        public static String iCheckIndexFile(String m_Arg)
        {
            if (Path.GetExtension(m_Arg) != ".idx")
            {
                throw new Exception("[ERROR]: You must select BigFile_PC.idx file!");
            }
            return m_Arg;
        }

        public static String iDebugFile(String m_Arg)
        {
            m_Arg = Path.GetDirectoryName(m_Arg) + @"\" + Path.GetFileNameWithoutExtension(m_Arg) + ".dbg";
            return m_Arg;
        }

        public static void iCreateDirectory(String m_Directory)
        {
            if (!Directory.Exists(Path.GetDirectoryName(m_Directory)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(m_Directory));
            }
        }
    }
}

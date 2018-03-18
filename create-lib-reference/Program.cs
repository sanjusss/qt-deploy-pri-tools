using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace create_lib_reference
{
    class Program
    {
        private static string debugPri = string.Empty;
        private static string releasePri = string.Empty;

        static void Main(string[] args)
        {
            try
            {
                switch (args.Length)
                {
                    case 1:
                        InitBasicPri(args[0]);
                        break;

                    case 3:
                        InitBasicPri(args[2]);
                        InitAppPri(args[0], args[1], args[2]);
                        break;

                    case 4:
                        InitBasicPri(args[3]);
                        InitStaticLibPri(args[0], args[1], args[2], args[3]);
                        break;

                    case 5:
                        InitBasicPri(args[4]);
                        InitDynamicLibPri(args[0], args[1], args[2], args[3], args[4]);
                        break;

                    default:
                        PrintHelp();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                PrintHelp();
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("create-lib-reference <debug|release> <lib path> <dll path> <include path> <pro path>");
            Console.WriteLine("create-lib-reference <debug|release> <lib path> <include path> <pro path>");
            Console.WriteLine("create-lib-reference <debug|release> <exe path> <pro path>");
            Console.WriteLine("create-lib-reference <pro path>");
        }

        private static bool NeedCreatePri(string pro, string pri)
        {
            FileInfo profi = new FileInfo(pro);
            FileInfo prifi = new FileInfo(pri);
            return prifi.Exists == false || profi.LastWriteTime > prifi.LastWriteTime;
        }

        private static string GetCopyDirCode(string src, string pri)
        {
            string path = Path.GetDirectoryName(pri);
            string srcDirPath = GetRelativePath(path, src);
            string name = Path.GetFileNameWithoutExtension(pri);
            name = name.Replace('-', '_');
            string result = string.Format(Resource.copy_reference_dir,
                srcDirPath,
                name) + "\n";
            return result;
        }

        private static string GetImportCode(string libPath, string includePath, string pri)
        {
            string path = Path.GetDirectoryName(pri);
            string libDir = Path.GetDirectoryName(libPath);
            string libDirPath = GetRelativePath(path, libDir);
            string libName = Path.GetFileNameWithoutExtension(libPath);
            string includeDirPath = GetRelativePath(path, includePath);
            string result = string.Format(Resource.default_import, libDirPath, libName, includeDirPath);
            return result;
        }

        private static string GetRelativePath(string root, string src)
        {
            if (root.EndsWith("/") == false && root.EndsWith("\\") == false)
            {
                root += "\\";
            }

            Uri uRoot = new Uri(root, UriKind.Absolute);
            Uri uSrc = new Uri(src, UriKind.Absolute);
            Uri uResult = uRoot.MakeRelativeUri(uSrc);
            string result = uResult.OriginalString;
            result = result.Replace('\\', '/');
            return result;
        }

        private static void InitBasicPri(string pro)
        {
            string baseName = Path.GetFileNameWithoutExtension(pro);
            string path = Path.GetDirectoryName(pro);
            string basePri = path + "\\" + baseName + ".pri";
            debugPri = path + "\\" + baseName + "-debug.pri";
            releasePri = path + "\\" + baseName + "-release.pri";
            if (NeedCreatePri(pro, basePri) == false)
            {
                return;
            }
            
            using (var writer = new StreamWriter(basePri, false, new UTF8Encoding(false)))
            {
                writer.WriteLine(string.Format(Resource.default_base, baseName));
                writer.Flush();
                writer.Close();
            }
        }

        private static void InitAppPri(string type, string exePath, string pro)
        {
            string pri = string.Empty;
            if (type == "release")
            {
                pri = releasePri;
            }
            else if (type == "debug")
            {
                pri = debugPri;
            }
            else
            {
                PrintHelp();
                return;
            }

            if (NeedCreatePri(pro, pri) == false)
            {
                return;
            }

            string path = Path.GetDirectoryName(exePath);
            using (var writer = new StreamWriter(pri, false, new UTF8Encoding(false)))
            {
                writer.WriteLine(GetCopyDirCode(path, pri));
                writer.Flush();
                writer.Close();
            }
        }

        private static void InitDynamicLibPri(string type, string libPath, string dllPath, string includePath, string pro)
        {
            InitLibPri(true, type, libPath, dllPath, includePath, pro);
        }

        private static void InitStaticLibPri(string type, string libPath, string includePath, string pro)
        {
            InitLibPri(false, type, libPath, string.Empty, includePath, pro);
        }

        private static void InitLibPri(bool dynamic, string type, string libPath, string dllPath, string includePath, string pro)
        {
            string pri = string.Empty;
            if (type == "release")
            {
                pri = releasePri;
            }
            else if (type == "debug")
            {
                pri = debugPri;
            }
            else
            {
                PrintHelp();
                return;
            }

            if (NeedCreatePri(pro, pri) == false)
            {
                return;
            }

            using (var writer = new StreamWriter(pri, false, new UTF8Encoding(false)))
            {
                if (dynamic)
                {
                    string path = Path.GetDirectoryName(dllPath);
                    writer.WriteLine(GetCopyDirCode(path, pri));
                }

                writer.WriteLine(GetImportCode(libPath, includePath, pri));
                writer.Flush();
                writer.Close();
            }
        }
    }
}

using System.IO;
using UnityEditor;

namespace cdi
{
    public class ClassWriter
    {
        StreamWriter outfile;
        string path;

        public int indentLevel;

        public ClassWriter(string path)
        {
            this.path = path;
        }

        public void Open()
        {
            outfile = new StreamWriter(path);
        }

        public void Close()
        {
            outfile.Close();
            AssetDatabase.Refresh();
        }

        public void WriteLine(string line = "")
        {
            for (int i = 0; i < indentLevel; i++)
            {
                line = "\t" + line;
            }

            outfile.WriteLine(line);
        }
    }
}
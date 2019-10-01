using Vocabulary.Data;

namespace Vocabulary.Logic
{
    internal class HelperClass
    {
        internal static bool Save(string path, Dictionary dictionary)
        {
            return DiskIO.Save(path, dictionary);
        }

        internal static Dictionary Open(string path)
        {
            return DiskIO.Open(path);
        }
    }
}
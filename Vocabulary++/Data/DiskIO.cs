// Copyright (c) Hannes Barbez. All rights reserved.
// Licensed under the GNU General Public License v3.0

using System.IO;
using System.Xml.Serialization;
using Vocabulary.Logic;

namespace Vocabulary.Data
{
    internal static class DiskIO
    {
        internal static Dictionary Open(string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Dictionary));
                    Dictionary mb = (Dictionary) xs.Deserialize(fs);
                    return mb;
                }
            }
            catch
            {
                return new Dictionary();
            }
        }

        internal static bool Save(string filename, Dictionary m)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(m.GetType());
                    xs.Serialize(fs, m);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

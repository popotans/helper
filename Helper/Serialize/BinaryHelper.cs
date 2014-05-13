using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace Helper.Serialize
{
    public class BinaryHelper<T> where T : class
    {
        public static string ToString(T t)
        {
            if (t == null)
                return null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, t);
            ms.Position = 0;
            byte[] bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return Convert.ToBase64String(bytes);
        }

        public static T ToObject(string binaryStr)
        {
            byte[] bytes = Convert.FromBase64String(binaryStr);
            T t = default(T);
            object obj = null;
            if (bytes == null)
                return t;
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);
            ms.Close();
            t = (T)obj;
            return t;
        }
    }
}

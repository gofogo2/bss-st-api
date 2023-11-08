using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_SE.Helpers
{
    public static class ByteConverter
    {
        public static object ByteToObject(byte[] buffer)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    stream.Position = 0;
                    return binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return null;
        }

        public static byte[] ObjectToByte(object obj)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return null;
        }

        // 바이트 배열을 String으로 변환 
        public static string ByteToString(byte[] buffer)
        {
            string str = Encoding.Default.GetString(buffer);
            return str;
        }
        // String을 바이트 배열로 변환 
        public static byte[] StringToByte(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return buffer;
        }

    }
}

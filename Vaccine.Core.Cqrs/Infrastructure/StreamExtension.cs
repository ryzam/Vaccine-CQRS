using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vaccine.Core.Cqrs.Infrastructure
{
    public class StreamExtension
    {
        public static T Deserialize<T>(byte[] payLoad)
        {
            using (var stream = new MemoryStream(payLoad))
            {
                var bf = new BinaryFormatter();

                return (T)bf.Deserialize(stream);
            }
        }

        public static Byte[] Serialize(object e)
        {
            using (var stream = new MemoryStream())
            {
                var bf = new BinaryFormatter();

                bf.Serialize(stream, e);

                return stream.ToArray();
            }
        }


        public Stream GetStream(byte[] payLoad)
        {
            return new MemoryStream(payLoad);
        }

    }
}

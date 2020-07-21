using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ICcard32.Base.Json
{
    public static class Json
    {
        public static void ToObj<T>(byte[] bytes, out T t)
        {
            t = default(T);

            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
                    t = (T)deseralizer.ReadObject(ms);// //反序列化ReadObject
                }
            }
            catch(Exception)
            {

            }
        }

        public static void ToString<T>(T obj, out string s)
        {
            s = "";
            Type t = typeof(T);

            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(t);

                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);

                    var byteArr = stream.ToArray();

                    s = Encoding.UTF8.GetString(byteArr);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

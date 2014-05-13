using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Helper.Serialize
{
    public class JsonHelper
    {
        public static string ToString<T>(T t) where T : class
        {
            try
            {
                string s = JsonConvert.SerializeObject(t);
                return s;
            }
            catch (Exception e)
            {
                throw new Excep.HelperException(e.Message);
            }
        }

        public static string ToString(object t)
        {
            try
            {
                string s = JsonConvert.SerializeObject(t);
                return s;
            }
            catch (Exception e)
            {
                throw new Excep.HelperException(e.Message);
            }
        }

        public static T ToObject<T>(string jsonString) where T : class
        {
            try
            {
                T t = default(T);
                t = JsonConvert.DeserializeObject<T>(jsonString);
                return t;
            }
            catch (Exception e)
            {
                throw new Excep.HelperException(e.Message);
            }
        }


        public static object ToObject(string jsonStr,Type type)
        {
            try
            {
                // t = JsonConvert.DeserializeObject<T>(jsonString);...
                return JsonConvert.DeserializeObject(jsonStr,type);

                //return t;
            }
            catch (Exception e)
            {
                throw new Excep.HelperException(e.Message);
            }
        }

    }


}

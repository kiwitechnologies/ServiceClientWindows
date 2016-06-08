using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Classes
{
    public static class Err_Logger
    {
        public static Err_BodyParameter err_BodyParameter { get; set; }
        public static Err_HeaderParameter err_HeaderParameter { get; set; }
        public static Err_MissingAction err_MissingAction { get; set; }

        public static void Reset()
        {
            err_BodyParameter = new Err_BodyParameter();
            err_HeaderParameter = new Err_HeaderParameter();
            err_MissingAction = new Err_MissingAction();
        }
    }

    public class Err_BodyParameter
    {
        public Dictionary<string, List<string>> missing = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> invalid = new Dictionary<string, List<string>>();
    }

    public class Err_HeaderParameter
    {
        public Dictionary<string, List<string>> missing = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> invalid = new Dictionary<string, List<string>>();
    }

    public class Err_MissingAction
    {
        public string MissingAction = string.Empty;
    }

    public class MissingKey
    {
        public static Dictionary<string, List<string>> dictMissingKey { get; set; }
    }

    public class InvalidKey
    {
        public static Dictionary<string, List<string>> dictInvalidKey { get; set; }
    }
}

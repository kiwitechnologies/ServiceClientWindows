// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.RequestManager
{
    public class JsonParseClass
    {
        // for Get signUp details
        public static APIResponse DummyAPIResponseSerialization(string validate)
        {
            APIResponse response;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(APIResponse));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(validate)))
            {
                response = serializer.ReadObject(stream) as APIResponse;
            }
            return response;
        }

    }
}

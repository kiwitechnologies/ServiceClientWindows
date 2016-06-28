// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SocialManager.LinkedInManager
{
    public class JsonParseClass
    {
        // for deviceRegister
        public static csUserInfo LinkedInUserInfoResponseSerialization(string validate)
        {
            csUserInfo response;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(csUserInfo));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(validate)))
            {
                response = serializer.ReadObject(stream) as csUserInfo;
            }
            return response;
        }

        public static Response PublishContentResponseSerialization(string validate)
        {
            Response response;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(validate)))
            {
                response = serializer.ReadObject(stream) as Response;
            }
            return response;
        }
    }

    public class StatusAndResponseClass
    {
        public string responseString { get; set; }
        public Response response { get; set; }
        public int statusCode { get; set; }
    }

    public class Response
    {
        public int errorCode { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public int status { get; set; }
        public long timestamp { get; set; }
    }
}

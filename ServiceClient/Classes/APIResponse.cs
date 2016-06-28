// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Classes
{
    public class Validations
    {
        public int format_string { get; set; }
        public string max { get; set; }
        public string min { get; set; }
        public int require { get; set; }
        public string size { get; set; }
        public string format_file { get; set; }
    }

    public class BodyParameter
    {
        public string key_name { get; set; }
        public int validation_data_type { get; set; }
        public Validations validations { get; set; }
    }

    public class Header
    {
        public string key_name { get; set; }
        public List<string> key_value { get; set; }
    }

    public class Action
    {
        public string base_url { get; set; }
        public string dev_url { get; set; }
        public string qa_url { get; set; }
        public string staging_url { get; set; }
        public string production_url { get; set; }
        public string action { get; set; }
        public string action_id { get; set; }
        public string request_type { get; set; }
        public List<BodyParameter> body_parameters { get; set; }
        public List<Header> headers { get; set; }
    }

    public class APIResponse
    {
        public string project_id { get; set; }
        public long updated_at { get; set; }
        public List<Action> actions { get; set; }
    }
}

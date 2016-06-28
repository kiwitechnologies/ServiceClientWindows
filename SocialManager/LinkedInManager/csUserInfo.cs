// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using System.Collections.Generic;

namespace SocialManager.LinkedInManager
{
    public class Company
    {
        public int id { get; set; }
        public string industry { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string type { get; set; }
    }

    public class StartDate
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Value
    {
        public Company company { get; set; }
        public int id { get; set; }
        public bool isCurrent { get; set; }
        public StartDate startDate { get; set; }
        public string title { get; set; }
    }

    public class Positions
    {
        public int _total { get; set; }
        public List<Value> values { get; set; }
    }

    public class csUserInfo
    {
        public string firstName { get; set; }
        public string headline { get; set; }
        public string id { get; set; }
        public string industry { get; set; }
        public string lastName { get; set; }
        public string pictureUrl { get; set; }
        public Positions positions { get; set; }
        public string summary { get; set; }
    }
}

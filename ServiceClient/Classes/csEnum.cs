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
    public class csEnum
    {
        public enum ValidationType
        {
            INTEGER = 0, 
            FLOAT = 1, 
            STRING = 2, 
            TEXT = 3, 
            FILE = 4
        }

        public enum StringFormat
        {
            ALPHA = 0,
            NUMERIC = 1,
            ALPHANUMERIC = 2,
            EMAIL = 3,
        }
    }
}

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
    public class APIDetails
    {
        public string ActionID
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        public string ActionType
        {
            get;
            set;
        }

        public string BaseURL
        {
            get;
            set;
        }

        public int DataType
        {
            get;
            set;
        }

        public int Format
        {
            get;
            set;
        }

        public string keyName
        {
            get;
            set;
        }

        public string MaximumLength
        {
            get;
            set;
        }

        public string MinimumLength
        {
            get;
            set;
        }

        public int Required
        {
            get;
            set;
        }
    }
}

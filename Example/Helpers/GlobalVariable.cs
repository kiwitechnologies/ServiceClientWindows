// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Helpers
{
    public static class GlobalVariable
    {
        public static FacebookClient fbclient = new FacebookClient();
        public static string FbAccessToken = string.Empty;
    }
}

// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

namespace SocialManager.TwitterManager
{
    public class TweetViewModel
    {
        public string ImageUrl { get; set; }

        public string ScreenName { get; set; }

        public string Text { get; set; }
    }

    public class FriendViewModel
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}

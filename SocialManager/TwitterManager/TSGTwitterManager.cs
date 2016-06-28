// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;

namespace SocialManager.TwitterManager
{
    public class TSGTwitterManager
    {
        //Check UserAuthenticate from Twitter and Get UserDeatil
        public static async Task<bool> UserAuthenticate()
        {
            try
            {
                var authorizer = new UniversalAuthorizer
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = GlobalVariable.TwitterConsumerKey,
                        ConsumerSecret = GlobalVariable.TwitterConsumerSecret
                    },
                    Callback = GlobalVariable.TwitterCallBackUri
                };
                await authorizer.AuthorizeAsync();
                GlobalVariable.twitterContext = new TwitterContext(authorizer);
                var verifyResponse = await (from acct in GlobalVariable.twitterContext.Account
                                            where acct.Type == AccountType.VerifyCredentials
                                            select acct).SingleOrDefaultAsync();
                if (verifyResponse != null && verifyResponse.User != null)
                {
                    GlobalVariable.TwitterUserInfo = verifyResponse.User;
                    return true;
                }
            }
            catch (TwitterQueryException TqEx)
            {
                Debug.WriteLine("network Problem " + TqEx);
                return false;
            }
            return false;
        }

        // Post Tweet 
        public static async Task<Tuple<bool, Status>> PostTweet(string strUserInput)
        {
            try
            {
                if (GlobalVariable.twitterContext != null)
                {
                    Status tweet = await GlobalVariable.twitterContext.TweetAsync(strUserInput);
                    if (!string.IsNullOrEmpty(tweet.Text))
                    {
                        return new Tuple<bool, Status>(true, tweet);
                    }
                    else
                    {
                        return new Tuple<bool, Status>(false, null);
                    }
                }
                return new Tuple<bool, Status>(false, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShowUserDetailsAsync" + ex);
            }
            return new Tuple<bool, Status>(false, null);
        }

        //Search Tweet  
        public async static Task<ObservableCollection<TweetViewModel>> SearchTweet(string strSearchString)
        {
            List<TweetViewModel> tweets = new List<TweetViewModel>();
            try
            {
                if (GlobalVariable.twitterContext != null)
                {
                    Search searchResponse =
                await
                (from search in GlobalVariable.twitterContext.Search
                 where search.Type == SearchType.Search &&
                       search.Query == strSearchString &&
                       search.Count == 100
                 select search)
                .SingleOrDefaultAsync();
                    tweets =
                  (from tweet in searchResponse.Statuses
                   select new TweetViewModel
                   {
                       ImageUrl = tweet.User.ProfileImageUrl,
                       ScreenName = tweet.User.ScreenNameResponse,
                       Text = tweet.Text
                   })
                  .ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-------TwitterClient---SearchTweet------" + ex.Message);
            }
            return new ObservableCollection<TweetViewModel>(tweets);
        }

        // Get Friend List
        public static async Task<List<FriendViewModel>> FriendsListAsync()
        {
            try
            {
                if (GlobalVariable.twitterContext != null)
                {
                    Friendship friendship;
                    long cursor = -1;
                    GlobalVariable.lstTwitterFriendList.Clear();
                    do
                    {
                        friendship =
                            await
                            (from friend in GlobalVariable.twitterContext.Friendship
                             where friend.Type == FriendshipType.FriendsList &&
                                   friend.ScreenName == GlobalVariable.TwitterUserInfo.ScreenNameResponse &&
                                   friend.Cursor == cursor
                             select friend)
                            .SingleOrDefaultAsync();

                        if (friendship != null &&
                            friendship.Users != null &&
                            friendship.CursorMovement != null)
                        {
                            cursor = friendship.CursorMovement.Next;

                            friendship.Users.ForEach(friend =>

                            GlobalVariable.lstTwitterFriendList.Add(new FriendViewModel { Name = friend.Name, Location = friend.Location, ImageUrl = friend.ProfileImageUrl }
                              ));
                        }

                    } while (cursor != 0);
                }
            }
            catch (Exception ex)
            {

            }
            return GlobalVariable.lstTwitterFriendList;
        }

        //Logout from Twitter
        public static void TwitterLogout()
        {
            if (GlobalVariable.twitterContext != null)
            {
                GlobalVariable.twitterContext = null;
            }
        }
    }
}

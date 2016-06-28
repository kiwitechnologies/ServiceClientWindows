// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SocialManager.TwitterManager;
using System;
using System.Linq;
using TSGSocialManager.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TwitterFriendList : Page
    {
        public TwitterFriendList()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(TwitterUserInfo));
            };
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                await TSGTwitterManager.FriendsListAsync();
                if (GlobalVariable.lstTwitterFriendList != null)
                {
                    if (GlobalVariable.lstTwitterFriendList.Count > 0)
                    {
                        lstFriendList.DataContext = GlobalVariable.lstTwitterFriendList.OrderBy(x => x.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtFriend_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string strName = txtFriend.Text.ToLower().Trim();
                if (!string.IsNullOrWhiteSpace(strName) && !string.IsNullOrWhiteSpace(strName))
                {

                    // lstGroups.ItemsSource = null;
                    lstFriendList.ItemsSource = GlobalVariable.lstTwitterFriendList.Where(x => x.Name.ToLower().Contains(strName)).ToList();
                }
                else
                {
                    // lstGroups.ItemsSource = null;
                    lstFriendList.ItemsSource = GlobalVariable.lstTwitterFriendList.OrderBy(x => x.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur txtFriend_TextChanged() FriendList : " + ex.ToString());
            }

        }
    }
}

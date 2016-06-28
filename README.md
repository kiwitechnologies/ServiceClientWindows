Facebook and Twitter Sample
=============
## Features
1. Facebook ( Login, getProfile, Text sharing, Image sharing, Linked Content Sharing, getFriend, Logout ).
2. Twitter ( Login, GetProfile, Post Tweet, Get user Tweet, Search Tweets, Get Friends list, Logout).
3. LinkedIn (Login, Get Profile, Publish Comment, Share Content, Logout).
4. Google (Login, Get user info, Logout).
5. Instagram (Login, Get Self Profile, Get User Profile, Get Recent Media, Get User Recent Media, Get Recent Media Liked, Search Query, Get Follows List, Get Followed By List, Logout).
6. Google Calendar (Login, Get Callendar Events).

Getting started
----------------
- The SocialSharing folder contains TSGSocialSharing project. This example project contains the sample code for Facebook, Twitter and LinkedIn integration about how to use the social sharing framework and .
- To use the Sharing module in your application, open the Example project and copy the coresponding package of Facebook or Twitter in your project.
- Configure the related social sharing module in your project.

Strategy configuration
======================

FACEBOOK
--------
1. Create an application on facebook (if not exist), https://developers.facebook.com/apps and click at "Add New App" button.
2. Go to settings option and click on Add Platform.
3. Now select Windows App as a Platform.
4. Fill Windows Store ID.
5. Installation of Facebook SDK.
	- Install the Facebook nuget into the solution by selecting Manage Nuget Packages.
6. Copy "Helpers" folder from the Example project into your Main project.
7. Copy "TSGFacebookManger.cs" file into your project.
8. Skip the step to initializing the facebook SDK. Instead of it just call the "TSGFacebookManager" functions from your activity.
9. Go throw with the sample code of TSGFacebookManager.cs class in FacebookMainPage.xaml.cs.

TWITTER
--------
1. Create an application on Twitter (if not exist), https://apps.twitter.com/app/new.
2. Install "LinqtoTwitter" Nuget into the solution by selecting Manage Nuget Packages.
3. Copy "TSGTwitterManager.cs" file into your project.
4. After setup, you can use any required static function of TSGTwitterManager.cs class.
5. Go throw with the sample code of TSGTwitterManager.cs class in TwitterMainPage.xaml.cs.

LINKEDIN
--------
1. Register your application on LinkedIn (It not exist), https://www.linkedin.com/developer/apps/new.
2. Copy LinkedInManager in your project.
3. After setup, you can use any required static function of TSGLinkedInManager.cs class.
4. Go throw with the sample code of TSGLinkedInManager.cs class in LinkedInMainPage.xaml.cs.

GOOGLE
------
1. Register your application at https://console.developers.google.com/apis/library.
2. Create a project using above link if not exist.
3. Copy GoogleManager folder in your application.
4. Call Configure method from the TSGGoogleManager.cs class to start the login process.
5. After login you can call the required function from the TSGGoogleManager class.
6. Go throw with the sample code of TSGGoogleManager.cs class in GoogleMainPage.xaml.cs. 


GOOGLE CALENDAR
---------------
1. Setup your project by using following instructions https://developers.google.com/google-apps/calendar/quickstart/dotnet.
2. Follow Step 1 & 2 only. Don't use Step 3 as it is for .Net users not for Windows Phone 8.1 or later.
3. Create App and get ClientId & Secret Key.
4. Copy GoogleCalendarContent folder from Main Project.
5. Call HelperMethods.Authenticatoin() to start autentication process.
6. Go throw with the sample code of TSGGoogleCalendarManager.cs class in GoogleCalendarMainPage.xaml.cs. 

INSTAGRAM
--------
1. Register your application and get ClientId, ClientSecret and Redirect_url at "https://www.instagram.com/developer/".
2. Copy InstagramManager folder to your project.
3. Create the instance of TSGInstagramManager class in your activity by providing the ClientId, ClientSecret and RedirectURL as parameter.
4. Use TSGInstagramManager.Authorization function for login.
5. Use TSGInstagramManager instance to call other functions in your application.
6. Gothrow with the sample code of TSGInstagramManager.cs class in InstagramMainPage.xaml.cs.

Required frameworks:
1.Visual Studio 2015 2.Facebook 3.FacebookClient 4.LinqtoTwitter

License
---------
Securtiy Framework is KiwiTechnolgies Licensed  
Copyright © 2016 


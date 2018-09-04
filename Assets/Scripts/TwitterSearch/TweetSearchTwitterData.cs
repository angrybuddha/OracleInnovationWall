using System.Collections;
using UnityEngine;
using System;

public class TweetSearchTwitterData {
	public string tweetText = "";
    public string tweetMedia = "";
	public string screenName = "";
	public string profileImageUrl = "";
	public Int64 retweetCount = 0;
	
	public override string ToString(){
		return screenName + " posted: \"" + tweetText + "\" and retweeted " + retweetCount.ToString() + " times. Profile image URL: " +  profileImageUrl + " Media URL: " + tweetMedia;
	}
}
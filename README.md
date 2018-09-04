##### Description:
Oracle Innovation Wall

##### Current Version: 1.0.1f1

##### 1.0.1f1
- Background color correction
- Moved Oracle logo over more

##### 1.0.0f2
- Added player icons for scatter plot
- Added Move to Vote text

##### 1.0.0f1
- Added exploding cube
- Floating text moves faster
- Made value prop last longer
- Added more ambient animations, and cms questions
- Fixed percent text
- Transition sequencefrom question results to Did You Know is smoother
- Removed Result Part1 for Scatter Plot

##### 1.0.0b2
- Did You Know and Value Prop floating animations
- Answers float in sync with floating questions
- For longer answers, the text size automatically scale down to fit in the single screen space

##### Builds Location:
[Build Links](https://downstream.egnyte.com/SimpleUI/home.do#Files/0/Shared/Oracle/Innovation%20Wall/Builds)

##### Simulator
http://45.33.111.206:5000/

##### Kinect Setup
1. each kinect pc needs to run the kinect tracker app. Something like "KinectWPFOpenCV.exe"
2. each one has a specific configuration (left, center, right, and some offsets)
3. the aggregator runs on one of the PC's. Something like "DSBlobAggregator.exe"

##### Release Notes 1.0.0b1 (06-30-15):
- Initial Version

##### Unity Build Process:
**1)** Open 'oracle_innovation_wall' project with Unity Editor 5.1.1f1

**2)** Make sure to have Oracle 'Innovation Wall.unity' as 0, should be already setup.

**3)** Select 'PC, Mac...' for the Platform.

**4)** Select Windows for Target Platform.

**5)** Select x86_64 for the Architecture.

**6)** Click Build

**7)** Once finished building make sure you copy and paste the ConfigFile.xml from the project to the same location that contains the .exe


##### Hidden Menu:

To access the hidden debug menu ether click on the oracle logo, or press ESC.

##### Keyboard Options:

To refresh the scene press F5

##### Shortcut Commands:
To pop out the screen in boarderless window mode make a shortcut of the Unity exe and add the following to the Target:
-popupwindow -screen-width 800 -screen-height 600
Example: "C:\Users\lane.gresham\Desktop\x64\Braves PC_DX11\Braves PC_DX11.exe" -popupwindow -screen-width 800 -screen-height  600

Note: that if in the config file if window_override = true, the width and size will be overwritten based on the config file.

##### Config:
 
The setup config file is located in the install folder as _ConfigFile.xml_

###### xml Parameter Descriptions:
 - window_fullscreen =  disable/enables fullscreen
 - window_override = disable/enables for overriding the fullscreen, and x/y size of launch window
 - window_x = width of the window
 - window_y = height of the window
 - url_request = http request url for saving votes
 - kinect_area_x = width area of kinect, in meters
 - kinect_area_y = height area of kinect, in meters
 - mqtt_server_ip = is the mqtt server used for the kinect signal
 - mqtt_topic = is the mqtt topic used for the kinect signal
 - qualitysettings_referenceresolution_x = sets the GUI resolution (note: by default it is 1920)
 - qualitysettings_referenceresolution_y = sets the GUI resolution (note: by default it is 1080)
 - resource_db = db file.
 - resource_location = resource folder location.
 - resource_media = media folder location.
 
##### ConfigFile.xml Example
 ```xml
<?xml version="1.0" encoding="utf-8"?>
<UserData xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/">
  <display>
    <window_fullscreen>false</window_fullscreen>
    <window_override>true</window_override>
    <window_x>4440</window_x>
    <window_y>1920</window_y>
  </display>
  <httpRequestSettings>
    <url_request>http://148.87.38.143/oracle/api/result</url_request>
  </httpRequestSettings>
  <kinectSettings>
    <kinect_area_x>6.096</kinect_area_x>
    <kinect_area_y>1.8288</kinect_area_y>
  </kinectSettings>
  <mqttSettings>
    <mqtt_server_ip>148.87.38.146</mqtt_server_ip>
    <mqtt_topic>stream/kinect</mqtt_topic>
  </mqttSettings>
  <qualitySettings>
    <qualitysettings_referenceresolution_x>1920</qualitysettings_referenceresolution_x>
    <qualitysettings_referenceresolution_y>1080</qualitysettings_referenceresolution_y>
  </qualitySettings>
  <resourcesSettings>
    <resource_db>db/oracle.sqlite</resource_db>
    <resource_location>C:/Users/downstream/Documents/downstream/oracle/</resource_location>
    <resource_media>oracle/</resource_media>
  </resourcesSettings>
</UserData>
 ```

Unity 3D Version: 5.1.1f1

* NOTE: Recommend using same version unless upgrading to new version.

IDE Tools Used:

* Visual Studio 2013 Professional
* MonoDeveloper (Optional)

Unity Asset Tools Used:

* [Visual Studio 2013 Tools for Unity 1.9.9.0](https://visualstudiogallery.msdn.microsoft.com/20b80b8c-659b-45ef-96c1-437828fe7cf2)
  
* [TextMesh Pro v0.1.46 Beta 4.4](https://www.assetstore.unity3d.com/en/#!/content/17662)

* [Particle Playground v2.26](https://www.assetstore.unity3d.com/en/#!/content/13325)
		
* [Circular Gravity Force v3.07](https://www.assetstore.unity3d.com/en/#!/content/8181)

Remote Access and PC Configuration:

* They have to enable their HTTP proxy to establish a webex session, but the HTTP proxy prevents access to the CMS API so, the steps to connect via webex are:

  - Have them enable HTTP proxy, and start webex session
  - Log into webex session
  - Do anything you need to do that needs Internet access
  - Before the app can talk to the CMS AND/OR before disconnecting from webex make sure to turn off HTTP proxy
  

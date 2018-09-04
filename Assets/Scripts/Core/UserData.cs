/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Class objected used for reading and writing for the xml settings file.
*******************************************************************************************/
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml;

[DataContract]
public class UserData {
    [DataContract]
    public class Display {
        [DataMember]
        public bool window_override { get; set; }

        [DataMember]
        public bool window_fullscreen { get; set; }

        [DataMember]
        public int window_x { get; set; }

        [DataMember]
        public int window_y { get; set; }

        [DataMember]
        public float scrn_x_perc { get; set; }
    }

    [DataContract]
    public class ResourcesSettings {
        [DataMember]
        public string resource_location { get; set; }

        [DataMember]
        public string resource_db { get; set; }

        [DataMember]
        public string resource_media { get; set; }
    }

    [DataContract]
    public class AppSettings {
        [DataMember]
        public float attract_timeout { get; set; }

        [DataMember]
        public float poll_a_timeout { get; set; }

        [DataMember]
        public float poll_b_timeout { get; set; }

        [DataMember]
        public int poll_prepare_countdown { get; set; }

        [DataMember]
        public int poll_countdown { get; set; }

        [DataMember]
        public int min_takeaway_tweets { get; set; }

        [DataMember]
        public string app_refresh_time { get; set; }

        [DataMember]
        public float bezel_width { get; set; }

        [DataMember]
        public List<float> cube_spawn_offsets { get; set; }

        [DataMember]
        public List<string> player_color_by_order { get; set; }
    }

    [DataContract]
    public class AppDebugSettings {
        [DataMember]
        public bool debug_show_cursor { get; set; }

        [DataMember]
        public bool debug_show_logging { get; set; }

        [DataMember]
        public bool debug_show_bezels { get; set; }
    }

    [DataContract]
    public class MQTTSettings {
        [DataMember]
        public string mqtt_server_ip { get; set; }

        [DataMember]
        public string mqtt_topic { get; set; }
    }

    [DataContract]
    public class HTTPRequestSettings {
        [DataMember]
        public string url_request { get; set; }
    }

    [DataContract]
    public class QualitySettings {
        [DataMember]
        public int refResolution_x { get; set; }

        [DataMember]
        public int refResolution_y { get; set; }
    }

    [DataMember]
    public Display display = new Display();

    [DataMember]
    public ResourcesSettings resourcesSettings = new ResourcesSettings();

    [DataMember]
    public AppSettings appSettings = new AppSettings();

    [DataMember]
    public AppDebugSettings appDebugSettings = new AppDebugSettings();

    [DataMember]
    public MQTTSettings mqttSettings = new MQTTSettings();

    [DataMember]
    public HTTPRequestSettings httpRequestSettings = new HTTPRequestSettings();

    public UserData() {
        display = new Display();
        resourcesSettings = new ResourcesSettings();
        appSettings = new AppSettings();
        appDebugSettings = new AppDebugSettings();
        mqttSettings = new MQTTSettings();
        httpRequestSettings = new HTTPRequestSettings();
    }

    public void WriteUserDataFile(string path, UserData userData) {
        DataContractSerializer DCS = new DataContractSerializer(typeof(UserData));
        var settings = new XmlWriterSettings { Indent = true };

        using (var w = XmlWriter.Create(path, settings))
            DCS.WriteObject(w, userData);
    }

    public UserData OpenUserDataFile(string path) {
        UserData myNewNodes = null;

        DataContractSerializer DCS = new DataContractSerializer(typeof(UserData));
        using (FileStream fs = new FileStream(path, FileMode.Open)) {
            myNewNodes = (UserData)DCS.ReadObject(fs);
        }

        return myNewNodes;
    }
}
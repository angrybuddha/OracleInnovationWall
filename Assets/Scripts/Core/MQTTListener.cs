/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for listening to the MQTT signal to get the player points, and translates them.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Linq;

//Json
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//MQTT
using System.Net;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;

public class MQTTListener : MonoBehaviour
{
    public delegate void SetPlayersDirty();

    #region classis

    //Raw player data
    [Serializable]
    public class RawPlayer
    {
        //Player ID from the MQTT connection
        [SerializeField]
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        //Player x from the MQTT connection
        [SerializeField]
        private float x;
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        //Player y from the MQTT connection
        [SerializeField]
        private float y;
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
    }

    #endregion

    #region Properties

    //Enables the MQTT connection
    [SerializeField, Header("MQTT Settings:")]
    private bool enableMQTT = true;
    public bool EnableMQTT
    {
        get { return enableMQTT; }
        set { enableMQTT = value; }
    }

    //MQTT server IP
    [SerializeField]
    private string serverIP = "192.168.0.178";
    public string ServerIP
    {
        get { return serverIP; }
        set { serverIP = value; }
    }

    //MQTT Topic
    [SerializeField]
    private string mqttTopic = "stream/kinect";
    public string MQTTTopic
    {
        get { return mqttTopic; }
        set { mqttTopic = value; }
    }

    //Received messages as raw jason 
    [SerializeField, Header("Received Messages:")]
    private string rawJson = string.Empty;
    public string RawJson
    {
        get { return rawJson; }
        set { rawJson = value; }
    }

    //List of player raw data 
    [SerializeField]
    public List<RawPlayer> rawPlayer;
    public List<RawPlayer> RawPlayers
    {
        get { return rawPlayer; }
        set { rawPlayer = value; }
    }

    public event SetPlayersDirty OnSetPlayersDirty = null;

    //MQTT client object
    private MqttClient client;

    static MQTTListener m_instance = null;
    public static MQTTListener Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<MQTTListener>();
            }
            return m_instance;
        }
    }

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start()
    {
        if (enableMQTT)
        {
            SetupConnection();
        }
    }

    #endregion

    #region MQTT Functions

    //Sets up the connection
    void SetupConnection()
    {
        try
        {
            // create client instance 
            client = new MqttClient(ServerIP);

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] { MQTTTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            if (Core.Instance) {
                Core.Instance.SaveOutputLine(Core.DebugType.Log, string.Format("Connected to MQTT server: {0}, topic: {1}", ServerIP, MQTTTopic), true);
            }
            else{
                Debug.LogWarning("Connected to MQTT server: " + ServerIP + ", topic: " + MQTTTopic);
            }
        }
        catch (Exception)
        {
            if (Core.Instance) {
                Core.Instance.SaveOutputLine(Core.DebugType.Error, string.Format("Can't connect to MQTT server: {0}, topic: {1}", ServerIP, MQTTTopic), true);
            }
            else {
                Debug.LogWarning("Can't connect to MQTT server: " + ServerIP + ", topic: " + MQTTTopic);
            }
        }
    }

    //Closeds the MQTT connection
    void CloseConnection()
    {
        if (client != null)
        {
            try
            {
                if (client.IsConnected)
                {
                    client.Disconnect();
                }
            }
            catch (Exception)
            {
                if (Core.Instance) {
                    Core.Instance.SaveOutputLine(Core.DebugType.Error, string.Format("Can't close MQTT connection", ServerIP, MQTTTopic), true);
                }
                else {
                    Debug.LogWarning("Can't close MQTT connection: " + ServerIP + ", topic: " + MQTTTopic);
                }
            }
        }
    }

    //Translates the Json into the player list
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {
        RawJson = System.Text.Encoding.Default.GetString(e.Message);
        RawPlayers = JsonConvert.DeserializeObject<List<RawPlayer>>(RawJson);
        if (OnSetPlayersDirty != null) {
            OnSetPlayersDirty();
        }
    }

    //Closes connection on quiting the app
    void OnApplicationQuit()
    {
        CloseConnection();
    }

    //Closes connection on quiting the app
    void OnDisable()
    {
        CloseConnection();
    }

    //Closes connection on quiting the app
    void OnDestroy()
    {
        CloseConnection();
    }

    #endregion

    #region Functions

    //Syncs all raw player data
    public void SyncRawPlayers()
    {
        /*foreach (var rawPlayer in Core.Instance._mqttListener.RawPlayers)
        {
            var list =
            from g in Core.Instance._playerManager.Players
            where g.Id == rawPlayer.Id
            select g;

            foreach (var item in list)
            {
                float x = ((rawPlayer.X - .5f) * Core.Instance._settings.Kinect_area_X);
                float y = ((rawPlayer.Y - .5f) * Core.Instance._settings.Kinect_area_y);

                item.FollowTarget.position = new Vector3
                    (
                        Core.Instance._settings.Kinect_pivot.transform.position.x + x,
                        Core.Instance._settings.Kinect_pivot.transform.position.y,
                        Core.Instance._settings.Kinect_pivot.transform.position.z + y
                    );
                break;
            }
        }*/
    }

    //Checks if a player id exists
    public bool DoesIdExist(string Id)
    {
        var list =
        from r in RawPlayers
        where r.Id == Id
        select r;

        if (list.Count() > 0)
        {
            return true;
        }

        return false;
    }

    #endregion
}

/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Manages all active players.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayersManager : MonoBehaviour
{
    //Max number of plauers used
    [SerializeField, Header("Generate Info:")]
    private int maxPlayers;
    public int MaxPlayers
    {
        get { return maxPlayers; }
        set { maxPlayers = value; }
    }

    //The player prefab used for the player
    [SerializeField]
    private GameObject playerPrefab;
    public GameObject PlayerPrefab
    {
        get { return playerPrefab; }
        set { playerPrefab = value; }
    }

    //Cubie prefab used
    [SerializeField]
    private GameObject cubiePrefab;
    public GameObject CubiePrefab
    {
        get { return cubiePrefab; }
        set { cubiePrefab = value; }
    }

    //Min max scatter polt map 
    [SerializeField]
    private Vector2 minMaxScatterMap = Vector2.zero;
    public Vector2 MinMaxScatterMap
    {
        get { return minMaxScatterMap; }
        set { minMaxScatterMap = value; }
    }

    //Parent follow transform
    [SerializeField, Header("Follow Transform:")]
    private Transform parentFollowTrans;
    public Transform ParentPlayerTrans
    {
        get { return parentFollowTrans; }
        set { parentFollowTrans = value; }
    }

    //Players transform list
    [SerializeField]
    private List<Transform> playersTrans;
    public List<Transform> PlayersTrans
    {
        get { return playersTrans; }
        set { playersTrans = value; }
    }

    //Parent for Players
    [SerializeField, Header("Players Prefabs:")]
    private Transform parentPlayers;
    public Transform ParentPlayers
    {
        get { return parentPlayers; }
        set { parentPlayers = value; }
    }

    //Player list
    [SerializeField]
    private List<Player_Old> players;
    public List<Player_Old> Players
    {
        get { return players; }
        set { players = value; }
    }

    //Acitve player list
    [SerializeField]
    private List<Player_Old> activePlayers;
    public List<Player_Old> ActivePlayers
    {
        get { return activePlayers; }
        set { activePlayers = value; }
    }

    //Avg of all active players
    [SerializeField, Header("ACG Values: ")]
    private Vector3 avgActivePlayerVector = Vector3.zero;
    public Vector3 AvgActivePlayerVector
    {
        get { return avgActivePlayerVector; }
        set { avgActivePlayerVector = value; }
    }

    //Avg of all active players for scatterplot
    [SerializeField]
    private float avgScatterValue = 0f;
    public float AvgScatterValue
    {
        get { return avgScatterValue; }
        set { avgScatterValue = value; }
    }

    //Draws the avg location in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AvgActivePlayerVector, .25f);
    }

    // Use this for initialization
    void Start()
    {
        //Creates Players
        CreateFollowTrans();
        CreatePlayers();

        Core.Instance._spawnManager.PlayerCubes.MaxCount = MaxPlayers;
        Core.Instance._spawnManager.PlayerCubes.SetupSpawnPool();
    }

    // Update is called once per frame
    void Update()
    {
        AvgActivePlayerVector = GetMeanVector();
        AvgScatterValue = Map(AvgActivePlayerVector.x, MinMaxScatterMap.x, MinMaxScatterMap.y);
    }

    //Creates follow gameobjects
    void CreateFollowTrans()
    {
        for (int i = 0; i < Core.Instance._playerManager.MaxPlayers; i++)
        {
            string playerId = (i + 1).ToString();

            var newPlayer = new GameObject(string.Format("{0} {1}", "Follow:", playerId));
            newPlayer.transform.SetParent(ParentPlayerTrans.transform, false);

            PlayersTrans.Add(newPlayer.transform);
        }
    }

    //Creates player gameobjects
    void CreatePlayers()
    {
        for (int i = 0; i < MaxPlayers; i++)
        {
            var newPlayer = Instantiate(PlayerPrefab) as GameObject;
            var player = newPlayer.GetComponent<Player_Old>();

            newPlayer.transform.SetParent(ParentPlayers.transform, false);

            string playerId = (i + 1).ToString();
            player.Id = playerId;
            player.FollowTarget = PlayersTrans[i];

            newPlayer.name = string.Format("{0} {1}", "Player:", playerId);

            Players.Add(player);
        }
    }

    //Resets all plauer mode times
    public void ResetAllPlayerModeTime()
    {
        foreach (var player in Players)
        {
            player.GetComponent<Player_Old>().ResetModeTime();
        }
    }

    //Gets the mean vector for all active players
    private Vector3 GetMeanVector()
    {
        IEnumerable<Player_Old> list = null;

        switch (Core.Instance._state)
        {
            case Core.WallState.Ambient:
                list = from p in ActivePlayers
                       select p;
                break;
            case Core.WallState.Question:
                list = from p in ActivePlayers
                       where p.Mode == Player_Old.PlayerMode.Engaged
                       select p;
                break;
        }

        //if (list.Count() == 0)
            //return Core.Instance._playerManager.cubiePrefab.GetComponent<Cubie>().IdleActiveLocation.position;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (var player in list)
        {
            x += player.Hand.position.x;
            y += player.Hand.position.y;
            z += player.Hand.position.z;
        }
        return new Vector3(x / list.Count(), y / list.Count(), z / list.Count());
    }

    //Map function
    float Map(float value, float inputMin, float inputMax, float outputMin = 0f, float outputMax = 1f, bool clamp = true)
    {

        if (Mathf.Abs(inputMin - inputMax) < 0.000001f)
        {
            return outputMin;
        }
        else
        {
            float outVal = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);

            if (clamp)
            {
                if (outputMax < outputMin)
                {
                    if (outVal < outputMax) outVal = outputMax;
                    else if (outVal > outputMin) outVal = outputMin;
                }
                else
                {
                    if (outVal > outputMax) outVal = outputMax;
                    else if (outVal < outputMin) outVal = outputMin;
                }
            }
            return outVal;
        }
    }
}
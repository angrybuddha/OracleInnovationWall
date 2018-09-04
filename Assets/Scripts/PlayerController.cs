/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 07-01-16 
* 
* Description: 
*   Used for managing all player cube events.
*******************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
    [System.NonSerialized]
    public GameObject[] CubePlayers;

    public Color[] all_colors;

    public GameObject player_cube;

    public Camera camera_cube;

    private static PlayerController _instance;
    private bool INIT = false;

    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<PlayerController>();

            return _instance;
        }
    }

	void Start () {


        //SET ALL THE COLORS FOR PLAYER CUBES

        all_colors = new Color[10];
        all_colors[0] = convertColor(208, 66, 47);
        all_colors[1] = convertColor(0, 112, 141);
        all_colors[2] = convertColor(215, 138, 54);
        all_colors[3] = convertColor(209, 169, 116);
        all_colors[4] = convertColor(15, 121, 49);
        all_colors[5] = convertColor(115, 5, 48);
        all_colors[6] = convertColor(56, 84, 126);
        all_colors[7] = convertColor(126, 153, 170);
        all_colors[8] = convertColor(184, 186, 188);
        all_colors[9] = convertColor(0, 0, 0);
        
        BuildPlayers();
	}

    void BuildPlayers()
    {
        
        CubePlayers = new GameObject[10];

        for (int i = 0; i < 10; i++)
        {

            GameObject clone = Instantiate(player_cube, new Vector3(-5 + i, 0, 0), transform.rotation) as GameObject;

            clone.GetComponent<PlayerCube>().setColor(all_colors[i]);
            clone.GetComponent<PlayerCube>().id = i;

            //ATTACH TO PARENT
            //clone.transform.parent = transform;

            CubePlayers[i] = clone;

            //TEMP PLEASE REMOVE - FOR TESTING ONLY
            //if(i != 0)
            clone.SetActive(false);
        }

        INIT = true;
    }

    Color convertColor(int r, int g, int b)
    {
        return new Color(r/255.0F, g/255.0F, b/255.0F);
    }

    public GameObject getActivePlayer(int r1, int r2)
    {

        GameObject player = new GameObject() ;

        //FINDS AND RETURNS THE PLAYER WITHIN THE REGION SPECIFIED

        foreach (GameObject p in CubePlayers)
        {
            if (p.GetComponent<PlayerCube>().region_id == r1 || p.GetComponent<PlayerCube>().region_id == r2)
            {
                player = p;
            }
        }

        return player;
    }

    public void hideAllCubes()
    {
        foreach (GameObject player in CubePlayers)
        {
            player.GetComponent<PlayerCube>().hideCube();
        }
    }

    public void showAllCubes()
    {
        foreach (GameObject player in CubePlayers)
        {
            player.GetComponent<PlayerCube>().showCube();
        }
    }


    void activePlayer(int id, bool active_state)
    {
        CubePlayers[id].SetActive(active_state);
    }

    void updatePlayer(int _id, float _x, float _y)
    {

        if(CubePlayers[_id - 1].activeSelf)
            CubePlayers[_id-1].GetComponent<PlayerCube>().updatePosition(_x, _y);
        //Debug.Log("CAMERA_WORLD " + camera_cube.WorldToScreenPoint(CubePlayers[_id - 1].GetComponent<PlayerCube>().transform.position));
        //Debug.Log("CUBE POSITION " + _x);
    }

    void doUpdate(int _id, string func)
    {
        if (func == "typeA")
            CubePlayers[_id - 1].GetComponent<PlayerCube>().doCountdown();
        if (func == "typeB")
        {

            CubePlayers[_id - 1].GetComponent<PlayerCube>().doCountdown();
            CubePlayers[_id - 1].GetComponent<PlayerCube>().showArrow();

        }
        else if (func == "align_to_cluster")

            CubePlayers[_id - 1].transform.position = new Vector3(7.45F, 1, 0);

        else if (func == "scale_down")
        {
            CubePlayers[_id - 1].GetComponent<PlayerCube>().scaleDown();
            CubePlayers[_id - 1].GetComponent<PlayerCube>().hidePerson();
        }
        else if (func == "scale_up")
        {

            CubePlayers[_id - 1].GetComponent<PlayerCube>().scaleUp();

        }
            
        //snaps the TYPE B (Scatterplot) to underneath its row/answer
        else if (func == "snap_B_col")
            CubePlayers[_id - 1].GetComponent<PlayerCube>().snapToCol();

        else if (func == "hideAll")
            CubePlayers[_id - 1].GetComponent<PlayerCube>().hideAll();
    }

    //FUNCTION TO GATHER ALL VALUES OF ACTIVE CUBES
    /*public void sweepValues()
    {
        foreach (int cube in MainController.Instance.active_player_list)
        {

        }
    }*/

    // 
    void Update () {

	    if(INIT)
        { 
            //SETS THE ACTIVE REGIONS FOR THE MAIN CONTROLLER
            MainController.region_occupied = new bool[8];

            foreach(GameObject player in CubePlayers)
            {
                if (player.activeSelf)
                {
                    //HIGHLIGHT REGION
                    MainController.region_occupied[player.GetComponent<PlayerCube>().region_id] = true;

                }
            }

        }
    }

    //void UpdateAfter()
    //{

    //}

    void OnDisable()
    {
        stopListen();
    }

    void OnEnable()
    {
        listen();
    }

    void listen()
    {
        MainController.MovePlayer -= updatePlayer;
        MainController.MovePlayer += updatePlayer;

        MainController.UpdatePlayer -= doUpdate;
        MainController.UpdatePlayer += doUpdate;

        MainController.ActivatePlayer -= activePlayer;
        MainController.ActivatePlayer += activePlayer;
    }

    void stopListen()
    {
        MainController.UpdatePlayer -= doUpdate;
        MainController.MovePlayer -= updatePlayer;
        MainController.ActivatePlayer -= activePlayer;
    }
}

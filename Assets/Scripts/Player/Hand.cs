/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used as the players hand.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

    #region Properties

    //Hand target
    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    //Icon used for the scatterplot
    [SerializeField]
    private GameObject iconPrefab;
    public GameObject IconPrefab
    {
        get { return iconPrefab; }
        set { iconPrefab = value; }
    }

    //Leap speed
    [SerializeField]
    private float lerpSpeed = 8f;
    public float LerpSpeed
    {
        get { return lerpSpeed; }
        set { lerpSpeed = value; }
    }

    //lock x flag
    [SerializeField]
    private bool lockX = false;
    public bool LockX
    {
        get { return lockX; }
        set { lockX = value; }
    }

    //Lock y flag
    [SerializeField]
    private bool lockY = false;
    public bool LockY
    {
        get { return lockY; }
        set { lockY = value; }
    }

    //Lock z flag
    [SerializeField]
    private bool lockZ = false;
    public bool LockZ
    {
        get { return lockZ; }
        set { lockZ = value; }
    }

    //Used for a position offset
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    public Vector3 Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    //Used for a position offset multiplier
    [SerializeField]
    private Vector3 offsetMultiplier = Vector3.one;
    public Vector3 OffsetMultiplier
    {
        get { return offsetMultiplier; }
        set { offsetMultiplier = value; }
    }

    private Player_Old player;
    private GameObject icon;

    #endregion

    #region Unity Functions

    void Start()
    {
        player = Target.GetComponent<Player_Old>();
        icon = Instantiate(IconPrefab);
        icon.GetComponent<UICanvas_CubeIcon_Bind>().PlayerRef = player;
        icon.transform.SetParent(this.transform, false);
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector3 newLocation = new Vector3
                (
                    (Target.position.x * OffsetMultiplier.x) + Offset.x, 
                    (Target.position.y * OffsetMultiplier.y) + Offset.y, 
                    (Target.position.z * OffsetMultiplier.z) + Offset.z
                );

            if (LockX)
            {
                newLocation.x = transform.position.x;
            }
            if (LockY)
            {
                newLocation.y = transform.position.y;
            }
            if (LockZ)
            {
                newLocation.z = transform.position.z;
            }

            Vector3 moveTo = Vector3.Lerp(transform.position, newLocation, Time.deltaTime * LerpSpeed);

            if (Target != null)
            {
                transform.position = moveTo;
            }
        }

        icon.SetActive(Core.Instance._state == Core.WallState.Question);

        icon.transform.position = new Vector3(this.transform.position.x, -2f, 4f);
    }

    #endregion
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PollGraph : MonoBehaviour {

    int maxX = 40;
    int totalVal;
    int px;

    List<GameObject> cubes_array = new List<GameObject>();
    List<float> snap_col_list = new List<float>();

	// Use this for initialization
	public void build (List<int> answer_arr) {

        if(cubes_array.Count > 0)
            Reset();

        maxX = answer_arr.Count;

        //Debug.Log("LEngh of answer " + answer_arr.Count);

        for(px = 0; px < maxX; ++px)
        {

            //CALCULATE TOTAL VALUE FOR ALL SLOTS
            totalVal += answer_arr[px];

        }

        Debug.Log("TOTAL " + totalVal);

        for (px = 0; px < maxX; ++px)
        {
            
            //THESE ARE THE X POSTIONS FOR THE COLS
            snap_col_list.Add(((float)px -20)/3);

            //BUILD COL BASED ON PCT
            buildCol(pxPercentage(answer_arr[px]));

        }

        playIntro();

	}

    void Reset()
    {
        totalVal = 0;

        foreach(GameObject cube in cubes_array)
        {
            Destroy(cube);
        }

        cubes_array = new List<GameObject>();

    }

    int pxPercentage(int val)
    {

        //RETURNS PCT IN INT FORM (ROUNDED)
        int pct = System.Convert.ToInt32(Mathf.Round(40.0f*(float)val/totalVal));

        if (pct == 0)//FOR BEAUTY
            pct = 1;

        return pct;

    }


    void buildCol(int value)
    {

        for (int py = 0; py < value; ++py)
        {

            GameObject cube = MakeCube(py);
            cubes_array.Add(cube);

            //Debug.Log("Adding cube");

            if(py == value - 1)
            {

                //cube.GetComponent<MeshRenderer>().material.color = Color.red;

            }

        }

    }

    public void playIntro()
    {
        //doReset();
        int count = 0;

        foreach (GameObject cube in cubes_array)
        {

            //float x = cube.transform.position.x;
            //cube.transform.DOMove(new Vector3(x, 0, 0), 1).From();
            count++;
            cube.transform.DOScale(0, 0.5f).From().SetDelay(count*0.01F);

        }

    }


    GameObject MakeCube(float ypos)
    {

        //Debug.Log(xpos + ", " + ypos +  ", " + zpos);
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.layer = 9;
        // color is controlled like this
        cube.GetComponent<MeshRenderer>().material.color = Color.white; // for example
        cube.GetComponent<Transform>().Translate(((float)px - 20)/3, ypos/3, 0);
        cube.GetComponent<Transform>().localScale = new Vector3(0.3F, 0.3F, 0.3F);
        cube.transform.parent = gameObject.transform;

        // There are lots more colours to choose
        return cube;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {
        //StopAllCoroutines();
    }

    void OnEnable()
    {
        //playIntro();
    }
}

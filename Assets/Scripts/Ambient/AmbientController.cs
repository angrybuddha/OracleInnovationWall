using UnityEngine;
using System.Collections;

public class AmbientController : MonoBehaviour {

    int maxY = 8;

    // Use this for initialization
	void Start ()
    {

        //MainController.TwitterResults += buildCubes;

        buildCubeRegions();

    }

    void buildCubeRegions()
    {
        for (int py = 0; py < maxY; ++py)
        {
            //BUILD CUBE REGION
            //CubeRegion cube_region = new CubeRegion();

            //STORE IN ARRAY
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

	}


}

using UnityEngine;
using System.Collections;

public class PCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}

    public void setColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

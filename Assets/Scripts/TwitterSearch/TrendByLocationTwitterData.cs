using System.Collections;
using UnityEngine;
using System;

public class TrendByLocationTwitterData {
	public string name = "";
	public string query = "";
	public string url = "";
		
	public override string ToString(){
		return "Name: " + name + " | Query: " + query + " | Url: " + url;
	}
}
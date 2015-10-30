using UnityEngine;
using System.Collections;

public class FogColor : MonoBehaviour {
    public Color fogColor;

	// Use this for initialization
	void Start () {
        RenderSettings.fogColor = fogColor;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

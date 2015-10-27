using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour {

    LineRenderer myRenderer;
    public Transform TargetA;
    public Transform TargetB;
    // Use this for initialization
    void Start () {
        myRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        myRenderer.SetPosition(0, TargetA.position);
        myRenderer.SetPosition(1, TargetB.position);
    }
}

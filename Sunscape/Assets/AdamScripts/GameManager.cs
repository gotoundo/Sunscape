using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Main;
    public GameObject Player;
    public Camera MainCamera;
    public Terrain MainTerrain;
    public TerrainModification terrainModification;

	// Use this for initialization
	void Start () {
        Main = this;
        Cursor.visible = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        GameObject TerrainObject = GameObject.FindGameObjectWithTag("Terrain");
        if (TerrainObject != null)
        {
            MainTerrain = TerrainObject.GetComponent<Terrain>();
            terrainModification = TerrainObject.GetComponent<TerrainModification>();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadLevel(string LevelName)
    {
        ExitLevel();
        Application.LoadLevel(LevelName);
    }

    void OnApplicationQuit()
    {
        ExitLevel();
    }

    void ExitLevel()
    {
        if (terrainModification != null)
            terrainModification.RevertChanges();
    }


}

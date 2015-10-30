using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Main;
    public GameObject Player;
    public Camera MainCamera;
    public Terrain MainTerrain;
    public TerrainModification terrainModification;
    public PhysicMaterial playerMaterialOverride;
    public float playerDragOverride = -1;

    public Vector3 Gravity = new Vector3(0, -9.81f, 0);
    public float playerSpeedModifier = 1;
    

	// Use this for initialization
	void Awake () {
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

        if (playerMaterialOverride != null)
            Player.GetComponent<Collider>().material = playerMaterialOverride;
        if (playerDragOverride != -1)
            Player.GetComponent<Rigidbody>().drag = playerDragOverride;

        Physics.gravity = Gravity*2;
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

    void OnTriggerExit(Collider col)
    {
        Destroy(col.gameObject);
    }



}

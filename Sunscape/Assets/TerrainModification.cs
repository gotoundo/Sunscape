using UnityEngine;
using System.Collections;

public class TerrainModification : MonoBehaviour
{

    public static TerrainModification Main;

    public Terrain terrain;
    TerrainData terrainData;

    Vector3 terrainPos;
   // GameObject Marker;

    float[,] initialHeights;


    // Use this for initialization
    void Awake()
    {
        Main = this;
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;

        initialHeights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
    }

    public void RevertChanges()
    {
        terrainData.SetHeights(0, 0, initialHeights);
    }



    public void AltitudeSquare(Vector3 position, float altitudeChange = .0001f, int width = 10, int height = 10)
    {
        int xLoc = Mathf.RoundToInt((position.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
        int yLoc = Mathf.RoundToInt((position.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

        int startX = xLoc - width / 2;
        int startY = yLoc - height / 2;

        float[,] heights = terrainData.GetHeights(startX, startY, width, height);

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float adjustedAltitudeChange = altitudeChange;
                float xDistance = Mathf.Abs(x - width / 2);
                float zDistance = Mathf.Abs(z - height / 2);
                Vector2 relativePosition = (new Vector2(xDistance, zDistance)).normalized;
                relativePosition = (new Vector2(1, 1)) - relativePosition;

                heights[x, z] += adjustedAltitudeChange * relativePosition.sqrMagnitude;
            }
        }

        terrainData.SetHeights(startX, startY, heights);
    }

    void SpikeTerrain()
    {
        int xBase = 0;
        int yBase = 0;

        int heightmapWidth = terrainData.heightmapWidth;
        int heightmapHeight = terrainData.heightmapHeight;
        float[,] heights = terrainData.GetHeights(xBase, yBase, heightmapWidth, heightmapHeight);

        for (int z = 0; z < heightmapHeight; z++)
        {
            for (int x = 0; x < heightmapWidth; x++)
            {
                float cos = Mathf.Cos(x);
                float sin = -Mathf.Sin(z);
                heights[x, z] = (cos - sin) / 250;
            }
        }
        terrainData.SetHeights(xBase, yBase, heights);
    }

    public void SetTerrain(int xPos, int zPos, int width, int height, float altitude)
    {
        int heightmapWidth = terrainData.heightmapWidth;
        int heightmapHeight = terrainData.heightmapHeight;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);


        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                heights[z, x] += altitude;

        terrainData.SetHeights(xPos, zPos, heights);
    }

    public void SetTerrainMap(float[,] newHeights)
    {

        terrainData.SetHeights(0,0,newHeights);
        /*int heightmapWidth = terrainData.heightmapWidth;
        int heightmapHeight = terrainData.heightmapHeight;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);


        for(int x = 0; x < newHeights.GetLength(0); x++)
            for(int y = 0; y < newHeights.GetLength(0); y++)

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                heights[z, x] += altitude;

        terrainData.SetHeights(xPos, zPos, heights);*/
    }

    public void AdjustTerrainMap(float[,] newHeights)
    {
        int heightmapWidth = terrainData.heightmapWidth;
        int heightmapHeight = terrainData.heightmapHeight;
        float[,] adjustedHeights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
        for (int i = 0; i < newHeights.GetLength(0); i++)
            for (int j = 0; j < newHeights.GetLength(0); j++)
                adjustedHeights[i, j] += newHeights[i, j];

        terrainData.SetHeights(0,0,adjustedHeights);

    }


}

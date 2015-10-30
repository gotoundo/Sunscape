using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit;
using ProceduralToolkit.Examples;
using UnityEngine.UI;
using ProceduralToolkit.Examples.UI;

public class LabyrinthGenerator : UIBase
{
    public GameObject[] MazeOccupants;

    public RectTransform leftPanel;
    public ToggleGroup algorithmsGroup;
    public RawImage mazeImage;

    private Texture2D texture;
    private int mapWidth;
    private int mapHeight;
    public int cellSize = 5;
    public int wallSize = 5;
    public float wallHeight = 0.01f;
    public float floorHeight = 0f;
    float[,] heights;

    private bool useRainbowGradient = true;
    private MazeGenerator mazeGenerator;
    public MazeGenerator.Algorithm generatorAlgorithm = MazeGenerator.Algorithm.RandomTraversal;

    //Discoverables
    private MazeGenerator.Algorithm[] algorithms = new[]
    {
            MazeGenerator.Algorithm.None,
            MazeGenerator.Algorithm.RandomTraversal,
            MazeGenerator.Algorithm.RandomDepthFirstTraversal,
            MazeGenerator.Algorithm.RandomBreadthFirstTraversal,
        };

    private Dictionary<MazeGenerator.Algorithm, string> algorithmToString =
        new Dictionary<MazeGenerator.Algorithm, string>
        {
                {MazeGenerator.Algorithm.None, "None"},
                {MazeGenerator.Algorithm.RandomTraversal, "Random traversal"},
                {MazeGenerator.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal"},
                {MazeGenerator.Algorithm.RandomBreadthFirstTraversal, "Random breadth-first traversal"}
        };



    void SetObjectToMazePosition(GameObject mazeObject, int xPos, float yPos, int zPos)
    {
        TerrainData data = GameManager.Main.MainTerrain.terrainData;
        float xPosReal = xPos * data.size.x / data.heightmapWidth;
        float zPosReal = zPos * data.size.z / data.heightmapHeight;
        mazeObject.transform.position = new Vector3(xPosReal, yPos, zPosReal);
        mazeObject.name += "Maze Location - x: " + xPos + ", z:" + zPos;
        mazeObject.name += "  Real Location - x: " + xPosReal + ", z:" + zPosReal;
    }

    private void PlaceDiscoverables()
    {
        TerrainData data = GameManager.Main.MainTerrain.terrainData;

       /* for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int z = 0; z < heights.GetLength(1); z++)
            {
                if (heights[x, z] == floorHeight)
                {
                    GameObject o = Instantiate(MazeOccupants[1]);
                    o.transform.localScale *= 20;
                    SetObjectToMazePosition(o, z, 20, x); //WHY DOES THIS HAVE TO BE REVERSED WHO KNOWS???
                }
            }

        }*/

        foreach (GameObject mazeObject in MazeOccupants)
        {
            bool locationFound = false;
            while (!locationFound)
            {
                int xLocCandidate = Random.Range(0, heights.GetLength(0)-1);
                int zLocCandidate = Random.Range(0, heights.GetLength(1)-1);

               // xLocCandidate = Mathf.Min(heights.GetLength(0)-1, xLocCandidate + xLocCandidate % 3);
             //   zLocCandidate = Mathf.Min(heights.GetLength(1) - 1, zLocCandidate + zLocCandidate % 3);

                if (heights[xLocCandidate, zLocCandidate] == floorHeight)
                {
                    bool wallsTooClose = false;

                    for (int i = -1; i < 2; i++)
                    {   
                        for (int j = -1; j < 2; j++)
                        {
                            if (heights[xLocCandidate + i, zLocCandidate + j] != floorHeight)
                                wallsTooClose = true;
                        }
                    }

                    if (!wallsTooClose)
                    {
                        SetObjectToMazePosition(mazeObject, zLocCandidate, 303, xLocCandidate);  //WHY DOES THIS HAVE TO BE REVERSED?? WHO KNOWS???
                        locationFound = true;
                    }
                }
            }
        }

    }

    private void Start()
    {
        mapWidth = GameManager.Main.MainTerrain.terrainData.heightmapWidth;
        mapHeight = GameManager.Main.MainTerrain.terrainData.heightmapHeight;

        var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
        header.Initialize("Generator algorithm");
        header.transform.SetAsFirstSibling();

        foreach (MazeGenerator.Algorithm algorithm in algorithms)
        {
            var toggle = InstantiateControl<ToggleControl>(algorithmsGroup.transform);
            toggle.Initialize(
                header: algorithmToString[algorithm],
                value: algorithm == generatorAlgorithm,
                onValueChanged: isOn =>
                {
                    if (isOn)
                    {
                        generatorAlgorithm = algorithm;
                    }
                },
                toggleGroup: algorithmsGroup);
        }

        var cellSizeSlider = InstantiateControl<SliderControl>(leftPanel);
        cellSizeSlider.Initialize("Cell size", 1, 10, cellSize, value => cellSize = value);

        var wallSizeSlider = InstantiateControl<SliderControl>(leftPanel);
        wallSizeSlider.Initialize("Wall size", 1, 10, wallSize, value => wallSize = value);

        var useRainbowGradientToggle = InstantiateControl<ToggleControl>(leftPanel);
        useRainbowGradientToggle.Initialize("Use rainbow gradient", useRainbowGradient,
            value => useRainbowGradient = value);

        var generateButton = InstantiateControl<ButtonControl>(leftPanel);
        generateButton.Initialize("Generate new maze", Generate);

        Generate();
        
    }

    private void Generate()
    {
        StopAllCoroutines();

        texture = new Texture2D(mapWidth, mapHeight, TextureFormat.ARGB32, false, true)
        {
            filterMode = FilterMode.Point
        };
        texture.Clear(Color.black);
        texture.Apply();
        mazeImage.texture = texture;

        mazeGenerator = new MazeGenerator(mapWidth, mapHeight, cellSize, wallSize);

        StartCoroutine(GenerateCoroutine());
    }


    

    private IEnumerator GenerateCoroutine()
    {
        var algorithm = generatorAlgorithm;
        if (algorithm == MazeGenerator.Algorithm.None)
        {
            algorithm = algorithms.GetRandom();
        }

        heights = new float[mapWidth,mapHeight];//textureWidth * cellSize, textureHeight * cellSize
        for (int i = 0; i < heights.GetLength(0); i++)
            for (int j = 0; j < heights.GetLength(1); j++)
                heights[i, j] = wallHeight;

        switch (algorithm)
        {
            case MazeGenerator.Algorithm.RandomTraversal:
                yield return StartCoroutine(mazeGenerator.RandomTraversal(DrawEdge, texture.Apply));
                break;
            case MazeGenerator.Algorithm.RandomDepthFirstTraversal:
                yield return StartCoroutine(mazeGenerator.RandomDepthFirstTraversal(DrawEdge, texture.Apply));
                break;
            case MazeGenerator.Algorithm.RandomBreadthFirstTraversal:
                yield return StartCoroutine(mazeGenerator.RandomBreadthFirstTraversal(DrawEdge, texture.Apply));
                break;
        }


        texture.Apply();
        heights[0, 0] = 2;
        heights[heights.GetLength(0)-1, heights.GetLength(1)-1] = -2;
        if (GameManager.Main != null && GameManager.Main.terrainModification != null)
            GameManager.Main.terrainModification.AdjustTerrainMap(heights);

        PlaceDiscoverables();
    }

    private void DrawEdge(Edge edge)
    {
        int x, y, width, height;
        if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Down)
        {
            x = Translate(edge.exit.x);
            y = Translate(edge.exit.y);
        }
        else
        {
            x = Translate(edge.origin.x);
            y = Translate(edge.origin.y);
        }

        if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Right)
        {
            width = cellSize * 2 + wallSize;
            height = cellSize;
        }
        else
        {
            width = cellSize;
            height = cellSize * 2 + wallSize;
        }

        Color color;
        if (useRainbowGradient)
        {
            float hue = Mathf.Repeat(edge.origin.depth / 360f, 1);
            color = ColorE.HSVToRGB(hue, 1, 1);
        }
        else
        {
            color = Color.white;
        }

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                heights[x + i, y + j] = floorHeight;

       
        texture.DrawRect(x, y, width, height, color);
    }

    private int Translate(int x)
    {
        return wallSize + x * (cellSize + wallSize);
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using Utility;


/// <summary>
/// A double key dictionary.
/// </summary>
/// <typeparam name="K">
/// The first key type.
/// </typeparam>
/// <typeparam name="T">
/// The second key type.
/// </typeparam>
/// <typeparam name="V">
/// The value type.
/// </typeparam>
/// <remarks>
/// See http://noocyte.wordpress.com/2008/02/18/double-key-dictionary/
/// A Remove method was added.
/// </remarks>
public class DoubleKeyDictionary<K, T, V> : IEnumerable<DoubleKeyPairValue<K, T, V>>,
                                            IEquatable<DoubleKeyDictionary<K, T, V>>
{
    /// <summary>
    /// The m_inner dictionary.
    /// </summary>
    private Dictionary<T, V> m_innerDictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleKeyDictionary{K,T,V}"/> class.
    /// </summary>
    public DoubleKeyDictionary()
    {
        this.OuterDictionary = new Dictionary<K, Dictionary<T, V>>();
    }

    /// <summary>
    /// Gets or sets OuterDictionary.
    /// </summary>
    private Dictionary<K, Dictionary<T, V>> OuterDictionary { get; set; }

    /// <summary>
    /// Gets or sets the value with the specified indices.
    /// </summary>
    /// <value></value>
    public V this[K index1, T index2]
    {
        get
        {
            return this.OuterDictionary[index1][index2];
        }

        set
        {
            this.Add(index1, index2, value);
        }
    }

    /// <summary>
    /// Clears this dictionary.
    /// </summary>
    public void Clear()
    {
        OuterDictionary.Clear();
        if (m_innerDictionary != null)
            m_innerDictionary.Clear();
    }

    /// <summary>
    /// Adds the specified key.
    /// </summary>
    /// <param name="key1">
    /// The key1.
    /// </param>
    /// <param name="key2">
    /// The key2.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    public void Add(K key1, T key2, V value)
    {
        if (this.OuterDictionary.ContainsKey(key1))
        {
            if (this.m_innerDictionary.ContainsKey(key2))
            {
                this.OuterDictionary[key1][key2] = value;
            }
            else
            {
                this.m_innerDictionary = this.OuterDictionary[key1];
                this.m_innerDictionary.Add(key2, value);
                this.OuterDictionary[key1] = this.m_innerDictionary;
            }
        }
        else
        {
            this.m_innerDictionary = new Dictionary<T, V>();
            this.m_innerDictionary[key2] = value;
            this.OuterDictionary.Add(key1, this.m_innerDictionary);
        }
    }

    /// <summary>
    /// Determines whether the specified dictionary contains the key.
    /// </summary>
    /// <param name="index1">
    /// The index1.
    /// </param>
    /// <param name="index2">
    /// The index2.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified index1 contains key; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsKey(K index1, T index2)
    {
        if (!this.OuterDictionary.ContainsKey(index1))
        {
            return false;
        }

        if (!this.OuterDictionary[index1].ContainsKey(index2))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Equalses the specified other.
    /// </summary>
    /// <param name="other">
    /// The other.
    /// </param>
    /// <returns>
    /// The equals.
    /// </returns>
    public bool Equals(DoubleKeyDictionary<K, T, V> other)
    {
        if (this.OuterDictionary.Keys.Count != other.OuterDictionary.Keys.Count)
        {
            return false;
        }

        bool isEqual = true;

        foreach (var innerItems in this.OuterDictionary)
        {
            if (!other.OuterDictionary.ContainsKey(innerItems.Key))
            {
                isEqual = false;
            }

            if (!isEqual)
            {
                break;
            }

            // here we can be sure that the key is in both lists,
            // but we need to check the contents of the inner dictionary
            Dictionary<T, V> otherInnerDictionary = other.OuterDictionary[innerItems.Key];
            foreach (var innerValue in innerItems.Value)
            {
                if (!otherInnerDictionary.ContainsValue(innerValue.Value))
                {
                    isEqual = false;
                }

                if (!otherInnerDictionary.ContainsKey(innerValue.Key))
                {
                    isEqual = false;
                }
            }

            if (!isEqual)
            {
                break;
            }
        }

        return isEqual;
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>
    /// </returns>
    public IEnumerator<DoubleKeyPairValue<K, T, V>> GetEnumerator()
    {
        foreach (var outer in this.OuterDictionary)
        {
            foreach (var inner in outer.Value)
            {
                yield return new DoubleKeyPairValue<K, T, V>(outer.Key, inner.Key, inner.Value);
            }
        }
    }

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key1">
    /// The key1.
    /// </param>
    /// <param name="key2">
    /// The key2.
    /// </param>
    public void Remove(K key1, T key2)
    {
        this.OuterDictionary[key1].Remove(key2);
        if (this.OuterDictionary[key1].Count == 0)
        {
            this.OuterDictionary.Remove(key1);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

}

/// <summary>
/// Represents two keys and a value.
/// </summary>
/// <typeparam name="K">
/// First key type.
/// </typeparam>
/// <typeparam name="T">
/// Second key type.
/// </typeparam>
/// <typeparam name="V">
/// Value type.
/// </typeparam>
public class DoubleKeyPairValue<K, T, V>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleKeyPairValue{K,T,V}"/> class.
    /// </summary>
    /// <param name="key1">
    /// The key1.
    /// </param>
    /// <param name="key2">
    /// The key2.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    public DoubleKeyPairValue(K key1, T key2, V value)
    {
        this.Key1 = key1;
        this.Key2 = key2;
        this.Value = value;
    }

    /// <summary>
    /// Gets or sets the key1.
    /// </summary>
    /// <value>The key1.</value>
    public K Key1 { get; set; }

    /// <summary>
    /// Gets or sets the key2.
    /// </summary>
    /// <value>The key2.</value>
    public T Key2 { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public V Value { get; set; }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return this.Key1 + " - " + this.Key2 + " - " + this.Value;
    }

}





public class TerrainManager : MonoBehaviour {

    public GameObject playerGameObject;
    public Terrain referenceTerrain;
    public int TERRAIN_BUFFER_COUNT = 50;
    public int spread = 1;

    private int[] currentTerrainID;
    private Terrain[] terrainBuffer;
    private DoubleKeyDictionary<int, int, int> terrainUsage;
    private DoubleKeyDictionary<int, int, TerrainData> terrainUsageData;
    private BitArray usedTiles;
    private BitArray touchedTiles;
    private Vector3 referencePosition;
    private Vector2 referenceSize;
    private Quaternion referenceRotation;

    // Use this for initialization
    void Start()
    {
        currentTerrainID = new int[2];
        terrainBuffer = new Terrain[TERRAIN_BUFFER_COUNT];
        terrainUsage = new DoubleKeyDictionary<int, int, int>();
        terrainUsageData = new DoubleKeyDictionary<int, int, TerrainData>();
        usedTiles = new BitArray(TERRAIN_BUFFER_COUNT, false);
        touchedTiles = new BitArray(TERRAIN_BUFFER_COUNT, false);

        referencePosition = referenceTerrain.transform.position;
        referenceRotation = referenceTerrain.transform.rotation;
        referenceSize = new Vector2(referenceTerrain.terrainData.size.x, referenceTerrain.terrainData.size.z);

        for (int i = 0; i < TERRAIN_BUFFER_COUNT; i++)
        {

            terrainBuffer[i] = Instantiate(referenceTerrain);
            /*
            TerrainData tData = new TerrainData();
            CopyTerrainDataFromTo(referenceTerrain.terrainData, ref tData);


            terrainBuffer[i] = Terrain.CreateTerrainGameObject(tData).GetComponent<Terrain>();
            terrainBuffer[i].castShadows = false;
            terrainBuffer[i].treeMaximumFullLODCount = 2000;
            terrainBuffer[i].treeCrossFadeLength= 2000;
            terrainBuffer[i].treeDistance = 2000;
            terrainBuffer[i].basemapDistance = 1000;
            terrainBuffer[i].drawHeightmap = true;*/
            terrainBuffer[i].gameObject.SetActive(false);
            
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResetTouch();
        Vector3 warpPosition = playerGameObject.transform.position;
        TerrainIDFromPosition(ref currentTerrainID, ref warpPosition);

        string dbgString = "";
        dbgString = "CurrentID : " + currentTerrainID[0] + ", " + currentTerrainID[1] + "\n\n";
        for (int i = -spread; i <= spread; i++)
        {
            for (int j = -spread; j <= spread; j++)
            {
                DropTerrainAt(currentTerrainID[0] + i, currentTerrainID[1] + j);
                dbgString += (currentTerrainID[0] + i) + "," + (currentTerrainID[1] + j) + "\n";
            }
        }
        Debug.Log(dbgString);
        ReclaimTiles();
    }

    void TerrainIDFromPosition(ref int[] currentTerrainID, ref Vector3 position)
    {
        currentTerrainID[0] = Mathf.RoundToInt((position.x - referencePosition.x) / referenceSize.x);
        currentTerrainID[1] = Mathf.RoundToInt((position.z - referencePosition.z) / referenceSize.y);
    }

    void DropTerrainAt(int i, int j)
    {
        // Check if terrain exists, if it does, activate it.
        if (terrainUsage.ContainsKey(i, j) && terrainUsage[i, j] != -1)
        {
            // Tile mapped, use it.
        }
        // If terrain doesn't exist, drop it.
        else
        {
            terrainUsage[i, j] = FindNextAvailableTerrainID();
            if (terrainUsage[i, j] == -1) Debug.LogError("No more tiles, failing...");
        }
        if (terrainUsageData.ContainsKey(i, j))
        {
            // Restore the data for this tile
        }
        else
        {
            // Create a new data object
            terrainUsageData[i, j] = CreateNewTerrainData();
        }

        ActivateUsedTile(i, j);
        usedTiles[terrainUsage[i, j]] = true;
        touchedTiles[terrainUsage[i, j]] = true;
    }

    TerrainData CreateNewTerrainData()
    {
        TerrainData tData = new TerrainData();
        CopyTerrainDataFromTo(referenceTerrain.terrainData, ref tData);
        return tData;
    }

    void ResetTouch()
    {
        touchedTiles.SetAll(false);
    }

    int CountOnes(BitArray arr)
    {
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i])
                count++;
        }
        return count;
    }

    void ReclaimTiles()
    {
        if (CountOnes(usedTiles) > ((spread * 2 + 1) * (spread * 2 + 1)))
        {
            for (int i = 0; i < usedTiles.Length; i++)
            {
                if (usedTiles[i] && !touchedTiles[i])
                {
                    usedTiles[i] = false;
                    terrainBuffer[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void ActivateUsedTile(int i, int j)
    {
        terrainBuffer[terrainUsage[i, j]].gameObject.transform.position =
                                    new Vector3(referencePosition.x + i * referenceSize.x,
                                                    referencePosition.y,
                                                    referencePosition.z + j * referenceSize.y);
        terrainBuffer[terrainUsage[i, j]].gameObject.transform.rotation = referenceRotation;
        terrainBuffer[terrainUsage[i, j]].gameObject.SetActive(true);

        terrainBuffer[terrainUsage[i, j]].terrainData = terrainUsageData[i, j];
    }

    int FindNextAvailableTerrainID()
    {
        for (int i = 0; i < usedTiles.Length; i++)
            if (!usedTiles[i]) return i;
        return -1;
    }

    void CopyTerrainDataFromTo(TerrainData tDataFrom, ref TerrainData tDataTo)
    {
        tDataTo.SetDetailResolution(tDataFrom.detailResolution, 8);
        tDataTo.heightmapResolution = tDataFrom.heightmapResolution;
        tDataTo.alphamapResolution = tDataFrom.alphamapResolution;
        tDataTo.baseMapResolution = tDataFrom.baseMapResolution;
        tDataTo.size = tDataFrom.size;
        tDataTo.splatPrototypes = tDataFrom.splatPrototypes;
        tDataTo.treePrototypes = tDataFrom.treePrototypes;
        tDataTo.treeInstances = tDataFrom.treeInstances;

        tDataTo.SetHeights(0,0,tDataFrom.GetHeights(0, 0, tDataFrom.heightmapWidth, tDataFrom.heightmapHeight));
        
        
       // tDataTo
    }
}

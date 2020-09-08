using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorManager : MonoBehaviour
{
    [SerializeField] TileSet[] tileSets;
    //[SerializeField] LevelData levels;

    [SerializeField] bool loadLevel;
    [SerializeField] bool saveLevel;

    public int levelNumber;

    GameObject _levelHolder;
    public GameObject LevelHolder
    {
        get
        {
            if (_levelHolder == null)
                LevelHolder = GameObject.Find("Level");
            
            return _levelHolder;
        }
        set
        {
            if (value == null)
                _levelHolder = new GameObject("Level");
            else
                _levelHolder = value;
        }
    }

    GameObject GetLevelHolder()
    {
        if (LevelHolder.transform.childCount < 1)
            return null;
        else
            return LevelHolder.transform.GetChild(0).gameObject;
    }

    void Start ()
    {
        if (saveLevel)
        {
            SaveLevel(levelNumber);
        }

        if (loadLevel)
        {
            LoadLevel(levelNumber);
        }
        
	}
	
	void LoadLevel(int levelNumber)
    {
        StartCoroutine(LoadLevelAsync(levelNumber));
    }

    IEnumerator LoadLevelAsync(int levelNumber)
    {
        yield return LevelCreatorAPIManager.Instance.GetAllLevels();
        var holder = GetLevelHolder();
        if (holder != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(holder);
            else
#endif
                Destroy(holder);
        }

        holder = new GameObject("Holder");
        holder.transform.position = new Vector3(.5f, 0, 0);
        holder.transform.parent = LevelHolder.transform;

        var level = LevelCreatorAPIManager.Instance.level;
        foreach (var block in level.BlockDatas)
        {
            var tile = tileSets[block.AssetId];
            var go = Instantiate(tile.prefabs[block.Id]);
            go.transform.position = block.Position;
            go.transform.parent = holder.transform;
        }
    }

    void SaveLevel(int levelNumber)
    {
        var holder = LevelHolder.transform.GetChild(0).gameObject;
        if (holder == null)
            return;

        var levelBlocks = new BlockData[holder.transform.childCount];
        for (int i = 0; i < levelBlocks.Length; i++)
        {
            var tItem = holder.transform.GetChild(i);
            var tile = tileSets[(int)tItem.position.y];

            for (int j = 0; j < tile.prefabs.Length; j++)
            {
                if (tItem.name == tile.prefabs[j].name)
                {
                    //Debug.Log(tItem.name + " saved successfully");
                    levelBlocks[i] = new BlockData(tItem.position, j);
                    //level.AddBlock(new BlockData(tItem.position, j));
                    break;
                }
            }
        }

        var jlevel = JsonUtility.ToJson(new LevelData(levelBlocks), false);
        LevelCreatorAPIManager.Instance.SaveLevel(jlevel);
    }
}
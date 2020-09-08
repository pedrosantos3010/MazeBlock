using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] BlockData[] _blockDatas;

    public LevelData (BlockData[] blockData)
    {
        _blockDatas = blockData;
    }
    /*
    public void AddBlock(BlockData item)
    {
        if (_blockDatas == null)
            _blockDatas = new List<BlockData>();

        _blockDatas.Add(item);
    }
    */
    public BlockData[] BlockDatas { get { return _blockDatas; } }
}
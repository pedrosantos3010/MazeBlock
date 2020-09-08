using System;
using UnityEngine;

[Serializable]
public class BlockData
{
    [SerializeField] Vector3 _position;
    [SerializeField] int _id;

    public int AssetId { get { return (int)_position.y; } }
    public int Id { get { return _id; } }
    public Vector3 Position { get { return _position; } }

    public BlockData(Vector3 position, int id)
    {
        _position = position;
        _id = id;
    }
}
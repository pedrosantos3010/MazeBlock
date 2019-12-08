using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameBlock
{
    public int Id;
    public bool IsGround; 
    public Vector3 Position;
    public float RotationY;
    
    public GameBlock(int _id, Vector3 _position, float _rotationY = 0)
    {
        Id = _id;
        IsGround = (_position.y == 0);
        Position = _position;
        RotationY = _rotationY;
    }
}

[System.Serializable]
public class GameLevelInformation
{
    public int id;
    public GameBlock[] levelBlocks;
}

public class GameLevelFromJson : MonoBehaviour {
    [SerializeField] GameLevelInformation _level;

    string ResourcesFolder { get { return Application.dataPath + "/Resources/" + "LevelData.json"; } }

    private void Update()
    {
        if (Input.GetKeyDown("s")) SaveLevel();

        if (Input.GetKeyDown("l")) LoadLevel();
    }

    public void SaveLevel ()
    {
        string levelJson = JsonUtility.ToJson(_level);
        File.WriteAllText(ResourcesFolder, levelJson);
    }

    public void LoadLevel ()
    {
        string levelData = File.ReadAllText(ResourcesFolder);
        _level = JsonUtility.FromJson<GameLevelInformation>(levelData);


    }
}

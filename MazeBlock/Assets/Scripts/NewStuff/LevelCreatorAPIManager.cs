using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelCreatorAPIManager : MonoBehaviour
{
    [Serializable]
    public class Record { public string id; }
    [Serializable]
    public class RecordData
    {
        public List<Record> records;
        public bool success;
    }

    const string API_URL = "https://api.jsonbin.io/";
    const string API_KEY = "$2b$10$d95xwDl2.dV7QbLMArsBNuvLwb0AUc4V91XEluUiwOwSh10xkIQI2";
    const string BUCKET = "5f562e9f514ec5112d1809ef"; //Collection

    public LevelData level;

    static LevelCreatorAPIManager _instance;
    public static LevelCreatorAPIManager Instance
    {
        get
        {
            if (_instance == null)
                Instance = GameObject.FindObjectOfType<LevelCreatorAPIManager>();

            return _instance;
        }
        set
        {
            if (value == null)
                _instance = new GameObject("Level Creator API Manager").AddComponent<LevelCreatorAPIManager>();
            else
                _instance = value;
        }
    }

    public Coroutine SaveLevel(string levelJson, int levelIndex = 0)
    {
        return StartCoroutine(OnPOSTRequest(levelIndex, levelJson));
    }

    public Coroutine GetAllLevels(int levelIndex = 0)
    {
        return StartCoroutine(OnGETRequest(levelIndex));
    }
 
    IEnumerator OnGETRequest(int levelIndex)
    {
        print("Loading levels...");
        var request = UnityWebRequest.Get(API_URL + "/e/collection/" + BUCKET + "/all-bins");
        request.SetRequestHeader("secret-key", API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }

        print(request.downloadHandler.text);

        var levelRecords = JsonUtility.FromJson<RecordData>(request.downloadHandler.text);
        print(levelRecords.records.Count);
        print(levelRecords.records[levelIndex].id);

        request = UnityWebRequest.Get(API_URL + "/b/" + levelRecords.records[levelIndex].id);
        request.SetRequestHeader("secret-key", API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }   

        print(request.downloadHandler.text);
        level = JsonUtility.FromJson<LevelData>(request.downloadHandler.text);
    }

    IEnumerator OnPOSTRequest(int levelIndex, string levelJson)
    {
        var test = new Record();
        test.id = "1";
        var jrecord = JsonUtility.ToJson(test);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection(jrecord));

        print(jrecord);
        print(levelJson);
        var request = UnityWebRequest.Post(API_URL + "/b", formData);
        request.SetRequestHeader("secret-key", API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("collection-id", BUCKET);
        request.SetRequestHeader("private", "true");
        request.SetRequestHeader("name", "Level" + levelIndex);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.responseCode);
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
        }

        Debug.Log("Post done!");
    }
}




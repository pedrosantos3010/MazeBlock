using System.Collections;
using UnityEngine;

public static class MyData
{
    public static float rayDistance = 1.0f;
}

public struct TimeToPercent
{
    float startTime, totalLerpTime;

    public TimeToPercent (float _startTime)
    {
        startTime = _startTime;
        totalLerpTime = TimeManager.LerpSpeed;
    }

    public float GetPercentage ()
    {
        float timeSinceStarted = Time.time - startTime; 
        return timeSinceStarted / totalLerpTime;
    }
}

public static class MyFunctions
{

    public static IEnumerator WaitTimeNotMove ()
    {
        yield return null;
        yield return new WaitUntil (() => !TimeManager.timeIsMoving);
        yield return null;
    }
}

public static class MyTags
{
    public const string startLevel = "StartLevel";
    public const string endLevel = "EndLevel";
    public const string player = "Player";

    public const string slipperyGround = "SlipperyBox";
    public const string slipperyBox = "SlipperyBox";
    public const string boxPusher = "BoxPusher";
    public const string moveWall = "MoveWall";

    public const string gameController = "GameController";
    public const string musicController = "MusicController";

}


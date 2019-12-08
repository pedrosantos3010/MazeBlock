using System;
using System.Collections.Generic;

public static class TimeManager
{

    public const float LerpSpeed = 0.1f;
    public static int objectsInAction;
    public static bool timeIsMoving;

    public static event System.Action OnMove;

    public static void StartTime ()
    {
        objectsInAction++;
        if (objectsInAction == 1) {
            if (OnMove != null)
                OnMove ();
            timeIsMoving = true;
		}
    }

    public static void EndTime ()
    {
        if (objectsInAction == 0)
            return;

        objectsInAction--;
        if (objectsInAction == 0) {
            timeIsMoving = false;
		}
    }

    public static void EndAllActions ()
    {
        objectsInAction = 0;	
        timeIsMoving = false;
    }
}
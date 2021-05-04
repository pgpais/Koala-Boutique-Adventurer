using System;

[Serializable]
public class Mission
{
    public static string firebaseReferenceName = "missions";

    public int seed;
    public bool successfulRun;

    public Mission()
    {
        seed = new Random().Next();
        successfulRun = false;
    }

    public Mission(int seed, bool successfulRun)
    {
        this.seed = seed;
        this.successfulRun = successfulRun;
    }

    public string GetFirebaseReferenceName() => firebaseReferenceName;
}
using System;
using System.Collections.Generic;

[Serializable]
public class Mission
{
    public static string firebaseReferenceName = "missions";

    public int seed;
    public bool completed;
    public List<string> boughtBuffs;

    public Mission()
    {
        seed = new Random().Next();
        completed = false;
        this.boughtBuffs = new List<string>();
    }

    public Mission(int seed, bool completed)
    {
        this.seed = seed;
        this.completed = completed;
        this.boughtBuffs = new List<string>();
    }

    public Mission(int seed, bool completed, List<string> boughtBuffs) : this(seed, completed)
    {
        this.boughtBuffs = boughtBuffs;
    }

    public string GetFirebaseReferenceName() => firebaseReferenceName;
}
using UnityEngine;

public static class Records
{
    public static Zombie currReqs;
    public static Zombie givenCorpse;
    public static int requestNum = 0;
    public static int reputation = 100;
    // ^ Maybe if we have time but I wanna implement a thing where like if you have low / high reputation, more zombies spawn
    // deliver a bad corpse, reputation go down
    // deliver a good corpse, reputation go up
    public static bool freeze = false;
    public static Vector3 playerLastPos;
}

using UnityEngine;
using static BodyParts;

public class Zombie : MonoBehaviour
{
    // Goal of zombie: Get to player -> attack player
    // Leg and its mutation affect getting to player part
    // Average is just normal walk
    // Long walks faster
    // Mutated walks in weird patterns
    // Robot makes a beeline for player (wip)
    // Arm and its mutation affect attack player part 
    // Average is just normal attack
    // Long attacks from further away
    // Mutated attacks in weird patterns
    // Robot idk kills the player or smthng lmao

    public BodyParts bp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(params BodyParts[] parts)
    {
        if (parts.Length != 2)
        {
            // BodyParts not passed in from ZombieSpawner, initialize random
            Array vals = Enum.GetValues(typeof(PartType)); // ugly af code uh it just grabs a random enum from PartType
            PartType bp1 = (PartType)vals.GetValue(UnityEngine.Random.Range(0, vals.Length));
            // did you know that GetValue is called from the array you get from Enum.GetValues instead of the enum itself i didn't know that oopsies
            bp1 == PartType.Torso ? bp = BodyParts(bp1, (TorsoVariation)TorsoVariation.GetValue(UnityEngine.Random.Range(0, TorsoVariation.Length))) : bp = BodyParts(bp1, (Variation)Variation.GetValue(UnityEngine.Random.Range(0, Variation.Length)));
        }
        else
        {
            if (parts[0] != typeof(PartType))
            {
                throw new System.Exception("Incorrect order of arguments / Wrong arguments");
            }
            else
            {
                bp = BodyParts(parts[0], parts[1]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

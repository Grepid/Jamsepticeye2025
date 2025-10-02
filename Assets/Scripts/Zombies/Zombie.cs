using UnityEngine;
using static BodyParts;
using System;

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
    void Start()
    {
        
    }

    public Zombie(PartType? parts, Variation? vari, TorsoVariation? tvari)
    {
        if (parts == null || (vari == null && tvari == null))
        {
            // BodyParts not passed in from ZombieSpawner, initialize random
            Array vals = Enum.GetValues(typeof(PartType)); // ugly af code uh it just grabs a random enum from PartType
            Array valsNT = Enum.GetValues(typeof(Variation));
            Array valsT = Enum.GetValues(typeof(TorsoVariation));
            PartType bp1 = (PartType)vals.GetValue(UnityEngine.Random.Range(0, vals.Length));
            // did you know that GetValue is called from the array you get from Enum.GetValues instead of the enum itself i didn't know that oopsies
            bp = (bp1 == PartType.Torso) ? new BodyParts(bp1, tvari: (TorsoVariation)valsT.GetValue(UnityEngine.Random.Range(0, valsT.Length))) : new BodyParts(bp1, vari: (Variation)valsNT.GetValue(UnityEngine.Random.Range(0, valsNT.Length)));
        }
        else
        {
            if (!parts.HasValue)
            {
                throw new System.Exception("Incorrect order of arguments / Wrong arguments");
            }
            else
            {
                if (parts == PartType.Torso)
                {
                    bp = new BodyParts(parts.Value, tvari: tvari);
                }
                else
                {
                    bp = new BodyParts(parts.Value, vari: vari);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System;
using System.Collections;

public class BodyParts
{
    public enum PartType
    {
        Legs,
        Arms,
        Torso
    }

    public enum Variation
    {
        Average,
        Missing,
        Long,
        Mutated,
        Robot,
        Pirate,
        SPECIAL_JACKSEPTICEYE_ARM_SPAGHETTI_CODE_THIS_IN
    }

    public enum TorsoVariation
    {
        Average,
        Missing,
        Suit,
        Construction_Uniform,
        Bulletproof_Vest,
        Jacksepticeye_Branded_Tshirt
    }

    public PartType pt;
    public Variation? v;
    public TorsoVariation? tv;

    // Private constructor for creating the overall zombie
    public BodyParts(PartType type, Variation? vari = null, TorsoVariation? tvari = null)
    {
        pt = type;
        v = vari;
        tv = tvari;

        // Sanitation check for Torso
        if (type == PartType.Torso)
        {
            if (vari != null)
            {
                throw new System.ArgumentException("Torso - Cannot have Variation argument.");
            }
            if (tvari == null)
            {
                throw new System.ArgumentException("Torso - Torso Variation can't be null.");
            }
        }
        else
        {
            if (tvari != null)
            {
                throw new System.ArgumentException("Cannot have Torso Variation in non-Torso Part.");
            }
        }
    }
}

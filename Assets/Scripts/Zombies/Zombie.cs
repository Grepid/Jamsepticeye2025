using UnityEngine;
using static BodyParts;
using System;
using UnityEngine.AI;
using System.Collections;

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

    private BodyParts bp;
    private BodyParts[] wholeBody = new BodyParts[5]; // a whole body has 2 arms, 2 legs, and 1 torso. We also want it so that there are between 0-2 unique body parts per zombie
    private bool firstPass = true;
    private float damageThrust = 500f;
    private int hp = 5;
    private int legCounter = 2;
    private int armCounter = 2;
    private bool torsoExists = false;
    private int currLimbs = 0;
    public GameObject droppablePrefab;
    public NavMeshAgent agent;
    public float reCalculationFrequency;
    private float recalculationTimer;
    public float ZombieSpeed = 3;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.speed = ZombieSpeed;
        agent.SetDestination(PlayerController.instance.transform.position);
        OnPlayerHit += () =>
        {
            StartCoroutine(ZombieLock());
        };

        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator ZombieLock()
    {
        isLocked = true;
        agent.SetDestination(transform.position);
        PlayerController.instance.isControlling = false;
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            agent.speed = ZombieSpeed;
        }
        yield return new WaitForSeconds(1);
        isLocked = false;
        PlayerController.instance.isControlling = true;
        recalculationTimer = reCalculationFrequency;
    }

    public void Initialize(PartType? parts, Variation? vari, TorsoVariation? tvari)
    {
        // LLLLLLLLLLLLLLLLETS GO GAMBLINGGGGGGG
        // Determine how many unique body parts this one has
        int uniqueBodyParts = UnityEngine.Random.Range(0, 3);
        int forcedSpecial = 0;
        forcedSpecial = (parts != null) ? 1 : 0;
        if (uniqueBodyParts + forcedSpecial >= 3)
        {
            forcedSpecial = 0;
        }
        Debug.Log("Num body parts: " + (uniqueBodyParts + forcedSpecial));

        for (int i = 0; i < uniqueBodyParts + forcedSpecial; ++i)
        {
            if (!firstPass || parts == null || (vari == null && tvari == null))
            {
                // BodyParts not passed in from ZombieSpawner, initialize random
                // Always want to go here past the first pass because the first pass is only if zombiespawner passes in a certain body part (for quest)
                Array vals = Enum.GetValues(typeof(PartType)); // ugly af code uh it just grabs a random enum from PartType
                Array valsNT = Enum.GetValues(typeof(Variation));
                Array valsT = Enum.GetValues(typeof(TorsoVariation));
                PartType bp1 = (PartType)vals.GetValue(UnityEngine.Random.Range(0, vals.Length));
                switch (bp1)
                {
                    case PartType.Torso:
                        if (torsoExists)
                        {
                            i++;
                            continue;
                        }
                        break;
                    case PartType.Arms:
                        if (armCounter == 0)
                        {
                            i++;
                            continue;
                        }
                        break;
                    case PartType.Legs:
                        if (legCounter == 0)
                        {
                            i++;
                            continue;
                        }
                        break;
                }
                // ^ spaghetti code lmao
                // did you know that GetValue is called from the array you get from Enum.GetValues instead of the enum itself i didn't know that oopsies
                // rng 1/10 chance that if it's arm, to do the special arm
                if (bp1 == PartType.Arms && UnityEngine.Random.Range(0, 10) == 0)
                {
                    bp = new BodyParts(bp1, vari: Variation.SPECIAL_JACKSEPTICEYE_ARM_SPAGHETTI_CODE_THIS_IN);
                    Debug.Log("Special arm spawned");
                }
                else
                {
                    bp = (bp1 == PartType.Torso) ? new BodyParts(bp1, tvari: (TorsoVariation)valsT.GetValue(UnityEngine.Random.Range(1, valsT.Length))) : new BodyParts(bp1, vari: (Variation)valsNT.GetValue(UnityEngine.Random.Range(1, valsNT.Length - 1)));
                    // ^ start at 1 to exclude average
                }
                firstPass = false;
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
                firstPass = false;
            }

            switch (bp.pt)
            {
                case PartType.Torso:
                    torsoExists = true;
                    break;
                case PartType.Arms:
                    armCounter--;
                    break;
                case PartType.Legs:
                    legCounter--;
                    break;
            }
            // Debug.Log("TEST");
            Debug.Log("TEST: " + bp.pt + " " + bp.v + " " + bp.tv);
            currLimbs++;
            wholeBody[i] = bp;
        }

        BuildOutRestOfBody();
        // if we ever need to modify it so that a zombie has multiple body parts we can just throw a for loop or smthng around this whole thing
    }

    public void BuildOutRestOfBody()
    {
        // Each zombie has 2 legs, 2 arms, and 1 torso
        // Called after Initialize, anything that's not special will just be the normal
        for (int i = 0; i < legCounter; ++i)
        {
            wholeBody[currLimbs] = new BodyParts(PartType.Legs, vari: Variation.Average);
            currLimbs++;
        }
        for (int i = 0; i < armCounter; ++i)
        {
            wholeBody[currLimbs] = new BodyParts(PartType.Arms, vari: Variation.Average);
            currLimbs++;
        }
        if (!torsoExists)
        {
            wholeBody[currLimbs] = new BodyParts(PartType.Torso, tvari: TorsoVariation.Average);
            currLimbs++;
        }
        Debug.Log("This zombie has: ");
        foreach (BodyParts part in wholeBody)
        {
            Debug.Log(part.pt + " " + part.v + " " + part.tv);
        }
    }

    public void DamageSelf(Vector3 pointOfImpact)
    {
        Debug.Log("hit");
        // Fixing the uh bug where the zombies fly 5000000 blocks away on hit lemme cook
        // _rb.isKinematic = false;
        // _rb.useGravity = false;
        // _agent.enabled = false;
        // Ok saw a youtube tutorial imma follow that

        // Drop body part at current location (if not average)
        // Pick random body part from wholeBody
        BodyParts randomPart;
        int randomPartToPick = UnityEngine.Random.Range(0, wholeBody.Length);
        randomPart = wholeBody[randomPartToPick];
        // remove element (only reason I'm not using List is because 50000 errors showed up when I tried and I don't have enough brain power to deal with that rn)
        for (int x = randomPartToPick; x < wholeBody.Length - 1; ++x)
        {
            wholeBody[x] = wholeBody[x + 1];
        }
        Array.Resize(ref wholeBody, wholeBody.Length - 1);
        Debug.Log(randomPart.pt);
        DroppedBodyPart drop = null;
        if (!(randomPart.v == Variation.Average || randomPart.tv == TorsoVariation.Average || randomPart.v == Variation.Missing))
        {
            drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), this.transform.position, Quaternion.identity, this.transform.parent);
            drop.Initialise(randomPart);
            drop.GetComponent<Collider>().enabled = false;
        }

        // this.GetComponent<Rigidbody>().AddForce((transform.position - pointOfImpact) * damageThrust);
        StopAllCoroutines();
        StartCoroutine(ApplyKnockback(pointOfImpact));

        if (drop != null)
        {
            drop.GetComponent<Collider>().enabled = true;
        }

        this.hp -= 1;
        if (this.hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public BodyParts[] ShowBodyParts()
    {
        return wholeBody;
    }

    // Update is called once per frame
    void Update()
    {
        TrySetAgentPath();
        TryAttack();
        // walk and run should probably be in another class inherited from an abstract ZombieWalk and ZombieAttack class methinks
        // and after that, this class could attach those specific classes:
        // i.e. if bp.pt = Arm and bp.v = Long, attach ZombieAttackLong to component
        // I forget do we want multiple body parts
        // ...or we just do like 50 million switch cases which looks to be somewhat better? mostly cuz abstract classes and interfaces don't really help much more than switch cases in this case
        recalculationTimer += Time.deltaTime;
    }
    private void TrySetAgentPath()
    {
        if (isLocked) return;
        if (recalculationTimer >= reCalculationFrequency)
        {
            agent.SetDestination(PlayerController.instance.transform.position);
            recalculationTimer = 0;
        }
    }
    public float AttackRange;
    Coroutine attackRoutine;
    private bool isLocked;
    private static event Action OnPlayerHit;
    public float slowdownWhenAttacking;
    private void TryAttack()
    {
        if (isLocked) return;
        if (attackRoutine != null) return;
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= AttackRange)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        agent.speed = ZombieSpeed * slowdownWhenAttacking;
        //Maybe if a zombie successfully attacks, it freezes all zombies aswell as Stunning the Player or whatever has been proposed (Better than all the zombies just flooding the player)

        //Just gonna do a quick 1 second delay before hitting, and attack just be distance based, we can expand further when specifications get made
        yield return new WaitForSeconds(1);

        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= AttackRange)
        {
            print("Player Hit By Zombie");
            OnPlayerHit?.Invoke();
        }
        else
        {
            print("Zombie missed Player");
        }
        agent.speed = ZombieSpeed;
        attackRoutine = null;
    }

    void OnCollisionEnter(Collision other)
    {
        LayerMask enemyLayer = LayerMask.NameToLayer("Enemy");
        if (other.gameObject.layer == enemyLayer)
        {
            StopAllCoroutines();
            StartCoroutine(ApplyKnockback(transform.position));
        }
    }

    private IEnumerator ApplyKnockback(Vector3 pointOfImpact)
    {
        yield return null;


        isLocked = true;
        _rb.useGravity = true;
        _agent.enabled = false;
        _rb.AddForce((transform.position - pointOfImpact) * damageThrust);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => _rb.linearVelocity.magnitude < 0.05f);

        _rb.useGravity = false;
        _agent.Warp(transform.position);
        _agent.enabled = true;

        yield return null;
        isLocked = false;
    }
}

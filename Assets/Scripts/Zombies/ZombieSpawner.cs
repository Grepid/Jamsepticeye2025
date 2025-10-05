using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject[] spawnLocations;
    public GameObject ground;
    public LayerMask enemyLayer;
    public int ZombiesToSpawn;
    private float spawnTimer = 1f;
    private float currTimer = 0f;
    // Here's how it's gonna work
    // Zombies spawn whenever Records.freeze = false
    // 5 zombies per level; increasing with level (level 1 = 5, level 2 = 10)
    // these all spawn on a timer with a random interval between each spawn
    // these all spawn in 1 of the spawnLocations
    void Start()
    {
        ZombiesToSpawn = (Records.requestNum + 1) * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Records.freeze) {
            if (currTimer >= spawnTimer)
            {
                SpawnZom();
                currTimer = 0;
                spawnTimer = Random.Range(0f, 3f);
            }
            currTimer += Time.deltaTime;
        }
    }

    void SpawnZom()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        if (spawnLocations.Length > 0)
        {
            // Vector3 x;
            // Vector3 z;
            // Collider collider = ground.GetComponent<Collider>();
            // Vector3 maxBound = collider.bounds.max;
            // Vector3 minBound = collider.bounds.min;
            // Vector3 center = collider.bounds.center;
            // Vector3 size = collider.bounds.size;
            // Vector3 extents = collider.bounds.extents;
            if (Records.spawnedZoms >= ZombiesToSpawn)
            {
                // gambling time
                // 1/6 chance to spawn another zombie every time this runs
                // 1/2 chance to have the part that is on the requirement zombie
                int randomChance = Random.Range(0, 6);
                if (randomChance == 0)
                {
                    int randomChance2 = Random.Range(0, 2);
                    if (randomChance2 == 0)
                    {
                        // spawn one with needed part
                        int randomIndex = Random.Range(0, spawnLocations.Length);
                        Zombie zom = Instantiate(zombiePrefab.GetComponent<Zombie>(), spawnLocations[randomIndex].transform.position, Quaternion.identity, this.transform);
                        zom.gameObject.layer = enemyLayer;
                        BodyParts[] allParts = Records.currReqs.ShowBodyParts();
                        int randomIndex2 = Random.Range(0, allParts.Length);
                        if (Records.currReqs.ShowBodyParts()[randomIndex2].pt == BodyParts.PartType.Torso)
                        {
                            zom.Initialize(Records.currReqs.ShowBodyParts()[randomIndex2].pt, null, Records.currReqs.ShowBodyParts()[randomIndex2].tv);
                        }
                        else
                        {
                            zom.Initialize(Records.currReqs.ShowBodyParts()[randomIndex2].pt, Records.currReqs.ShowBodyParts()[randomIndex2].v, null);
                        }
                    }
                    else
                    {
                        // spawn random one
                        int randomIndex = Random.Range(0, spawnLocations.Length);
                        Zombie zom = Instantiate(zombiePrefab, spawnLocations[randomIndex].transform.position, Quaternion.identity, this.transform).GetComponent<Zombie>();
                        zom.gameObject.layer = enemyLayer;
                        zom.Initialize(null, null, null);
                    }
                }
            }
            else
            {
                // for (int i = 0; i < ZombiesToSpawn; ++i)
                // {
                // x = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.x * ground.transform.right;
                // z = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.z * ground.transform.forward;
                // // ^ I hate this part I have no clue why it works something to do with localScale tho
                // if (Vector3.Distance(center + x + z, PlayerController.instance.transform.position) <= 5)
                // {
                //     // reroll lmao
                //     --i;
                //     continue;
                // }
                // Zombie zom = Instantiate(zombiePrefab.GetComponent<Zombie>(), center + x + z + new Vector3(0, size.y+extents.y, 0), Quaternion.identity, this.transform);

                // Pick random spawn location
                int randomIndex = Random.Range(0, spawnLocations.Length);
                Zombie zom = Instantiate(zombiePrefab, spawnLocations[randomIndex].transform.position, Quaternion.identity, this.transform).GetComponent<Zombie>();
                zom.gameObject.layer = enemyLayer;
                zom.Initialize(null, null, null);
                Records.spawnedZoms += 1;
            }
            // }
        }
        else
        {
            Vector3 x;
            Vector3 z;
            Collider collider = ground.GetComponent<Collider>();
            Vector3 maxBound = collider.bounds.max;
            Vector3 minBound = collider.bounds.min;
            Vector3 center = collider.bounds.center;
            Vector3 size = collider.bounds.size;
            Vector3 extents = collider.bounds.extents;

            x = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.x * ground.transform.right;
            z = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.z * ground.transform.forward;
            // ^ I hate this part I have no clue why it works something to do with localScale tho
            Zombie zom = Instantiate(zombiePrefab, center + x + z + new Vector3(0, size.y + extents.y, 0), Quaternion.identity, this.transform).GetComponent<Zombie>();
            zom.gameObject.layer = enemyLayer;
            zom.Initialize(null, null, null);

        }
    }
}

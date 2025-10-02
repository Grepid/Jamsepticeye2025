using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 x;
        Vector3 z;
        Collider collider = ground.GetComponent<Collider>();
        Vector3 maxBound = collider.bounds.max;
        Vector3 minBound = collider.bounds.min;
        Vector3 center = collider.bounds.center;
        Vector3 size = collider.bounds.size;
        Vector3 extents = collider.bounds.extents;
        for (int i = 0; i < 10; ++i)
        {
            x = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.x * ground.transform.right;
            z = Random.Range(-0.5f, 0.5f) * ground.transform.localScale.z * ground.transform.forward;
            if (Vector3.Distance(center + x + z, PlayerController.instance.transform.position) <= 5)
            {
                // reroll lmao
                --i;
                continue;
            }
            Zombie zom = Instantiate(zombiePrefab.GetComponent<Zombie>(), center + x + z + new Vector3(0, size.y+extents.y, 0), Quaternion.identity, this.transform);
            zom.Initialize(null, null, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

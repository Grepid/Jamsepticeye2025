using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float x;
        float y;
        float z;
        Collider collider = ground.GetComponent<Collider>();
        Vector3 maxBound = collider.bounds.max;
        Vector3 minBound = collider.bounds.min;
        Vector3 center = collider.bounds.center;
        Vector3 size = collider.bounds.size;
        Vector3 extents = collider.bounds.extents;
        for (int i = 0; i < 10; ++i)
        {
            x = Random.Range((center-extents).x, (center+extents).x);
            y = size.y;
            z = Random.Range((center-extents).z, (center+extents).z);
            Zombie zom = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(x, y, z), Quaternion.identity, this.transform);
            zom.Initialize(null, null, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

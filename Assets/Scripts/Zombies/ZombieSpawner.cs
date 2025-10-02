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
        x = Random.Range(minBound.x, maxBound.x);
        y = maxBound.y;
        z = Random.Range(minBound.z, maxBound.z);
        Zombie zom = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(x, y, z), Quaternion.identity, this.transform);
        zom.Initialize(null, null, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

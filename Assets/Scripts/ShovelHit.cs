using UnityEngine;

public class ShovelHit : MonoBehaviour
{
    public LayerMask enemyLayer;
    private float timeDelay = 0.5f;
    void Update()
    {
        timeDelay -= Time.time;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (timeDelay < 0f) {
            enemyLayer = LayerMask.NameToLayer("Enemy");
            if (other.gameObject.layer == enemyLayer)
            {
                other.GetComponent<Zombie>().DamageSelf(transform.position);
            }
            timeDelay = 0.5f;
        }
    }
}

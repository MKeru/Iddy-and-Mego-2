using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DangerZone : MonoBehaviour
{
    int decay = 100;
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            FindObjectOfType<HealthBar>().LoseHealth(decay);
        }
    }
}

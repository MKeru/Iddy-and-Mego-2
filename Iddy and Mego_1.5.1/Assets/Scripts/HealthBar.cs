using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;
    public float health;
    // 100 health = 1 fill

    public void LoseHealth(int val)
    {
        // do nothing if no health left
        if (health <= 0)
            return;
        // Reduces health
        health -= val;

        // Refresh UI fill bar
        fill.fillAmount = health / 100;

        //checks death condition
        if (health <= 0)
        {
            FindObjectOfType<Cat>().Die();

        }
    }

    private void Update()
    {

    }
}

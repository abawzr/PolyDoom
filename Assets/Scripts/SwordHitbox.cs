using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float attackDamage;

    public bool CanDamage { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanDamage)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}

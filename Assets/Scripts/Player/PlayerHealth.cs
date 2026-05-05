using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthAmount = 100;
    [SerializeField] private int maxHealth = 100;

    public bool isAlive = true;

    private Combat m_Combat;
    private Movment m_PlayerMovment;
    private Animator m_Animator;

    void Start()
    {
        m_Combat = GetComponent<Combat>();
        m_Animator = GetComponentInChildren<Animator>();
        m_PlayerMovment = GetComponent<Movment>();
    }

    void Update()
    {
        if (healthAmount <= 0 && isAlive)
        {
            Death();
        }
    }

    private void Death()
    {
        isAlive = false;

        m_PlayerMovment.canMove = false;
        m_Animator.SetBool("Death", true);
        m_Combat.turnOffLayers();
    }

    public void TakeDamage(int amount)
    {
        healthAmount -= amount;
    }
}

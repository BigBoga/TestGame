using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] GameObject VFXPosition;

    [SerializeField] private bool canAttack = true;

    InputAction attackInput;

    private Animator m_Animator;
    private Combat m_Combat;
    private PlayerHealth m_PlayerHealth;

    private void init()
    {
        m_PlayerHealth = GetComponentInParent<PlayerHealth>();

        if (!m_PlayerHealth.isAlive) { return; }

        m_Animator = GetComponentInParent<Animator>();
        m_Combat = GetComponentInParent<Combat>();

        attackInput = InputSystem.actions.FindAction("Attack");

        StartCoroutine(m_Combat.ChangeLayerWeight(1, 0.3f, weaponData.animtionIdLayer));

        if (!canAttack)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnEnable()
    {
        init();
    }

    void Start()
    {
        init();
    }

    void Update()
    {
        if (!m_PlayerHealth.isAlive) { return; }

        if (attackInput.IsPressed() && canAttack)
        {
            Attack();
            StartCoroutine(AttackCooldown());
        }
    }

    void Attack()
    {
        m_Animator.SetTrigger("Attack");

        StartCoroutine(slashEffect());
    }

    private IEnumerator slashEffect()
    {
        yield return new WaitForSeconds(weaponData.timeBeforeSlash);

        GameObject slashEffectClone = Instantiate(weaponData.slashEffect, VFXPosition.transform.position, VFXPosition.transform.rotation);
        slashEffectClone.transform.parent = VFXPosition.transform;

        slashEffectClone.GetComponent<ParticleSystem>().Play();

        Destroy(slashEffectClone, 1);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(weaponData.weaponCooldown);
        canAttack = true;
    }

  
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject[] Weapons;

    [SerializeField] GameObject weaponsParent;

    InputAction weaponSelect;

    private Animator m_Animator;
    private PlayerHealth m_PlayerHealth;

    private void InitGuns()
    {
        Weapons = GameObject.FindGameObjectsWithTag("Weapon");

        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
            Weapons[i].GetComponent<Weapon>().enabled = true;
        }
    }

    void Start()
    {
        weaponSelect = InputSystem.actions.FindAction("WeaponSelect");

        m_Animator = GetComponentInChildren<Animator>();
        m_PlayerHealth = GetComponent<PlayerHealth>();

        InitGuns();
    }

    private void Update()
    {
        if (!m_PlayerHealth.isAlive) { return; }

        for (int i = 0; i < Weapons.Length + 1; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + (i)))
            {
                Debug.Log($"Нажата клавиша {i}");
                SelectGun(i);
            }
        }
    }

    private void takeOffWeapons()
    {
        turnOffLayers();

        for (int i = 0; i < Weapons.Length;i++)
        {
            Weapons[i].SetActive(false);
        }
    }

    private void SelectGun(int gunIndex)
    {
        takeOffWeapons();
        if (gunIndex == 0)
        {
            return;
        }

        StopAllCoroutines();
        Weapons[gunIndex - 1].SetActive(true);
    }

    public void turnOffLayers()
    {
        for (int i = 1; i < m_Animator.layerCount; i++)
        {
            m_Animator.SetLayerWeight(i, 0);
        }
    }

    public IEnumerator ChangeLayerWeight(float target, float duration, int Index)
    {
        float startWeight = m_Animator.GetLayerWeight(Index);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newWeight = Mathf.Lerp(startWeight, target, t);
            m_Animator.SetLayerWeight(Index, newWeight);
            yield return null;
        }

        m_Animator.SetLayerWeight(Index, target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireballScript : MonoBehaviour
{
    [SerializeField] PlayerDataSO data;
    [SerializeField] Transform boss;
    [SerializeField] GameObject FIREBALL;

    GameObject g;

    // THIS IS VITAL
    private void OnEnable()
    {
        data.FireballActionDelegate += SpawnFireball;
    }
    private void OnDisable()
    {
        data.FireballActionDelegate -= SpawnFireball;
    }
    

    public void SpawnFireball(RaycastHit r)
    {
        if (g)
        {
            Destroy(g);
        }

        g = Instantiate(FIREBALL, transform.position + data.action_forward * 1.5f, Quaternion.identity);
        g.transform.forward = data.action_forward;

        g.GetComponent<FireballBehaviorScript>().boss = boss.position;
    }
}

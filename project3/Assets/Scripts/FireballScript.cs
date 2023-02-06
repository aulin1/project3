using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireballScript : MonoBehaviour
{
    [SerializeField] GameObject FIREBALL;
    [SerializeField] Transform pov;

    GameObject g;

    public void SpawnFireball(RaycastHit r)
    {
        if (g)
        {
            Destroy(g);
        }

        Debug.Log(pov.position);
        g = Instantiate(FIREBALL, pov.position, pov.rotation);
    }
}

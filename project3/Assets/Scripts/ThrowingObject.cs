using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject thrownObject;
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;
    bool throwPossible;

    // Start is called before the first frame update
    void Start()
    {
        throwPossible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(throwKey) && throwPossible) {
            Throw();
        }
    }

    private void Throw() {
        throwPossible = false;

        GameObject projectile = Instantiate(thrownObject, attackPoint.position, cam.rotation);

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 projectileForce = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRB.AddForce(projectileForce, ForceMode.Impulse);
    }
}

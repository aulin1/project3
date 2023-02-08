using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviorScript : MonoBehaviour
{
    [SerializeField] ParticleSystem fire, explosion;
    public Vector3 boss;
    Vector3 origin;

    IEnumerator process;
    Quaternion rot;

    private void Start()
    {
        process = IEProcess();

        origin = transform.position;

        StartCoroutine(process);
    }

    IEnumerator IEProcess()
    {
        while ((origin-transform.position).sqrMagnitude < 40 * 40)
        {
            yield return new WaitForFixedUpdate();

            transform.Translate(transform.forward * 0.4f, Space.World);

            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.LookRotation(boss - transform.position + Vector3.up*0.5f, Vector3.up), 0.03f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HI");
        if (!collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    private void Die()
    {
        StopCoroutine(process);

        explosion.Play();

        StartCoroutine(WaitThenDestroy(3f));
    }

    IEnumerator WaitThenDestroy(float t)
    {
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(t);

        Destroy(gameObject);
    }
}

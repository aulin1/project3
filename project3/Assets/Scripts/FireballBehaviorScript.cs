using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviorScript : MonoBehaviour
{
    [SerializeField] ParticleSystem fire, explosion;

    IEnumerator process;

    private void Start()
    {
        process = IEProcess();

        StartCoroutine(process);
    }

    IEnumerator IEProcess()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            transform.Translate(transform.forward * 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(process);

        explosion.Play();

        StartCoroutine(WaitThenDestroy(1.2f));
    }

    IEnumerator WaitThenDestroy(float t)
    {
        yield return new WaitForSeconds(t);

        Destroy(gameObject);
    }
}

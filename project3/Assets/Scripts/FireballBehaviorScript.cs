using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviorScript : MonoBehaviour
{
    [SerializeField] ParticleSystem fire, explosion;
    [SerializeField] Vector3 dest;

    IEnumerator process;

    private void Start()
    {
        process = IEProcess();

        StartCoroutine(process);
    }

    IEnumerator IEProcess()
    {
        while ((transform.position - dest).sqrMagnitude > 0.5f)
        {
            yield return new WaitForFixedUpdate();

            transform.position = Vector3.MoveTowards(transform.position, dest, 0.5f);
        }

        Die();
    }

    private void Die()
    {
        StopCoroutine(process);

        explosion.Play();

        StartCoroutine(WaitThenDestroy(0.6f));
    }

    IEnumerator WaitThenDestroy(float t)
    {
        yield return new WaitForSeconds(t);

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingMissileAttack : MonoBehaviour
{
    public Transform player;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other){
        if(!other.CompareTag("boss")){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other){
        if(!other.gameObject.CompareTag("boss")){
            Destroy(gameObject);
        }
    }
}

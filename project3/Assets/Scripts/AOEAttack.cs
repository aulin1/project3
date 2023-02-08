using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;

    private float internalTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
        internalTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        internalTimer += Time.deltaTime;
        if(internalTimer <= 3){

            Vector3 ppos = player.position;
            ppos.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, ppos, speed * Time.deltaTime);
        } else if(internalTimer >= 5){
            transform.localScale = new Vector3(7f, 1f, 7f);
            Destroy(gameObject, 0.1f);
        }
    }
}

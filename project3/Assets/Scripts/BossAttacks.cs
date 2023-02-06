using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private float internalTimer;
    private float nextAttack;


    public bool bossAttack;
    public GameObject missile;
    public GameObject AOE;

    // Start is called before the first frame update
    void Start()
    {
        internalTimer = 0f;
        bossAttack = false;
        nextAttack = Random.Range(0.2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        internalTimer += Time.deltaTime;
        if(!bossAttack){
            if(internalTimer >= nextAttack){
                bossAttack = true;
                internalTimer = 0;
                float chooseAttack = Random.Range(1, 4);
                if(chooseAttack == 3){
                   Instantiate(AOE, gameObject.transform.position, Quaternion.identity);
                } else {
                    chooseAttack = Random.Range(1, 4);
                    int i = 0;
                    while(i < chooseAttack){
                        Invoke("createMissile", 0.1f);
                        i++;
                    }
                }
            }
        } else {
            if(GameObject.FindGameObjectWithTag("attack") == null){
                bossAttack = false;
                internalTimer = 0;
                nextAttack = Random.Range(0.2f, 2f);
            }
            /*if(internalTimer >= 5){
                bossAttack = false;
                internalTimer = 0;
                nextAttack = Random.Range(0.2f, 2f);
            }*/
        }
    }

    void createMissile(){
        Instantiate(missile, gameObject.transform.position, Quaternion.identity);
    }
}

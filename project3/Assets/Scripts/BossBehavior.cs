using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public int HP;

    private Color col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Renderer>().material.color;
        HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //For testing purposes only
        if(Input.GetKeyDown(KeyCode.Space)){  
            damageTaken(10);
        }   

        if(HP <= 0){
            Destroy(gameObject);
        }
    }

    void damageTaken(int damage){
        HP = HP - damage;
        GetComponent<Renderer>().material.color = Color.red;
        Invoke("changeColorBack", 0.5f);
    }

    void OnCollisionEnter(Collision collision){
        if(!(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("attack"))){
            damageTaken(10);
        }
    }    

    void changeColorBack(){
        GetComponent<Renderer>().material.color = col;
    }    
}

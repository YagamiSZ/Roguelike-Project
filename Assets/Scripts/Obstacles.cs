using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public float knockback = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

   
    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D collider)
    {       
        if(collider.gameObject.tag == "Player")
        {
           Vector2 playerPos = collider.gameObject.transform.position; // Acessar a posição atual do player

           CharacterProperties.HP--; // Diminuir o hp em 1 unidade

           playerPos = new Vector2(playerPos.x - knockback * (transform.position.x - playerPos.x), playerPos.y - knockback * (transform.position.y - playerPos.y)); // Simular relação angular obstáculo-personagem baseado na distância(x,y) entre eles
           collider.gameObject.transform.position = playerPos; // Setar a posição           
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
   
    Rigidbody2D rb2;
    float hAxis; // Eixo horizontal
    float vAxis; // Eixo vertical
    public float ms { get; set; } // Movement Speed

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>(); // Acessar o rigidbody do personagem
        ms = 3;
    }

    // Update is called once per frame
    void Update()
    {


    }

    void FixedUpdate()
    {
        hAxis = Input.GetAxis("Horizontal"); // Atualizar valores dos axis
        vAxis = Input.GetAxis("Vertical");
        transform.Translate(new Vector2(ms*hAxis*Time.fixedDeltaTime,ms*vAxis*Time.fixedDeltaTime));    
    }
}

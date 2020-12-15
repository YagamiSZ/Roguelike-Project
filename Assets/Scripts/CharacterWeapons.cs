using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterWeapons : MonoBehaviour
{
    public GameObject normalBullet;
    Vector2 relativeMousePos; // Posição do mouse em relação às dimensões da tela,"setando",assim,a origem dele para o meio dela
    float equalizer; // Igualar a posição X do mouse com a posição Y dele
    Vector2 velocity; // Fórmula da velocidade angular do tiro
    GameObject bullet; // Bala clonada a partir da prefab "normalBullet"
    float relativeDistance; // Distância inicial bullet-character
    public float cooldown = 0.5f; // CD para evitar flood de tiro
    public float storedCD; // Manter o valor original do CD salvo quando modificado
    public float knockback = 0.2f; // Empurrar o personagem na direção contrária do tiro
    public bool canBeKB = false; // Indica se o personagem vai sofrer ação do knockback ou não
    bool onCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        relativeDistance = transform.localScale.x + 0.3f;
        storedCD = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        relativeMousePos = new Vector2((Input.mousePosition.x - (Screen.width / 2)), (Input.mousePosition.y - (Screen.height / 2)));
        equalizer = Mathf.Abs(relativeMousePos.x) + Mathf.Abs(relativeMousePos.y);
        velocity = new Vector2((relativeMousePos.x / Mathf.Abs(relativeMousePos.x)) * Mathf.Sqrt(Mathf.Abs((relativeMousePos.x / equalizer))),(relativeMousePos.y / Mathf.Abs(relativeMousePos.y)) * (Mathf.Sqrt(Mathf.Abs((relativeMousePos.y / equalizer)))));


        if (equalizer == 0) //Prevenir que o equalizer seja 0
        {
            equalizer += 1 / 1000;
        }

        if (Input.GetMouseButtonDown(0) && onCooldown == false)
        {
            if (relativeMousePos.x != 0 && relativeMousePos.y != 0)
            {
                bullet = Instantiate(normalBullet, new Vector2(transform.position.x + velocity.x*relativeDistance, transform.position.y + velocity.y*relativeDistance), Quaternion.identity);
                bullet.GetComponent<Bullet>().velocity = velocity; // Acessar a script da nova "bullet" criada
                if(canBeKB == true) transform.Translate(new Vector2(-velocity.x*knockback,-velocity.y*knockback)); // O knockback só vai funcionar se a field "canBeKB" retornar "true"
                onCooldown = true;
            }

            //Evitar erros caso uma das coordenadas seja igual a 0
            else if (relativeMousePos.x == 0)
            {
                bullet = Instantiate(normalBullet, new Vector2(transform.position.x + 0, transform.position.y + velocity.y*relativeDistance), Quaternion.identity);
                bullet.GetComponent<Bullet>().velocity = new Vector2(0,velocity.y);
                if(canBeKB == true) transform.Translate(new Vector2(0,-velocity.y*knockback));
                onCooldown = true;
            }

            else if (relativeMousePos.y == 0)
            {
                bullet = Instantiate(normalBullet, new Vector2(transform.position.x + velocity.x*relativeDistance, transform.position.y + 0), Quaternion.identity);
                bullet.GetComponent<Bullet>().velocity = new Vector2(velocity.x,0);
                if(canBeKB == true) transform.Translate(new Vector2(-velocity.x*knockback,0));
                onCooldown = true;
            }
        }
        else if(onCooldown == true)
        {
            if (cooldown <= 0)
            {
                onCooldown = false;
                cooldown = storedCD;
            }
            else cooldown -= Time.deltaTime;
        }
    }
}

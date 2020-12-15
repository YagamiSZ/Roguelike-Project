using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    float cooldown = 3.0f; // Tempo para a tela de morte sumir e os componentes se reinicializarem
    static bool dead = false;

    public GameObject gameObjects;
    public GameObject Character;
    public GameObject startUI;
    public GameObject hpText;
    public GameObject deadScreen;
    public GameObject deadText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dead == true && cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if(cooldown <= 0) // Reinicializar componentes
        {
            cooldown = 3.0f; // Reiniciar o cooldown para uso futuro
            dead = false;
            Character.transform.position = new Vector2(0, 0); // Resetar a posição do personagem
            Character.SetActive(true);
            gameObjects.SetActive(false);
            hpText.SetActive(true);
            deadScreen.SetActive(false);
            deadText.SetActive(false);
            startUI.SetActive(true);
        }
    }

    public static void isDead()
    {
        dead = true;
    }
}

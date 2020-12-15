using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorFound : MonoBehaviour
{

    // Acessar a tela que vai aparecer quando o personagem encontrar a porta
    public GameObject winS;

    //Acessar o texto que vai aparecer quando o personagem encontrar a porta
    public GameObject winT;

    public GameObject gameObjects;
    public GameObject startUI;
    public GameObject character;

    bool doorFound = false;

    float cooldown = 3.0f; // Tempo que demora para o conjunto winS e winT sumir

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doorFound == true && cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if(cooldown <= 0)
        {
            winS.SetActive(false);
            winT.SetActive(false);
            startUI.SetActive(true);
            character.transform.position = new Vector3(0, 0, 0); // Restar a posição do personagem
            GenerateMap.redo = true; // Gerar um novo mapa
            doorFound = false;
            cooldown = 3.0f;
            gameObjects.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            winS.SetActive(true);
            winT.GetComponent<TextMeshProUGUI>().text = $"Congratulations {CharacterProperties.NICK},You found the door".ToUpper();
            winT.SetActive(true);
            doorFound = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour
{
    public static int HP { get; set; }
    public static string NICK { get; set; }

    //Fields relacionadas à morte do persoangem
    public GameObject deadText;
    public GameObject startScreen;
    public GameObject deadScreen;
    public GameObject hpText;
    string[] deadMsgs;

    // Start is called before the first frame update
    void Start()
    {
        HP = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // HP

        deadMsgs = new string[] { $"{NICK} WAS OOF'ED", $"{NICK} HIT HARD THE GROUND", $"WE WILL NEVER FORGET YOU,{NICK}" , $"{NICK} MET GOD" , $"{NICK} WAS HULK SMASHED" , $"{NICK} IS WEAK" , $"{NICK} SQUARE PANTS" , $"{NICK} DIED" , $"{NICK} GOT AN INFECTION AND DIED BY NATURAL CAUSES" , $"MOSQUITOS KILLED {NICK}.WE FEEL BAD FOR HIM ):" , "EASTER EGG" , "YOU AREN'T WEAK,JUST NOT STRONG ENOUGH" , $"HOPE TO SEE YOU AGAIN,{NICK}" , "NEVER GONNA GIVE YOU UPPP" , $"{NICK} WALKED OUT FROM THIS GAME TO PLAY MAINECRAFT (;"};

        if (HP <= 0)
        {
            Dead.isDead(); // Chamar a função isDead para reinicializar os componentes

            int randomNumber = new System.Random().Next(0, deadMsgs.Length); // Escolher uma mensagem de morte aleatória
            deadText.GetComponent<TMPro.TextMeshProUGUI>().text = deadMsgs[randomNumber]; // Setar a mensagem

            HP = 3;
            gameObject.SetActive(false);
            hpText.SetActive(false);
            deadScreen.SetActive(true);
            deadText.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button button;
    public GameObject startUI;
    public GameObject gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>(); // Acessar as propriedades do botão
        button.onClick.AddListener(OnButtonClicked); // Inscrever o método ao evento "onClick"
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClicked()
    {
            CharacterProperties.NICK = NickInput.text.text; // Setar o nick do personagem de acordo com o input dado
            startUI.SetActive(false);
            gameObjects.SetActive(true);
    }
}

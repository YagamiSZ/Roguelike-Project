using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hp : MonoBehaviour
{
    TextMeshProUGUI textObj;
      
    // Start is called before the first frame update
    void Start()
    {
        textObj = GetComponent<TextMeshProUGUI>(); // Acessar o componente do TMPro para conseguir acessar as propriedades dele
    }

    // Update is called once per frame
    void Update()
    {
        textObj.text = $"Hp: {CharacterProperties.HP}"; // Atualizar o texto constantemente
    }
}

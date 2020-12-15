using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickInput : MonoBehaviour
{
    public static TextMeshProUGUI text;
    public static Input input;
   
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>(); // Acessar o componente textmeshpro do "GameObject"
        input = GetComponent<Input>();  // Acessar o campo de input do texto
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

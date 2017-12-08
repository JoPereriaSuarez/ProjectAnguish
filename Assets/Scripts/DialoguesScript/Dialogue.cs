using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  CODIGO PARA LOS DIALOGOS
 *  De momento solo guarda los datos de los dialogos
 *  "textToShow" corresponde al texto inicial 
 *  antes de mostrar las alternativas.
 *  Las "options" corresponden al texto que se muestra en cada opcion
 *  "answer" son los dialogos con las reaccion del personaje
 *  al selecionar una alternativa (el indice se corresponde con el "options")
 *  Cada grupo de dialogo, puede tener un requisito del inventario.
 *  para eso hay que indicar el indice en las respuestas.
 */
[CreateAssetMenu(fileName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [Header("Answer Options")]

    [TextArea]  [SerializeField] string     textToShow;
    [TextArea]  [SerializeField] string[]   options;
                [SerializeField] Dialogue[] _answers;

    public Dialogue[] answers { get { return _answers; } }
    public bool hasOptions
    {
        get
        {
            return (options.Length > 0) ? true : false;
        }
    }

    [Header("Answer Requirement")]
    public InventaryObject answerRequirement;
    public int requirementIndex = 0;
    [TextArea] public string  defaultAnswer;

    public string GetOptionAt(int index)
    {
        return options[index];
    }
    public Dialogue GetDialogueAt(int index)
    {
        return _answers[index];
    }
    public string GetInitialText()
    {
        return textToShow;
    }
    public string[] GetAllOptions()
    {
        return options;
    }
}

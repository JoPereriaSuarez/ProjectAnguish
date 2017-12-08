using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    item,
    dialogue,
    dialogueWithOptions,
}
public class HUDSystem : MonoBehaviour
{
    public delegate void OnTextEndEventHandler(TextType type);
    public event OnTextEndEventHandler OnTextEnded;

    GameController controller;

    [SerializeField] string moveThroughText = "Interact";
    [SerializeField] string cancelTextButton = "Cancel";
    [Header("Dialogue HUD")]
    [SerializeField] private GameObject dialogueOptionPanel;
    [SerializeField] private GameObject normalDialoguePanel;
    [SerializeField] private Button[]   dialogueOptions;

    [Header("Item HUD")]
    [SerializeField] private GameObject itemPanel;

    [Header("Inventory")]
    [SerializeField] private Image[] inventoryHUD;

    // TEXT COMPONENT
    private Text itemText;
    private Text dialogueText;
    private Text normalDialogueText;

    private Text currentText;

    private GameObject generalDialoguePanel;
    private bool hasPressedInput;
    private int index;
    private string[] textsToDisplay;
    public bool isShowingText { get; set; }
    private string lastText;

    private void Start()
    {
        controller = GameController.instance;

        itemText = itemPanel.GetComponentInChildren<Text>();
        dialogueText = dialogueOptionPanel.transform.Find("OptionTextPanel").GetComponentInChildren<Text>();
        normalDialogueText = normalDialoguePanel.GetComponentInChildren<Text>();

        generalDialoguePanel = normalDialoguePanel.transform.parent.gameObject;

        generalDialoguePanel.SetActive(false);
        itemPanel.SetActive(false);
        dialogueOptionPanel.SetActive(false);
        normalDialoguePanel.SetActive(false);

        foreach(Button button in dialogueOptions)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ActiveItemPanel(bool value)
    {
        itemPanel.SetActive(value);
        if(value)
        {
            generalDialoguePanel.SetActive(false);
            currentText = itemText;
            ActiveDialoguePanel(false);
            ActiveDialogueWithOptions(false);
        }
    }
    public void ActiveDialoguePanel(bool value)
    {
        generalDialoguePanel.SetActive(value);
        if (value)
        {
            
            currentText = normalDialogueText;
            ActiveItemPanel(false);
        }
        dialogueOptionPanel.SetActive(false);
        normalDialoguePanel.SetActive(value);
    }
    public void ActiveDialogueWithOptions(bool value, int number = 4)
    {
        generalDialoguePanel.SetActive(value);
        if (value)
        {          
            dialogueOptionPanel.SetActive(value);
            ActiveItemPanel(false);
        }

        for (int i = 0; i < number; i++)
        { 
            dialogueOptions[i].gameObject.SetActive(value);
        }
    }

    public void SetText(TextType type, string text, float speed = 0)
    {
        if(isShowingText)
        { return; }

        textsToDisplay = text.Split(new string[] { "/n" }, System.StringSplitOptions.RemoveEmptyEntries);
        print(textsToDisplay[0]);


        switch (type)
        {
            case TextType.item:
                ActiveItemPanel(true);
                break;
            case TextType.dialogue:
                ActiveDialoguePanel(true);
                break;
        }


        StartCoroutine(WaitToSetText(textsToDisplay[0], 1 / speed, type));
        try
        {
            lastText = textsToDisplay[textsToDisplay.Length - 1];
        }
        catch(System.IndexOutOfRangeException)
        {
            
        }

    }
    public void SetText(string[] options)
    {
        int n_options = options.Length;

        
        dialogueText.text = lastText;
        for (int i = 0; i < n_options; i++)
        {
            dialogueOptions[i].GetComponentInChildren<Text>().text = options[i];
        }
        ActiveDialogueWithOptions(true, n_options);
        isShowingText = true;   
    }
    
    // Espera una cantidad de tiempo para mostrar el siguiente "char" del texto
    IEnumerator WaitToSetText(string text, float s, TextType type)
    {
        currentText.text = "";
        isShowingText = true;
        // Convierte el texto a mostrar en un arreglo de "char"
        char[] textToChar = text.ToCharArray();
        for (int i = 0; i < text.Length; i++)
        {
            currentText.text +=  textToChar[i].ToString();
            yield return new WaitForSecondsRealtime(s);
        }

        index++;
        hasPressedInput = false;
        yield return new WaitUntil(() => hasPressedInput == true);

        if(index < textsToDisplay.Length)
        {
            StartCoroutine(WaitToSetText(textsToDisplay[index], s, type));
        }
        else
        {
            isShowingText = false;
            hasPressedInput = false;
            index = 0;  
            switch (type)
            {
                case TextType.dialogue:
                    ActiveDialoguePanel(false);
                    if (OnTextEnded != null)
                    {
                        OnTextEnded(type);
                    }
                    break;
                case TextType.dialogueWithOptions:
                    ActiveDialogueWithOptions(false);
                    break;
                case TextType.item:
                    ActiveItemPanel(false);
                    break;
            }

            textsToDisplay = new string[0];

            if(controller !=null)
            {
                controller.EndInteraction();
            }
        }

    } // End Coroutine

    public void AddInventaryImage(Sprite image)
    {
        for (int i = 0; i < inventoryHUD.Length; i++)
        {
            if(inventoryHUD[i].sprite == null)
            {
                inventoryHUD[i].sprite = image;
                break;
            }
        }
    }
    public void RemoveInventaryImage(Sprite image)
    {
        for (int i = 0; i < inventoryHUD.Length; i++)
        {
            if(inventoryHUD[i].sprite == image)
            {
                inventoryHUD[i].sprite = null;
            }
        }
    }


    private void Update()
    {
        if (Input.GetButton(moveThroughText) && !hasPressedInput)
        {
            hasPressedInput = true;
        }
        if(Input.GetButtonDown(cancelTextButton) && isShowingText)
        {
            StopAllCoroutines();

            isShowingText = false;
            index = 0;
            hasPressedInput = false;
            textsToDisplay = new string[0];
            currentText = null;

            ActiveDialoguePanel(false);
            ActiveDialogueWithOptions(false);
            ActiveItemPanel(false);

        }
    }
}

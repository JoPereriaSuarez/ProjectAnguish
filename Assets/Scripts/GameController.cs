using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : ControllerBase
{
    public static GameController instance;

    public bool replaceForThis = false;

    [SerializeField] HUDSystem _hud;
    public HUDSystem hud
    {
        get
        {
            if(_hud == null)
            {
                _hud = FindObjectOfType<HUDSystem>();
            }
            return _hud;
        }
    }

    public List<InventaryObject> inventary = new List<InventaryObject>();
    [SerializeField] private List<InventaryObject> goodThings = new List<InventaryObject>();
    [SerializeField] private List<InventaryObject> badThings  = new List<InventaryObject>();

    public CustomCharacterController player { get; private set; }
    private ControllerBase playerControllerBase;
    private Dialogue npcDialogue;
    public IInteractable interactalbeObject;

    private void Awake()
    {
        if(instance != null)
        {
            if(replaceForThis)
            {
                Destroy(GameController.instance.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        player = FindObjectOfType<CustomCharacterController>();
        playerControllerBase = player.GetComponent<ControllerBase>();
    }

    private void Start()
    {
        hud.OnTextEnded += OnHUDFinished;
        for (int i = 0; i < inventary.Count; i++)
        {
            hud.AddInventaryImage(inventary[i].sprite);
        }
    }

    void OnHUDFinished (TextType type)
    {
        if(type == TextType.dialogue || type == TextType.dialogueWithOptions && npcDialogue != null)
        {
            string[] options = npcDialogue.GetAllOptions();
            if(options.Length > 0)
            {
                hud.SetText(options);
            }
        }
    }

    public void SendAnswer(int index)
    {
        if(npcDialogue.answerRequirement != null && npcDialogue.requirementIndex == index && FindOnInventary(npcDialogue.answerRequirement))
        {
            npcDialogue = npcDialogue.answers[index];
            Show("showNPCText." + npcDialogue.GetInitialText(), 20F, npcDialogue.hasOptions, this, false);
            inventary.Remove(npcDialogue.answerRequirement);
        }
        else if(npcDialogue.answerRequirement != null && npcDialogue.requirementIndex == index && !FindOnInventary(npcDialogue.answerRequirement))
        {
            hud.isShowingText = false;
            Show("showText." + npcDialogue.defaultAnswer, 20F);
        }
        else
        {
            npcDialogue = npcDialogue.answers[index];
            Show("showNPCText." + npcDialogue.GetInitialText(), 20F, npcDialogue.hasOptions, this, false);
        }
    }

    protected override void UpdateController()
    {
        return;
    }

    new void Update()
    {
        //  TEST PARA PAUSE PLAYER
        if(Input.GetKeyDown(KeyCode.P))
        {
            PausePlayer();
        }

        if(playerControllerBase.pauseController && !hud.isShowingText)
        {
            PausePlayer(false);
        }
    }

    public void PausePlayer()
    {
        playerControllerBase.pauseController = !playerControllerBase.pauseController;
    }
    public void PausePlayer(bool value)
    {
        playerControllerBase.pauseController = value;
    }

    public void Show(string message, params object[] args)
    {
        string[] instructions = message.Split('.');

        switch(instructions[0])
        {
            // MOSTRAR TEXTO MEDIANTE LA INTERFAZ
            // SI NO SE HA ASIGNADO UNA, IMPRIME POR CONSOLA
            case ("showText"):
                if(hud == null)
                {
                    Debug.LogWarning("NO ESTA ASIGNADO UN HUD EN EL GAME MANAGER");
                    print(instructions[1]);
                    return;
                }

                if (args.Length > 0 && args[0] is float)
                {
                    float speed = (float)args[0];
                    hud.SetText(TextType.item, instructions[1], speed);
                    PausePlayer(true);
                }
                break;
            case ("showNPCText"):
                if (hud == null)
                {
                    Debug.LogWarning("NO ESTA ASIGNADO UN HUD EN EL GAME MANAGER");
                    print(instructions[1]);
                    return;
                }

                if(args.Length == 0 || args[0] is float == false || args[1] is bool == false)
                {
                    Debug.LogError("AGREGAR MAS DATOS PARA CREAR MOSTRAR UN DIALOGO DE NPC");
                    return;
                }
                if(args[2] is NPCController)
                {
                    NPCController currentNPC = args[2] as NPCController;
                    npcDialogue = currentNPC.dialogue;
                }
                if(args.Length >=4 && args[3] is bool)
                {
                    hud.isShowingText = (bool)args[3];
                }
                bool showOptions = (bool) args[1];
                float _speed = (float) args[0];

                
                hud.SetText(TextType.dialogue, instructions[1], _speed);
                PausePlayer(true);
                break;
        }
    }

    public void EndInteraction()
    {
        if(interactalbeObject != null)
        {
            interactalbeObject.OnInteractionEnds();
        }
    }

    public bool FindOnInventary(InventaryObject requirement)
    {
        bool value = false;
        if(inventary.Count > 0)
        {
            for (int i = 0; i < inventary.Count; i++)
            {
                if (inventary[i] == requirement)
                {
                    value = true;
                    inventary.Remove(requirement);
                    if(hud != null)
                    {
                        hud.RemoveInventaryImage(requirement.sprite);
                    }
                    break;
                }
            }
        }

        return value;
    }
    public void AddToInventory(InventaryObject objectToAdd)
    {
        inventary.Add(objectToAdd);
        if(hud != null)
        {
            hud.AddInventaryImage(objectToAdd.sprite);
        }
    }
}

using UnityEngine;
using System.Collections;
using System;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialogue initialDialogue;
    public Dialogue dialogue { get { return initialDialogue; } }
    private bool isOnCollision = false;

    public bool GetIsInCollision()
    {
        return isOnCollision;
    }
    public void OnInteract()
    {
        GameController.instance.Show("showNPCText." + initialDialogue.GetInitialText(), 20F, initialDialogue.hasOptions, this);
    }
    public void OnInteractionEnds()
    {
        return;
    }
    public void SetIsInCollision(bool value)
    {
        isOnCollision = value;
    }
}

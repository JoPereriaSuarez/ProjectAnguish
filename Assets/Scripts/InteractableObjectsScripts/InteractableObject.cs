using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    public delegate void OnStartInteractionEventHandler(GameObject sender, params object[] args);
    public event OnStartInteractionEventHandler OnStartInteraction;

    public delegate void OnEndInteractionEventHandler(GameObject sender, params object[] args);
    public event OnEndInteractionEventHandler OnEndedInteraction;

    [TextArea] [SerializeField] private string textToShow;
    [TextArea] [SerializeField] private string defaultText;
    bool isOnCollision = false;
    public InventaryObject requirement;
    public InventaryObject reward;

    public void OnInteract()
    {
        if (requirement != null && !GameController.instance.FindOnInventary(requirement))
        {
            GameController.instance.interactalbeObject = this as IInteractable;
            if(defaultText == default(string) || defaultText == "")
            {
                GameController.instance.Show("showText." + "Necesito un " + requirement.name, 20F);
            }
            else
            {
                GameController.instance.Show("showText." + defaultText, 20F);
            }
            return;
        }

        if (OnStartInteraction != null)
        {
            OnStartInteraction(this.gameObject, null);
        }
        GameController.instance.interactalbeObject = this as IInteractable;
        GameController.instance.Show("showText." + textToShow, 20F);
        if(reward != null)
        {
            GameController.instance.AddToInventory(reward);
            reward = null;
        }

        if(requirement != null)
        {
            requirement = null;
        }
    }

    public void OnInteractionEnds()
    {
        if(OnEndedInteraction != null)
        {
            OnEndedInteraction(this.gameObject, null);
        }
        isOnCollision = false;
    }

    public bool GetIsInCollision()
    {
        return isOnCollision;
    }

    public void SetIsInCollision(bool value)
    {
        isOnCollision = value;
    }
}

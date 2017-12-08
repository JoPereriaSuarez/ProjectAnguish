using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simplemente al interactuar con el objeto, elige entre una de las opciones
 * Luego muestra esa opcion, para finalmente borrarla de la lista.
 */

[RequireComponent(typeof(Collider2D))]
public sealed class MultipleInteractableObject : MonoBehaviour, IInteractable
{
    public delegate void OnStartInteractionEventHandler(GameObject sender, params object[] args);
    public event OnStartInteractionEventHandler OnStartInteraction;

    public delegate void OnEndInteractionEventHandler(GameObject sender, params object[] args);
    public event OnEndInteractionEventHandler OnEndedInteraction;

    public InventaryObject requirement;
    public InventaryObject reward;
    [SerializeField] int correctIndex = 0;
    private string correctAnswer; 
    [TextArea] [SerializeField] private List<string> textsOptions = new List<string>(2);
    int selectedIndex = 0;
    bool isOnCollsiion;

    private void Start()
    {
        correctAnswer = textsOptions[correctIndex];
    }

    public bool GetIsInCollision()
    {
        return isOnCollsiion;
    }

    public void OnInteract()
    {
        if(requirement != null && !GameController.instance.FindOnInventary(requirement))
        {
            GameController.instance.interactalbeObject = this as IInteractable;
            GameController.instance.Show("showText." + "Necesito un " + requirement.name, 10F);
            return;
        }
        if (textsOptions.Count <= 0) 
		{
			return;
		}

        if (OnStartInteraction != null)
        {
            OnStartInteraction(this.gameObject, selectedIndex);
        }

        selectedIndex = UnityEngine.Random.Range(0, textsOptions.Count);
        if(GameController.instance != null)
        {
            GameController.instance.interactalbeObject = this as IInteractable;
            GameController.instance.Show("showText." + textsOptions[selectedIndex], 20F);
            if(reward != null && textsOptions[selectedIndex] == correctAnswer)
            {
                GameController.instance.AddToInventory(reward);
            }
        }

        if (requirement != null && GameController.instance != null && GameController.instance.FindOnInventary(requirement))
        {
            requirement = null;
        }

        textsOptions.RemoveAt(selectedIndex);
        textsOptions.TrimExcess();
    }

    public void OnInteractionEnds()
    {
        if (OnEndedInteraction != null)
        {
            OnEndedInteraction(this.gameObject, selectedIndex);
        }

        isOnCollsiion = false;
    }

    public void SetIsInCollision(bool value)
    {
        isOnCollsiion = value;
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractableObject))]
[RequireComponent(typeof(Animator))]
public class SingleInteractAnimatorScript : MonoBehaviour
{
    Animator anim;
    InteractableObject interactalbe;
    [SerializeField] string interactParam = "open";

    private void Start()
    {
        anim = GetComponent<Animator>();
        interactalbe = GetComponent<InteractableObject>();
        interactalbe.OnStartInteraction += OnInteract;
    }

    void OnInteract(GameObject sender, params object[] args)
    {
        if (sender == this.gameObject)
        {
            anim.SetBool(interactParam, true);
        }
    }
}

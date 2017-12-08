using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MultipleInteractableObject))]
[RequireComponent(typeof(Animator))]
public class MultipleInteractAnimatorScript : MonoBehaviour
{
    Animator anim;
    MultipleInteractableObject interactalbe;
    int index = 0;
    [SerializeField] string[] openCaseParams = new string[] {"open1", "open2" };

    private void Start()
    {
        anim = GetComponent<Animator>();
        interactalbe = GetComponent<MultipleInteractableObject>();
        interactalbe.OnStartInteraction += OnInteract;
    }

    void OnInteract(GameObject sender, params object[] args)
    {
       if(sender == this.gameObject && args.Length > 0 && args[0] is int)
       {
            anim.SetBool(openCaseParams[index], true);
            index++;
        }
    }
}

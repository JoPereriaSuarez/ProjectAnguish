using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomCharacter))]
public class CustomCharacterController : ControllerBase
{
    [SerializeField] string verticalInput = "Vertical";
    [SerializeField] string horizontalInput = "Horizontal";
    [SerializeField] string interactInput = "Interact";

    Vector2 dir;
    CustomCharacter model;

    private void Awake()
    {
        model = GetComponent<CustomCharacter>();
    }

    protected sealed override void UpdateController()
    {
        dir.x = Input.GetAxis(horizontalInput);
        dir.y = Input.GetAxis(verticalInput);

        model.Move(dir);
    }

    IInteractable interactable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableObject"))
        {
            model.rig2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            interactable = collision.GetComponent<IInteractable>();
            interactable.SetIsInCollision(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(pauseController)
        {
            return;
        }

        if(interactable != null)
        {
            if (Input.GetButtonDown(interactInput))
            {
                interactable.OnInteract();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableObject"))
        {
            model.rig2D.sleepMode = RigidbodySleepMode2D.StartAwake;
            if(interactable != null)
            {
                interactable.SetIsInCollision(false);
            }
        }
    }
    protected override void OnPaused()
    {
        model.StopMoving();
    }
    protected override void OnUnPaused()
    {
        model.NormalizeAnimator();
    }
}

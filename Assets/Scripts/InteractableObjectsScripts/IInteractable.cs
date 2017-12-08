using UnityEngine;
using System.Collections;

public interface IInteractable
{
    void OnInteract();
    void OnInteractionEnds();
    bool GetIsInCollision();
    void SetIsInCollision(bool value);
}

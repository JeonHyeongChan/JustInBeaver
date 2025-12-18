using UnityEngine;

public interface IInteractable
{
    string InteractText { get; } // UI 안내문구
    Transform UIAnchor { get; }
    void Interact(PlayerController player);
}

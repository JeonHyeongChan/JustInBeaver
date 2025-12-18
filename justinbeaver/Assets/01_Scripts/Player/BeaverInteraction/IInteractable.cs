using UnityEngine;

public interface IInteractable
{
    public string InteractText { get; } // UI 안내문구
    public Transform UIAnchor { get; } // UI 표시할 위치
    public bool RequiresHold { get; }
    public float HoldDuration { get; }

    public void Interact(PlayerController player);
    public void OnHoldUpdate(PlayerController player, float progress01);
    public void OnHoldCancel(PlayerController player);
}

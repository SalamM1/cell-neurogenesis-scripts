using UnityEngine;

namespace com.egamesstudios.cell
{
    public interface IInteractable
    {
        void TriggerDialogueState(Transform CellPosition);
        void EndDialogueState();
    }
}
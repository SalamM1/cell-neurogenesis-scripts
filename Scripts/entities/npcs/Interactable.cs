using UnityEngine;

namespace com.egamesstudios.cell
{
    public interface IInteractable
    {
        void TriggerInteraction(Transform CellPosition);
        void EndInteraction();
    }
}
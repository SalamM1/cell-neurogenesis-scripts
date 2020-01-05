using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "New DropTable", menuName = "Custom/Drop Table")]
    public class DropTable : ScriptableObject
    {
        public GameObject[] pickups;
        
    }
}

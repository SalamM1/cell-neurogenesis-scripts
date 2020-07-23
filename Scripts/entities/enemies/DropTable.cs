using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "New DropTable", menuName = "Custom/Drop Table")]
    public class DropTable : ScriptableObject
    {
        public ObjectAndValue[] pickups;
        
    }
    
    [System.Serializable]
    public class ObjectAndValue
    {
        public GameObject gameObject;
        [Range(0, 1)]
        public float probability;
        public float quantity;
    }
}

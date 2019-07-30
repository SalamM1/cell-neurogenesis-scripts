using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recovery", menuName = "Custom/Recovery")]
public class Pickup : ScriptableObject {

    public PICKUPS type;

    [InlineEditor]
    public Sprite image;
    public int value;

}

public enum PICKUPS
{
    HEALTH,
    ENERGY,
    MONEY,
}

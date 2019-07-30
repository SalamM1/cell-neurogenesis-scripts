using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public interface ISwitch
    {
        void TriggerSwitch();
    }

    public interface IWeightSwitch : ISwitch
    { }

    public interface IGuitarSwitch : ISwitch
    { }

    public interface IGunSwitch : ISwitch
    { }

    //Heavy -> Blocks only
    //Light -> Cell can also activate
    public enum WeightSwitchType
    {
        LIGHT, HEAVY
    }
}

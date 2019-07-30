using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator {

     public static float CalcFixedPos(float valueWithoutPixelPerfection)
    {
        float PPU = 48;
        float resDivision = valueWithoutPixelPerfection * PPU;
        // cut off decimals
        int resDivisionInt = (int)resDivision;
        // get only decimals and multiply with factor 10 to make rounding decision
        float resDivisionDezim = (resDivision - resDivisionInt) * 10;
        // rounding up if decimals > 0.5 (0.5 = 5/10)
        if (resDivisionDezim >= 5)
        {
            resDivisionInt++;
        }
        // resDivisionInt = integer amount number of _invPPU for closestWholePixelValue
        float closestWholePixelValue = resDivisionInt / PPU;
        // return closestWholePixelValeu
        return closestWholePixelValue;
    }

    public static float Speed(float speed)
    {
        return speed / 48;
    }
}

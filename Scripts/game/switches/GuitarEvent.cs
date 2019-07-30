using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{

	public interface IGuitarEvent
	{
        void ActivateEvent(CellController cell);
	}
}

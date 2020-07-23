using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
	public class BeatBlockParent : MonoBehaviour
	{
        [SerializeField]
        private float BLOCK_TIMER = 5.0f; //Blink at 3.5, 4 and 4.5
        [SerializeField]
        private float BLOCK_SOUND_TIMER = 0.5f;
        private BeatBlockChild[] beatBlocks;
        private float blockTimer, blockSoundTimer;
        private bool playSound;

        private void Awake()
        {
            beatBlocks = FindObjectsOfType<BeatBlockChild>();
            blockSoundTimer = BLOCK_SOUND_TIMER;
        }
        void Update()
		{
			if(blockTimer >= BLOCK_TIMER)
            {
                for (int i = 0; i < beatBlocks.Length; i++)
                {
                    beatBlocks[i].SwitchActive();
                }
                blockTimer = 0;
                playSound = false;
                blockSoundTimer = BLOCK_SOUND_TIMER;
            }
            else
            {
                blockTimer += Time.deltaTime;
                if(blockTimer >= (BLOCK_TIMER - BLOCK_SOUND_TIMER*3.1f) && blockTimer <= (BLOCK_TIMER - BLOCK_SOUND_TIMER))
                {
                    if(blockSoundTimer >= BLOCK_SOUND_TIMER)
                    {
                        for (int i = 0; i < beatBlocks.Length; i++)
                        {
                            beatBlocks[i].Blink();
                        }
                        blockSoundTimer = 0;
                    }
                    else
                    {
                        blockSoundTimer += Time.deltaTime;
                    }
                }
            }
		}
	}
}
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class HatAnimator : MonoBehaviour
    {
        private Animator cellAnim;
        private string previousState;
        private static string[] animationStates;
        private Vector3 originalPos;
        private bool doOnce;

        void Start()
        {
            animationStates = new string[] { "Idle", "Move", "HitGround", "Jump", "Falling", "Die", "WallSlide", "WallJump", "WallCharge",
            "GroundPound", "TakeHit", "FloorCharge"};
            cellAnim = transform.parent.GetComponentInChildren<Animator>();
            previousState = "";
            originalPos = transform.localPosition;
        }

        void Update()
        {
            if (!cellAnim.GetCurrentAnimatorStateInfo(0).IsName(previousState))
            {
                doOnce = false;
                DOTween.Kill(transform);
                transform.localPosition = originalPos;
                transform.localRotation = Quaternion.identity;
                for (int i = 0; i < animationStates.Length; i++)
                {
                    if (cellAnim.GetCurrentAnimatorStateInfo(0).IsName(animationStates[i]))
                    {
                        previousState = animationStates[i];
                        break;
                    }
                }
            }
            if (doOnce && !previousState.Equals("Idle")) return;
            switch (previousState)
            {
                case "Idle":
                    transform.localPosition = originalPos;
                    break;
                case "Move":
                    transform.DOLocalMoveY(originalPos.y - 0.07f, 0.33f).SetLoops(-1, LoopType.Yoyo).Play();                    
                    break;
                case "HitGround":
                    transform.DOLocalMoveY(originalPos.y - 0.168f, 0.18f).SetLoops(2, LoopType.Yoyo).Play();
                    break;
                case "Jump":
                    transform.DOLocalMoveY(originalPos.y - 0.1f, 0.18f).Play();
                    break;
                case "Falling":
                    transform.DOLocalMoveY(originalPos.y + 0.22f, 0.4f).Play();
                    break;
                case "Die":
                    break;
                case "WallSlide":
                    transform.DOLocalMoveY(originalPos.y + 0.08f, 0.2f).Play();
                    transform.DOLocalRotate(new Vector3(0, 0, -35), 0.2f);
                    break;
                case "WallJump":
                    transform.DOLocalMoveY(originalPos.y + 0.1f, 0.18f).Play();
                    break;
                case "WallCharge":
                    break;
                case "GroundPound":
                    break;
                case "TakeHit":
                    break;
                case "FloorCharge":
                    break;
            }
            doOnce = true;
        }
    }
}

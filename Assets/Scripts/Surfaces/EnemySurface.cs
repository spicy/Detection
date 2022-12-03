using System.Collections;
using UnityEngine;

namespace Detection
{
    public class EnemySurface : MonoBehaviour, IScannable, IRevealable
    {
        public int hitCount = 0;
        private int hitThreshold = 40;
        private int hitMax = 200;
        private bool runningReduceHitCount = false;
        [SerializeField] private DissolveController dissolveController;

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            if (hitCount < hitMax) hitCount++;
            if (hitCount > hitThreshold) Reveal();
            if (!runningReduceHitCount) StartCoroutine(ReduceHitCount());
        }

        private IEnumerator ReduceHitCount()
        {
            runningReduceHitCount = true;
            while (hitCount > 0)
            { 
                hitCount -= 25;
                yield return new WaitForSeconds(.1f);
            }
            dissolveController.Disappear();
            runningReduceHitCount = false;
        }

        public void Reveal()
        {
            dissolveController.Appear();
        }
    }
}
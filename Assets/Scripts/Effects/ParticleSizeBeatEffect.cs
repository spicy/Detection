using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
	public class ParticleSizeBeatEffect : MonoBehaviour, IEffect
	{
		public MusicAnalyzer musicAnalyzer;

		public void Initialize(MusicAnalyzer mAnalyzer) 
		{ 
			musicAnalyzer = mAnalyzer;
		}

		void IEffect.DoEffect(double duration, Action callback) => StartCoroutine(DoParticleSizeBeatEffect(duration, callback));

        public IEnumerator DoParticleSizeBeatEffect(double duration, Action callback)
		{
			double currentTimeCount = 0;

			while (currentTimeCount < duration)
			{
				EffectManager.effectManager.effectEmitArgs.size = musicAnalyzer.currentLoudness;
				yield return null;
			}

			// reset the override size
			EffectManager.effectManager.effectEmitArgs.size = null;

			callback();
		}
	}
}

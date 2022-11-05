using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class ParticleSizeBeatEffect : MonoBehaviour, IEffect
	{
		public List<ParticleSystem> particleSystems;
		public MusicAnalyzer musicAnalyzer;
		private List<float> startParticleSizes;

		public void Initialize(List<ParticleSystem> ps, MusicAnalyzer mAnalyzer) 
		{ 
			particleSystems = ps;
			musicAnalyzer = mAnalyzer;
		}

		void IEffect.DoEffect(double duration, Action callback) => StartCoroutine(DoParticleSizeBeatEffect(duration, callback));

        public IEnumerator DoParticleSizeBeatEffect(double duration, Action callback)
		{
			double currentTimeCount = 0;
			

			while (currentTimeCount < duration)
			{
				for (int i = 0; i < particleSystems.Count; i++)
				{
					startParticleSizes.Add(particleSystems[i].startSize);
					particleSystems[i].startSize = musicAnalyzer.currentLoudness;
				}

				yield return null;
			}

            // reset the initial particle system sizes
            for (int i = 0; i < particleSystems.Count; i++)
			{
                particleSystems[i].startSize = startParticleSizes[i];
			}
			startParticleSizes.Clear();

			callback();
		}
	}
}

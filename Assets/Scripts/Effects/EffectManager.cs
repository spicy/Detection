using System;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class EffectManager : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float loudnessTolerance = 0.8f;
        [SerializeField] private MusicAnalyzer musicAnalyzer;
        public List<ParticleSystem> particleSystems;

        [Range(0.01f, float.MaxValue)]
        [SerializeField] double minEffectDuration = 0.5;
        [Range(0.01f, 30)]
        [SerializeField] double maxEffectDuration = 15;

        public bool allowMultipleEffectsAtOnce = false;
        private bool isApplyingEffect = false;
        private bool waitFlag = false;

        private WeightedRandom<IEffect> weightedEffectsBag;

        private void Awake()
        {
            List<KeyValuePair<IEffect, double>> effectList = new List<KeyValuePair<IEffect, double>>();
            effectList.Add(new KeyValuePair<IEffect, double>(new ParticleSizeBeatEffect(particleSystems, musicAnalyzer), 10));

            // populate the weightedRandom effects bag we can pick from
            foreach (var x in effectList)
            {
                weightedEffectsBag.Add(x.Key, x.Value);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if ((allowMultipleEffectsAtOnce || !isApplyingEffect) && !waitFlag)
            {
                // if currentLoudness is greater than a tolerance percentage of the maxLoudness
                if (musicAnalyzer.currentLoudness > musicAnalyzer.maxLoudness * loudnessTolerance)
                {
                    isApplyingEffect = true;

                    // randomly decide an event and some duration
                    var chosenEffect = weightedEffectsBag.GetRandomWeighted();

                    // generate a random duration within defined min/max values
                    System.Random rand = new System.Random();
                    double randomDuration = rand.NextDouble() * (maxEffectDuration - minEffectDuration) + minEffectDuration;

                    chosenEffect.DoEffect(randomDuration, OnFinishedEffect);
                }
            }
        }

        void OnFinishedEffect()
        {
            isApplyingEffect = false;
        }
    }
}
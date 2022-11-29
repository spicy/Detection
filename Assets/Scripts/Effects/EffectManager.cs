using System;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager effectManager;
        public VFXEmitArgs effectEmitArgs;

        [Range(0, 1)]
        [SerializeField] private float loudnessTolerance = 0.8f;
        private MusicAnalyzer musicAnalyzer;

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
            if (effectManager != null && effectManager != this) Destroy(this);
            else effectManager = this;
        }

        private void Start()
        {
            musicAnalyzer = MusicManager.musicManager.GetComponent<MusicAnalyzer>();
            effectEmitArgs = new VFXEmitArgs(null, null, null);

            List<KeyValuePair<IEffect, double>> effectList = new List<KeyValuePair<IEffect, double>>();

            gameObject.AddComponent<ParticleSizeBeatEffect>().Initialize(musicAnalyzer);
            effectList.Add(new KeyValuePair<IEffect, double>(gameObject.GetComponent<ParticleSizeBeatEffect>(), 10));

            // populate the weightedRandom effects bag we can pick from
            weightedEffectsBag = new WeightedRandom<IEffect>();
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public interface IEffect
    {
        public void DoEffect(double duration, Action callback);
    }
}
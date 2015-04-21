using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialLoadingProgressbarCSharp
{
    public class EndCurveInterpolator : AccelerateDecelerateInterpolator
    {
        public override float GetInterpolation(float input)
        {
            return base.GetInterpolation(Math.Max(0, (input - 0.5f) * 2.0f));
        }
    }
}

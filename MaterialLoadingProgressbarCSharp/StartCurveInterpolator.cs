using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialLoadingProgressbarCSharp
{
    public class StartCurveInterpolator : AccelerateDecelerateInterpolator
    {
        public override float GetInterpolation(float input)
        {
            return base.GetInterpolation(Math.Min(1, input * 2.0f));
        }
    }
}

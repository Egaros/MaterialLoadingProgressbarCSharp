using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;

namespace MaterialLoadingProgressbarCSharp
{
    public class DefaultAnimation : Animation
    {
		private IInterpolator END_CURVE_INTERPOLATOR = new EndCurveInterpolator();
		private IInterpolator START_CURVE_INTERPOLATOR = new StartCurveInterpolator();
		
        private MaterialProgressDrawale mProgressDrawable;

        public DefaultAnimation(MaterialProgressDrawale progressDrawable)
        {
            this.mProgressDrawable = progressDrawable;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            Ring ring = mProgressDrawable.Ring;
            if (mProgressDrawable.Finishing)
            {
                mProgressDrawable.ApplyFinishTranslation(interpolatedTime, ring);
            }
            else
            {
                float minProgressArc = (float)Java.Lang.Math.ToRadians(ring.StrokeWidth / (2 * Math.PI * ring.RingCenterRadius));
                float startingEndTrim = ring.StartingEndTrim;
                float startingTrim = ring.StartingStartTrim;
                float startingRotation = ring.GetStartingRotation();

                float minArc = MaterialProgressDrawale.MAX_PROGRESS_ARC - minProgressArc;
                float endTrim = startingEndTrim + (minArc * START_CURVE_INTERPOLATOR.GetInterpolation(interpolatedTime));
                float startTrim = startingTrim + (MaterialProgressDrawale.MAX_PROGRESS_ARC * END_CURVE_INTERPOLATOR.GetInterpolation(interpolatedTime));

                float sweepTrim = endTrim - StartTime;

                if (Math.Abs(sweepTrim) >= 1)
                {
                    endTrim = startingTrim + 0.5f;
                }

                ring.EndTrim = endTrim;
                ring.StartTrim = startingTrim;

                float rotation = startingRotation + (0.25f * interpolatedTime);
                ring.Rotation = rotation;

                float groupRotation = ((720.0f / MaterialProgressDrawale.NUM_POINTS) * interpolatedTime)
                    + (720.0f * (mProgressDrawable.RotationCount / MaterialProgressDrawale.NUM_POINTS));
                mProgressDrawable.SetRotation(groupRotation);
            }
            base.ApplyTransformation(interpolatedTime, t);
        }
    }
}
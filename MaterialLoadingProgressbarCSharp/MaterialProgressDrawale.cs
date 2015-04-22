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
using Android.Graphics.Drawables;
using Android.Views.Animations;
using Android.Graphics;
using Android.Content.Res;
using Android.Util;

namespace MaterialLoadingProgressbarCSharp
{
    public class MaterialProgressDrawale : Drawable,IAnimatable,Animation.IAnimationListener
    {
        public const int LARGE = 0;
        public const int DEFAULT = 1;

        private IInterpolator LINEAR_INTERPOLATOR = new LinearInterpolator();

        private IInterpolator EASE_INTERPOLATOR = new AccelerateDecelerateInterpolator();

        private const int CIRCLE_DIAMETER = 40;
        private const float CENTER_RADIUS = 8.75f;
        private const float STROKE_WIDTH = 2.5f;

        private const int CIRCLE_DIAMETER_LARGE = 56;
        private const float CENTER_RADIUS_LARGE = 12.5f;
        private const float STROKE_WIDTH_LARGE = 3f;

        private const int ANIMATION_DURATION = 1000 * 80 / 60;

        public const float NUM_POINTS = 5f;

        private const int ARROW_WIDTH = 10;
        private const int ARROW_HEIGHT = 5;

        private const int ARROW_WIDTH_LARGE = 12;
        private const int ARROW_HEIGHT_LARGE = 6;
        public const float MAX_PROGRESS_ARC = 0.8f;
        private Color[] COLORS = new Color[] { Color.Black };

        private List<Animation> mAnimators = new List<Animation>();

        private Ring mRing;
        private Drawable.ICallback mCallback;
        private bool mFinishing;

        private float mRotation;
        private Resources mResources;
        private View mAnimExcutor;
        private Animation mAnimation;
        private float mRotationCount;
        private double mWidth;
        private double mHeight;

        public MaterialProgressDrawale(Context context, View animExcutor)
        {
            mCallback = new DefaultCallback(this);

            mAnimExcutor = animExcutor;
            mResources = context.Resources;

            mRing = new Ring(mCallback);
            mRing.SetColors(COLORS);

            UpdateSizes(DEFAULT);
            SetupAnimators();
        }

        public void SetSizeParameters(double progressCircleWidth, double progressCircleHeight,
            double centerRadius, double strokeWidth, float arrowWidth, float arrowHeight)
        {
            Ring ring = mRing;
            mWidth = progressCircleWidth;
            mHeight = progressCircleHeight;
            ring.StrokeWidth = (float)strokeWidth;
            ring.RingCenterRadius = centerRadius;
            ring.SetArrowDimensions(arrowWidth, arrowHeight);
            ring.SetInsets((int)mWidth, (int)mHeight);
        }

        public void UpdateSizes(int size)
        {
            DisplayMetrics metrics = mResources.DisplayMetrics;
            float screenDensity = metrics.Density;

            if (size == LARGE)
            {
                SetSizeParameters(CIRCLE_DIAMETER_LARGE * screenDensity, CIRCLE_DIAMETER_LARGE * screenDensity,
                    CENTER_RADIUS_LARGE * screenDensity, STROKE_WIDTH_LARGE * screenDensity, ARROW_WIDTH_LARGE * screenDensity, ARROW_HEIGHT_LARGE * screenDensity);
            }
            else
            {
                SetSizeParameters(CIRCLE_DIAMETER * screenDensity, CIRCLE_DIAMETER * screenDensity, CENTER_RADIUS * screenDensity, STROKE_WIDTH * screenDensity,
                    ARROW_WIDTH * screenDensity, ARROW_HEIGHT * screenDensity);
            }
        }

        public void ShowArrow(bool show)
        {
            mRing.SetShowArrow(show);
        }

        public void SetArrowScale(float scale)
        {
            mRing.SetArrowScale(scale);
        }

        public void SetStartEndTrim(float startAngle, float endAngle)
        {
            mRing.StartTrim = startAngle;
            mRing.EndTrim = endAngle;
        }

        public void SetProgressRotation(float rotation)
        {
            mRing.Rotation = rotation;
        }

        public void SetBackgrounColor(Color color)
        {
            mRing.BackgrounColor = color;
        }

        public void SetColorSchemeColors(params Color[] colors)
        {
            mRing.SetColors(colors);
            mRing.SetColorIndex(0);
        }

        public float RotationCount
        {
            get
            {
                return mRotationCount;
            }
        }

        public override int IntrinsicHeight
        {
            get
            {
                return (int)mHeight;
            }
        }

        public override int IntrinsicWidth
        {
            get
            {
                return (int)mWidth;
            }
        }

        public override void Draw(Canvas canvas)
        {
            Rect bounds = Bounds;
            int saveCount = canvas.Save();
            canvas.Rotate(mRotation, bounds.ExactCenterX(), bounds.ExactCenterY());
            mRing.Draw(canvas, bounds);
            canvas.RestoreToCount(saveCount);
        }

        public Ring Ring
        {
            get
            {
                return mRing;
            }
        }

        public bool Finishing
        {
            get
            {
                return mFinishing;
            }
        }

        public int Alpha
        {
            get
            {
                return mRing.Alpha;
            }
        }

        public override void SetAlpha(int alpha)
        {
            mRing.Alpha = alpha;
        }

        public override void SetColorFilter(ColorFilter cf)
        {
            mRing.SetColorFilter(cf);
        }

        public float GetRotation()
        {
            return mRotation;
        }

        public void SetRotation(float rotation)
        {
            mRotation = rotation;
            InvalidateSelf();
        }

        public override int Opacity
        {
            get { return (int)Format.Translucent; }
        }

        public void ApplyFinishTranslation(float interpolatedTime, Ring ring)
        {
            float targetRotaion = (float)(Math.Floor(ring.StartingStartTrim / MAX_PROGRESS_ARC) + 1f);
            float startTrim = ring.StartingStartTrim + (ring.StartingEndTrim - ring.StartingStartTrim) * interpolatedTime;
            ring.StartTrim = startTrim;
            float rotation = ring.GetStartingRotation() + ((targetRotaion - ring.GetStartingRotation()) * interpolatedTime);
            ring.Rotation = rotation;
        }

        private void SetupAnimators()
        {
            Ring ring = mRing;
            Animation animation = new DefaultAnimation(this);
            animation.RepeatCount = Animation.Infinite;
            animation.RepeatMode = RepeatMode.Restart;
            animation.Interpolator = LINEAR_INTERPOLATOR;
            animation.SetAnimationListener(this);
            mAnimation = animation;
        }

        #region IAnimatable

        public bool IsRunning
        {
            get 
            {
                List<Animation> animators = mAnimators;
                int n = animators.Count;
                for (int i = 0; i < n; i++)
                {
                    Animation animator = animators[i];
                    if (animator.HasStarted && !animator.HasEnded)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Start()
        {
            mAnimation.Reset();
            mRing.StoreOriginals();

            if (mRing.EndTrim != mRing.StartTrim)
            {
                mFinishing = true;
                mAnimation.Duration = ANIMATION_DURATION / 2;
                mAnimExcutor.StartAnimation(mAnimation);
            }
            else
            {
                mRing.SetColorIndex(0);
                mRing.ResetOriginals();
                mAnimation.Duration = ANIMATION_DURATION;
                mAnimExcutor.StartAnimation(mAnimation);
            }
        }

        public void Stop()
        {
            mAnimExcutor.ClearAnimation();
            SetRotation(0);
            mRing.SetShowArrow(false);
            mRing.SetColorIndex(0);
            mRing.ResetOriginals();
        }

        #endregion

        #region IAnimationListener

        public void OnAnimationEnd(Animation animation)
        {

        }

        public void OnAnimationRepeat(Animation animation)
        {
            Ring.StoreOriginals();
            Ring.GoToNextColor();
            Ring.StartTrim = Ring.EndTrim;
            if (mFinishing)
            {
                mFinishing = false;
                animation.Duration = ANIMATION_DURATION;
                Ring.SetShowArrow(false);
            }
            else
            {
                mRotationCount = (mRotationCount + 1) % (NUM_POINTS);
            }
        }

        public void OnAnimationStart(Animation animation)
        {
            mRotationCount = 0;
        }

        #endregion
    }
}
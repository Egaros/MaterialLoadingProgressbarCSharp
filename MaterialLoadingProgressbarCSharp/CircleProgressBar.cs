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
using Android.Graphics;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Content.Res;

namespace MaterialLoadingProgressbarCSharp
{
    public class CircleProgressBar : ImageView
    {
        private const Color KEY_SHADOW_COLOR = new Color(0x1E000000);
        private const Color FILL_SHADOW_COLOR = new Color(0x3D000000);

        private const float X_OFFSET = 0f;
        private const float Y_OFFSET = 1.75f;
        private const float SHADOW_RADIUS = 3.5f;
        private int SHADOW_ELEVATION = 4;

        private const Color DEFAULT_CIRCLE_BG_LIGHT = new Color(0xff,0xfa,0xfa,0xfa);
        private const int DEFAULT_CIRCLE_DIAMETER = 56;
        private const int STROKE_WIDTH_LARGE = 3;
        public const int DEFAULT_TEXT_SIZE = 9;

        private Animation.IAnimationListener mListener;
        private int mShadowRadius;
        private Color mBackGroundColor;
        private Color mProgressColor;
        private int mProgressStokeWidth;
        private int mArrowWidth;
        private int mArrowHeight;
        private int mProgress;
        private int mMax;
        private int mDiameter;
        private int mInnerRadius;
        private Paint mTextPaint;
        private Color mTextColor;
        private int mTextSize;
        private bool mIfDrawText;
        private bool mShowArrow;
        private MaterialProgressDrawale mProgressDrawable;
        private ShapeDrawable mBgCircle;
        private bool mCircleBackgroundEnabled;
        private Color[] mColors = new Color[] { Color.Black };

        public CircleProgressBar(Context context)
            : base(context)
        {
            Init(context, null, 0);
        }

        public CircleProgressBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init(context, attrs, 0);
        }

        public CircleProgressBar(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs, defStyleAttr);
        }

        private void Init(Android.Content.Context context, IAttributeSet attrs, int p)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircleProgressBar, p, 0);
            float density = context.Resources.DisplayMetrics.Density;

            mBackGroundColor = a.GetColor(Resource.Styleable.CircleProgressBar_mlpb_background_color, DEFAULT_CIRCLE_BG_LIGHT);
            mProgressColor = a.GetColor(Resource.Styleable.CircleProgressBar_mlpb_progress_color, DEFAULT_CIRCLE_BG_LIGHT);
            mInnerRadius = a.GetDimensionPixelOffset(Resource.Styleable.CircleProgressBar_mlpb_inner_radius, -1);
            mProgressStokeWidth = a.GetDimensionPixelOffset(Resource.Styleable.CircleProgressBar_mlpb_progress_stoke_width, (int)(STROKE_WIDTH_LARGE * density));
            mArrowWidth = a.GetDimensionPixelOffset(Resource.Styleable.CircleProgressBar_mlpb_arrow_width, -1);
            mArrowHeight = a.GetDimensionPixelOffset(Resource.Styleable.CircleProgressBar_mlpb_arrow_height, -1);
            mTextSize = a.GetDimensionPixelOffset(Resource.Styleable.CircleProgressBar_mlpb_progress_text_size, (int)(DEFAULT_TEXT_SIZE * density));
            mTextColor = a.GetColor(Resource.Styleable.CircleProgressBar_mlpb_progress_text_color, Color.Black);
            mShowArrow = a.GetBoolean(Resource.Styleable.CircleProgressBar_mlpb_show_arrow, false);
            mCircleBackgroundEnabled = a.GetBoolean(Resource.Styleable.CircleProgressBar_mlpb_enable_circle_background, true);

            mProgress = a.GetInt(Resource.Styleable.CircleProgressBar_mlpb_progress, 0);
            mMax = a.GetInt(Resource.Styleable.CircleProgressBar_mlpb_max, 100);
            int textVisible = a.GetInt(Resource.Styleable.CircleProgressBar_mlpb_progress_text_visibility, 1);
            if (textVisible != 1)
            {
                mIfDrawText = true;
            }

            mTextPaint = new Paint();
            mTextPaint.SetStyle(Paint.Style.Fill);
            mTextPaint.Color = mTextColor;
            mTextPaint.TextSize = mTextSize;
            mTextPaint.AntiAlias = true;
            a.Recycle();
            mProgressDrawable = new MaterialProgressDrawale(Context, this);
            base.SetImageDrawable(mProgressDrawable);
        }

        private bool ElevationSupported()
        {
            return Build.VERSION.SdkInt >= (BuildVersionCodes)21;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            if (!ElevationSupported())
            {
                SetMeasuredDimension(MeasuredWidth + mShadowRadius * 2, MeasuredHeight + mShadowRadius * 2);
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

        }
    }
}
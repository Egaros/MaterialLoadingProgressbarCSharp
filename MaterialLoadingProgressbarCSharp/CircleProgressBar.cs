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
using Android.Graphics.Drawables.Shapes;
using Android.Support.V4.View;

namespace MaterialLoadingProgressbarCSharp
{
    public class CircleProgressBar : ImageView
    {
        private Color KEY_SHADOW_COLOR = new Color(0x1E000000);
        

        private const float X_OFFSET = 0f;
        private const float Y_OFFSET = 1.75f;
        private const float SHADOW_RADIUS = 3.5f;
        private int SHADOW_ELEVATION = 4;

        private Color DEFAULT_CIRCLE_BG_LIGHT = new Color(0xff,0xfa,0xfa,0xfa);
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
            float density = Context.Resources.DisplayMetrics.Density;
            mDiameter = Math.Min(MeasuredWidth, MeasuredHeight);
            if (mDiameter <= 0)
            {
                mDiameter = (int)density * DEFAULT_CIRCLE_DIAMETER;
            }
            if (Background == null && mCircleBackgroundEnabled)
            {
                int shadowYOffset = (int)(density * Y_OFFSET);
                int shadowXOffset = (int)(density * X_OFFSET);
                mShadowRadius = (int)(density * SHADOW_RADIUS);

                if (ElevationSupported())
                {
                    mBgCircle = new ShapeDrawable(new OvalShape());
                    ViewCompat.SetElevation(this, SHADOW_ELEVATION * density);
                }
                else
                {
                    OvalShape oval = new OvalShadow(mShadowRadius, mDiameter, this);
                    mBgCircle = new ShapeDrawable(oval);
                    ViewCompat.SetLayerType(this, ViewCompat.LayerTypeSoftware, mBgCircle.Paint);
                    mBgCircle.Paint.SetShadowLayer(mShadowRadius, shadowXOffset, shadowYOffset,
                        KEY_SHADOW_COLOR);
                    int padding = (int)mShadowRadius;
                    SetPadding(padding, padding, padding, padding);
                }
                mBgCircle.Paint.Color = mBackGroundColor;
                SetBackgroundDrawable(mBgCircle);
            }
            mProgressDrawable.SetBackgrounColor(mBackGroundColor);
            mProgressDrawable.SetColorSchemeColors(mColors);
            mProgressDrawable.SetSizeParameters(mDiameter, mDiameter,
                mInnerRadius <= 0 ? (mDiameter - mProgressStokeWidth * 2) / 4 : mInnerRadius,
                mProgressStokeWidth,
                mArrowWidth < 0 ? mProgressStokeWidth * 5 : mArrowWidth,
                mArrowHeight < 0 ? mProgressStokeWidth * 2 : mArrowHeight);
            if (IsShowArrow())
            {
                mProgressDrawable.SetArrowScale(1f);
                mProgressDrawable.ShowArrow(true);
            }
            base.SetImageDrawable(null);
            base.SetImageDrawable(mProgressDrawable);
            mProgressDrawable.SetAlpha(255);
            mProgressDrawable.Start();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (mIfDrawText)
            {
                String text = String.Format("{0}%", mProgress);
                int x = Width / 2 - text.Length * mTextSize / 4;
                int y = Height / 2 + mTextSize / 4;
                canvas.DrawText(text, x, y, mTextPaint);
            }
        }

        public override void SetImageResource(int resId)
        {
            
        }

        private bool IsShowArrow()
        {
            return mShowArrow;
        }

        public void SetShowArrow(bool showArrow)
        {
            mShowArrow = showArrow;
        }

        public override void SetImageURI(Android.Net.Uri uri)
        {
            base.SetImageURI(uri);
        }

        public override void SetImageDrawable(Drawable drawable)
        {
            
        }

        public void SetAnimationListener(Animation.IAnimationListener listener)
        {
            mListener = listener;
        }

        protected override void OnAnimationStart()
        {
            base.OnAnimationStart();
            if (mListener != null)
            {
                mListener.OnAnimationStart(Animation);
            }
        }

        protected override void OnAnimationEnd()
        {
            base.OnAnimationEnd();
            if (mListener != null)
            {
                mListener.OnAnimationEnd(Animation);
            }
        }

        public void SetColorSchemeResources(params int[] colorResIds)
        {
            Color[] colorRes = new Color[colorResIds.Length];
            for (int i = 0; i < colorResIds.Length; i++)
            {
                colorRes[i] = Resources.GetColor(colorResIds[i]);
            }
            SetColorSchemeColors(colorRes);
        }

        private void SetColorSchemeColors(Color[] colorRes)
        {
            mColors = colorRes;
            if (mProgressDrawable != null)
            {
                mProgressDrawable.SetColorSchemeColors(colorRes);
            }
        }

        public void SetBackgroundColor(int colorRes)
        {
            if (Background is ShapeDrawable)
            {
                ((ShapeDrawable)Background).Paint.Color = Resources.GetColor(colorRes);
            }
        }
        public bool IsShowProgressText()
        {
            return mIfDrawText;
        }

        public void SetShowProgressText(bool ifDrawText)
        {
            mIfDrawText = ifDrawText;
        }

        public int Max
        {
            get
            {
                return mMax;
            }
            set
            {
                mMax = value;
            }
        }

        public int Progress
        {
            get
            {
                return mProgress;
            }
            set
            {
                if (Max > 0)
                {
                    mProgress = value;
                }
            }
        }

        public bool CircleBackgroundEnabled
        {
            get
            {
                return mCircleBackgroundEnabled;
            }
            set
            {
                mCircleBackgroundEnabled = value;
            }
        }

        public override ViewStates Visibility
        {
            get
            {
                return base.Visibility;
            }
            set
            {
                base.Visibility = value;
                if (mProgressDrawable != null)
                {
                    if (value != ViewStates.Visible)
                    {
                        mProgressDrawable.Stop();
                    }
                    mProgressDrawable.SetVisible(value == ViewStates.Visible, false);
                }
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            if (mProgressDrawable != null)
            {
                mProgressDrawable.Stop();
                mProgressDrawable.SetVisible(Visibility == ViewStates.Visible, false);
            }
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            if (mProgressDrawable != null)
            {
                mProgressDrawable.Stop();
                mProgressDrawable.SetVisible(false, false);
            }
        }
    }
}
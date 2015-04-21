using Android.Graphics;
using Android.Graphics.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialLoadingProgressbarCSharp
{
    public class Ring
    {
        private float ARROW_OFFSET_ANGLE = 0;

        private RectF mTempBounds = new RectF();
        private Paint mPaint = new Paint();
        private Paint mArrowPaint = new Paint();

        private Drawable.ICallback mCallback;
        private Paint mCirclePaint = new Paint();
        private float mStartTrim = 0.0f;
        private float mEndTrim = 0.0f;
        private float mRotation = 0.0f;
        private float mStrokeWidth = 5.0f;
        private float mStrokeInset = 2.5f;
        private Color[] mColors;

        private int mColorIndex;
        private float mStartingStartTrim;
        private float mStartingEndTrim;
        private float mStartingRotation;
        private bool mShowArrow;
        private Path mArrow;
        private float mArrowScale;
        private int mArrowWidth;
        private int mArrowHeight;

        public double RingCenterRadius { get; set; }
        public float Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                mRotation = value;
                InvalidateSelf();
            }
        }
        public float EndTrim
        {
            get
            {
                return mEndTrim;
            }
            set
            {
                mEndTrim = value;
                InvalidateSelf();
            }
        }
        public float StartingStartTrim
        {
            get
            {
                return mStartingStartTrim;
            }
        }
        public float StartingEndTrim
        {
            get
            {
                return mStartingEndTrim;
            }
        }
        public float StartTrim
        {
            get
            {
                return mStartTrim;
            }
            set
            {
                mStartTrim = value;
                InvalidateSelf();
            }
        }
        public float StrokeWidth
        {
            get
            {
                return mStrokeWidth;
            }
            set
            {
                mStrokeWidth = value;
                mPaint.StrokeWidth = value;
                InvalidateSelf();
            }
        }
        public int Alpha { get; set; }
        public Color BackgrounColor { get; set; }

        public Ring(Drawable.ICallback callback)
        {
            this.mCallback = callback;

            mPaint.StrokeCap = Paint.Cap.Square;
            mPaint.AntiAlias = true;
            mPaint.SetStyle(Paint.Style.Stroke);

            mArrowPaint.SetStyle(Paint.Style.Fill);
            mArrowPaint.AntiAlias = true;
        }

        public void SetArrowDimensions(float width, float height)
        {
            mArrowWidth = (int)width;
            mArrowHeight = (int)height;
        }

        public void Draw(Canvas c, Rect bounds)
        {
            RectF arcBounds = mTempBounds;
            arcBounds.Set(bounds);
            arcBounds.Inset(mStrokeInset, mStrokeInset);

            float startAngle = (mStartTrim + mRotation) * 360;
            float endAngle = (mEndTrim + mRotation) * 360;
            float sweepAngle = endAngle - startAngle;
            mPaint.Color = mColors[mColorIndex];
            c.DrawArc(arcBounds, startAngle, sweepAngle, false, mPaint);

            DrawTriangle(c, startAngle, sweepAngle, bounds);

            if (Alpha < 255)
            {
                mCirclePaint.Color = BackgrounColor;
                mCirclePaint.Alpha = 255 - Alpha;
                c.DrawCircle(bounds.ExactCenterX(), bounds.ExactCenterY(), bounds.Width() / 2, mCirclePaint);
            }
        }

        private void DrawTriangle(Canvas c, float startAngle, float sweepAngle, Rect bounds)
        {
            if (mShowArrow)
            {
                if (mArrow == null)
                {
                    mArrow = new Path();
                    mArrow.SetFillType(Path.FillType.EvenOdd);
                }
                else
                {
                    mArrow.Reset();
                }

                float x = (float)(RingCenterRadius * Math.Cos(0) + bounds.ExactCenterX());
                float y = (float)(RingCenterRadius * Math.Sin(0) + bounds.ExactCenterY());

                mArrow.MoveTo(0, 0);
                mArrow.LineTo((mArrowWidth) * mArrowScale, 0);
                mArrow.LineTo(((mArrowWidth) * mArrowScale / 2), (mArrowHeight * mArrowScale));
                mArrow.Offset(x - ((mArrowWidth) * mArrowScale / 2), y);
                mArrow.Close();

                mArrowPaint.Color = mColors[mColorIndex];
                c.Rotate(startAngle + (sweepAngle < 0 ? 0 : sweepAngle) - ARROW_OFFSET_ANGLE, bounds.ExactCenterX(), bounds.ExactCenterY());
                c.DrawPath(mArrow, mArrowPaint);
            }
        }

        public void SetColors(params Color[] colors)
        {
            mColors = colors;
            SetColorIndex(0);
        }

        public void SetColorIndex(int p)
        {
            mColorIndex = p;
        }

        public void GoToNextColor()
        {
            mColorIndex = (mColorIndex + 1) % (mColors.Length);
        }

        public void SetColorFilter(ColorFilter filter)
        {
            mPaint.SetColorFilter(filter);
            InvalidateSelf();
        }

        public void SetInsets(int width, int height)
        {
            float minEdge = (float)Math.Min(width, height);
            float insets = 0;
            if (RingCenterRadius <= 0 || minEdge < 0)
            {
                insets = (float)Math.Ceiling(mStrokeWidth / 2.0f);
            }
            else
            {
                insets = (float)(minEdge / 2.0f - RingCenterRadius);
            }
            mStrokeInset = insets;
        }

        public float GetInsets()
        {
            return mStrokeInset;
        }

        public void SetShowArrow(bool show)
        {
            if (mShowArrow != show)
            {
                mShowArrow = show;
                InvalidateSelf();
            }
        }

        public void SetArrowScale(float scale)
        {
            if (scale != mArrowScale)
            {
                mArrowScale = scale;
                InvalidateSelf();
            }
        }

        public float GetStartingRotation()
        {
            return mStartingRotation;
        }

        public void StoreOriginals()
        {
            mStartingStartTrim = mStartTrim;
            mStartingEndTrim = mEndTrim;
            mStartingRotation = mRotation;
        }

        public void ResetOriginals()
        {
            mStartingStartTrim = 0;
            mStartingEndTrim = 0;
            mStartingRotation = 0;
            StartTrim = 0;
            EndTrim = 0;
            Rotation = 0;
        }

        private void InvalidateSelf()
        {
            mCallback.InvalidateDrawable(null);
        }
    }
}

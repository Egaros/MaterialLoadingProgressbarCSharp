using Android.Graphics;
using Android.Graphics.Drawables.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialLoadingProgressbarCSharp
{
    public class OvalShadow : OvalShape
    {
        private Color FILL_SHADOW_COLOR = new Color(0x3D000000);

        private RadialGradient mRadialGradient;
        private int mShadowRadius;
        private Paint mShadowPaint;
        private int mCircleDiameter;
        private CircleProgressBar mProgressBar;

        public OvalShadow(int shadowRadius, int circleDiameter,CircleProgressBar progressBar)
            : base()
        {
            mProgressBar = progressBar;
            mShadowPaint = new Paint();
            mShadowRadius = shadowRadius;
            mCircleDiameter = circleDiameter;
            mRadialGradient = new RadialGradient(mCircleDiameter / 2, mCircleDiameter / 2,
                mShadowRadius, new int[] { FILL_SHADOW_COLOR, Color.Transparent },
                null, Shader.TileMode.Clamp);
            mShadowPaint.SetShader(mRadialGradient);
        }

        public new void Draw(Canvas canvas, Paint paint)
        {
            int viewWidth = mProgressBar.Width;
            int viewHeight = mProgressBar.Height;
            canvas.DrawCircle(viewWidth / 2, viewHeight / 2, (mCircleDiameter / 2 + mShadowRadius), mShadowPaint);
            canvas.DrawCircle(viewWidth / 2, viewHeight / 2, (mCircleDiameter / 2), paint);
        }
    }
}

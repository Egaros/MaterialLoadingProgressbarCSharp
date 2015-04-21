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

namespace MaterialLoadingProgressbarCSharp
{
    public class DefaultCallback : Java.Lang.Object, Drawable.ICallback
    {
        private MaterialProgressDrawale mProgressDrawable;

        public DefaultCallback(MaterialProgressDrawale progressDrawable)
        {
            this.mProgressDrawable = progressDrawable;
        }

        public void InvalidateDrawable(Drawable who)
        {
            mProgressDrawable.InvalidateSelf();
        }

        public void ScheduleDrawable(Drawable who, Java.Lang.IRunnable what, long when)
        {
            mProgressDrawable.ScheduleSelf(what, when);
        }

        public void UnscheduleDrawable(Drawable who, Java.Lang.IRunnable what)
        {
            mProgressDrawable.UnscheduleSelf(what);
        }
    }
}
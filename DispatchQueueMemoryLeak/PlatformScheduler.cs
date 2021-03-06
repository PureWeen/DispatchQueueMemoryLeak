﻿using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace XamarinDispatchScheduler
{
    //https://github.com/xamarin/Xamarin.Forms/blob/d3d59ee4f0b3098457e1debe8d7b03d0d0061a53/Xamarin.Forms.Platform.iOS/Forms.cs
    public class PlatformScheduler
    {
        public static IDisposable StartInterval(TimeSpan interval, Action callback)
        {
            NSTimer timer = NSTimer.CreateRepeatingTimer(interval, t =>
            {
                callback();
            });

            NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);            
            return Disposable.Create(() =>
            {
                timer.Invalidate();
            });
        }

        public static IDisposable StartTimer(TimeSpan interval, Action callback)
        {
            NSTimer timer = NSTimer.CreateTimer(interval, t =>
            {
                if (t.IsValid)
                {
                    callback();
                }
            });

            NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);
            return Disposable.Create(() =>
            {
                timer.Invalidate();
            });
        }

        public static void BeginInvokeOnMainThread(Action action)
        {
            NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
        }
    }
}

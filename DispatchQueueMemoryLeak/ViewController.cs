using CoreFoundation;
using System;
using UIKit;
using System.Timers;
using System.Diagnostics;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Foundation;

namespace DispatchQueueMemoryLeak
{
    public partial class ViewController : ReactiveUI.ReactiveViewController<ViewControllerViewModel>
    {

        public ViewController(IntPtr handle) : base(handle)
        {
            ViewModel = new ViewControllerViewModel();
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.OneWayBind(ViewModel, x => x.Tick, x => x.lblText.Text, _ => $"{System.GC.GetTotalMemory(false)}");


            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => GC.Collect());
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }


    public class ViewControllerViewModel : ReactiveObject
    {
        ObservableAsPropertyHelper<DateTimeOffset> _tick;
        public DateTimeOffset Tick => _tick.Value;
        public string SomeRandomProperty { get; set; }

        public ViewControllerViewModel()
        {


            _tick =
                this.WhenAnyValue(x => x.SomeRandomProperty)
                    .Select(_ => Observable.Interval(TimeSpan.FromSeconds(1)).Select(__ => DateTimeOffset.Now))
                    .Switch()
                    .ToProperty(this, x => x.Tick, scheduler: RxApp.MainThreadScheduler);
        }
    }
}
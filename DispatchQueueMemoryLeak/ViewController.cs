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
            this.OneWayBind(ViewModel, x => x.SomeRandomProperty, x => x.lblText.Text);


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

        string _property;
        public string SomeRandomProperty
        {
            get { return _property; }
            set { this.RaiseAndSetIfChanged(ref _property, value); }
        }

        public ViewControllerViewModel()
        {
            _tick =
                this.WhenAnyValue(x => x.SomeRandomProperty)
                    .Select(_ => Observable.Interval(TimeSpan.FromSeconds(1)).Select(__ => DateTimeOffset.Now))
                    .Switch()
                    .ToProperty(this, x => x.Tick, scheduler: RxApp.MainThreadScheduler);


            this.WhenAnyValue(x => x.Tick)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => SomeRandomProperty = $"{System.GC.GetTotalMemory(false)}");
        }
    }
}
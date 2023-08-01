using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Weather.Extensions;
using Weather.Models;
using Weather.ViewModels;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Weather.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsSityControl : ContentView
    {
        private static Dictionary<CustomerLocation, BoxView> _boxes = new Dictionary<CustomerLocation, BoxView>();
        private static WeatherPreferencesViewModel _weatherPreferencesViewModel;

        public ItemsSityControl()
        {
            InitializeComponent();
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> BoxViewChanged;

        public static readonly BindableProperty PathToCommandProperty = BindableProperty.Create(
            nameof(PathToCommand),
            typeof(object),
            typeof(ItemsSityControl),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnPathToCommandPropertyChanged);

        public static readonly BindableProperty ItemSityContextProperty = BindableProperty.Create(
            nameof(ItemSityContext),
            typeof(object),
            typeof(ItemsSityControl),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnItemSityContextChanged);

        public object ItemSityContext
        {
            get => (object)GetValue(ItemSityContextProperty);
            set => SetValue(ItemSityContextProperty, value);
        }

        public object PathToCommand
        {
            get => (object)GetValue(PathToCommandProperty);
            set => SetValue(PathToCommandProperty, value);
        }

        private static void OnItemSityContextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ItemsSityControl control) || !(newValue is CustomerLocation model))
                return;

            control.BindingContext ??= model;

            var box = control.FindByName("SideBox") as BoxView;

            if(box != null)
            {

                if (model.IsSelected)
                {
                    box.Scale = 0;
                    box.Opacity = 0;
                }
                else
                {
                    box.Scale = 1.5;
                    box.Opacity = 1;
                }

                _boxes.AddOrUpdate(model, box);
            }

            var frame = control.FindByName("GeneralFrame") as Frame;

            if (control.TrySetFrameCommands(frame, model))
                return;
            else
            {
                Task.Run(async () => 
                {
                    await Task.Delay(1000);

                    control.TrySetFrameCommands(frame, model);
                });
            }
        }

        private static void OnPathToCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ItemsSityControl control) || !(newValue is WeatherPreferencesViewModel model))
                return;

            _weatherPreferencesViewModel = model;
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var frame = sender as Frame;

            var box = frame.FindByName("SideBox") as BoxView;

            try
            {
                _boxes.ForEach(c =>
                {
                    if (c.Value.Scale != 1)
                    {
                        c.Value.FadeTo(1, 400, easing: Easing.SpringOut);
                        c.Value.ScaleTo(1.5, 400, easing: Easing.SpringOut);
                    }
                });

                box?.FadeTo(0, 300, easing: Easing.SpringIn);
                box?.ScaleTo(0, 400, easing: Easing.SpringIn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Touch animation failed.");
            }
        }

        public static void Reset(CustomerLocation addedCustomerLocation)
        {
            _boxes.ForEach(c =>
            {
                if (c.Key.Name != addedCustomerLocation.Name)
                {
                    c.Value.Scale = 1.5;
                    c.Value.Opacity = 1;
                }
                else
                {
                    c.Value.Scale = 0;
                    c.Value.Opacity = 0;
                }
            });
        }

        public static void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Reset((CustomerLocation)e.NewItems[0]);
            }
        }

        public bool TrySetFrameCommands(Frame frame, CustomerLocation model)
        {
            if (frame != null && _weatherPreferencesViewModel != null)
            {
                var gest = frame.GestureRecognizers[0];

                if (gest != null && gest is TapGestureRecognizer recognizer)
                {
                    recognizer.Command = _weatherPreferencesViewModel.ChoiceCity;
                    recognizer.CommandParameter = model;
                }

                Binding CommandBinding = new Binding();
                CommandBinding.Source = _weatherPreferencesViewModel;
                CommandBinding.Path = "Delete";
                CommandBinding.Mode = BindingMode.TwoWay;
                frame.SetBinding(TouchEffect.LongPressCommandProperty, CommandBinding);

                var parameterBinding = new Binding();
                parameterBinding.Source = model;

                frame.SetBinding(TouchEffect.LongPressCommandParameterProperty, parameterBinding);

                return true;
            }

            return false;
        }
    }
}
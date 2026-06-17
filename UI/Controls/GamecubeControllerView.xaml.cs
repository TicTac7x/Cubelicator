using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace Cubelicator
{
    public partial class GamecubeControllerView : UserControl
    {
        private readonly GamecubeController gamecubeController;
        private int _leftStickX;
        private int _leftStickY;
        private int _rightStickX;
        private int _rightStickY;
        private readonly float leftStickCanvasMultiplier = 0.3f;
        private readonly float rightStickCanvasMultiplier = 0.35f;
        private readonly float triggerCanvasMultiplier = 0.15f;

        public GamecubeControllerView(GamecubeController gamecubeController)
        {
            this.gamecubeController = gamecubeController;
            InitializeComponent();
            SetupEventListeners();
            SetControllerColor(ControllerColors.Map[ControllerColor.Indigo]);
        }

        public void SetControllerColor(string color)
        {
            var brush = App.StringToSolidColorBrush(color);

            DispatcherQueue.TryEnqueue(() =>
            {
                BaseCenter.Fill = brush;
                BaseLeftPalm.Fill = brush;
                BaseLeftPlate.Fill = brush;
                BaseRightPalm.Fill = brush;
                BaseRightPlate.Fill = brush;
            });
                
        }

        private void SetupEventListeners()
        {
            gamecubeController.OnButtonChanged += (button, pressed) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    switch (button)
                    {
                        case GamecubeControllerButton.A:
                            ButtonA.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonA);
                            break;
                        case GamecubeControllerButton.B:
                            ButtonB.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonB);
                            break;
                        case GamecubeControllerButton.X:
                            ButtonX.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonX);
                            break;
                        case GamecubeControllerButton.Y:
                            ButtonY.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonY);
                            break;
                        case GamecubeControllerButton.Z:
                            ButtonZ.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonZ);
                            break;
                        case GamecubeControllerButton.Start:
                            ButtonStart.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonStart);
                            break;
                        case GamecubeControllerButton.DPadUp:
                            DPadUp.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadDown:
                            DPadDown.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadLeft:
                            DPadLeft.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadRight:
                            DPadRight.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.LeftBumper:
                            LeftTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.Trigger);
                            break;
                        case GamecubeControllerButton.RightBumper:
                            RightTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.Trigger);
                            break;
                    }
                });
            };

            gamecubeController.OnStickChanged += (side, axis, value) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    switch (side)
                    {
                        case ControllerSide.Left:
                            if (axis == ControllerStickAxis.X)
                                _leftStickX = value;
                            else
                                _leftStickY = value;

                            Canvas.SetLeft(LeftStick, _leftStickX * leftStickCanvasMultiplier);
                            Canvas.SetTop(LeftStick, _leftStickY * -1 * leftStickCanvasMultiplier);
                            break;

                        case ControllerSide.Right:
                            if (axis == ControllerStickAxis.X)
                                _rightStickX = value;
                            else
                                _rightStickY = value;

                            Canvas.SetLeft(RightStick, _rightStickX * rightStickCanvasMultiplier);
                            Canvas.SetTop(RightStick, _rightStickY * rightStickCanvasMultiplier * -1);
                            break;
                    }
                });
            };

            gamecubeController.OnTriggerChanged += (side, value) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    switch (side)
                    {
                        case ControllerSide.Left:
                            Canvas.SetTop(LeftTrigger, value * triggerCanvasMultiplier);
                            break;
                        case ControllerSide.Right:
                            Canvas.SetTop(RightTrigger, value * triggerCanvasMultiplier);
                            break;
                    }
                });
                    
            };
        }

        private void OnClickButton(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element &&
                element.Tag is GamecubeControllerButton gamecubeControllerButton)
            {
                var flyout = new MenuFlyout();

                foreach (XboxControllerButton xboxControllerbutton in Enum.GetValues<XboxControllerButton>())
                {
                    var item = new MenuFlyoutItem
                    {
                        Text = xboxControllerbutton.ToString()
                    };

                    item.Click += (_, _) =>
                    {
                        gamecubeController.Profile.SetButton(gamecubeControllerButton, xboxControllerbutton);
                    };

                    flyout.Items.Add(item);
                }

                var centerBottomLocal = new Point(
                    element.ActualWidth / 2,
                    element.ActualHeight
                );

                var screenPoint = element
                    .TransformToVisual(null)
                    .TransformPoint(centerBottomLocal);

                flyout.ShowAt(
                    element,
                    new FlyoutShowOptions
                    {
                        Placement = FlyoutPlacementMode.Bottom,
                        Position = screenPoint
                    });
            }
        }
    }
}
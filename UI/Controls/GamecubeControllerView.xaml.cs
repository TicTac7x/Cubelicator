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
                Element_BaseCenter.Fill = brush;
                Element_BaseLeftPalm.Fill = brush;
                Element_BaseLeftPlate.Fill = brush;
                Element_BaseRightPalm.Fill = brush;
                Element_BaseRightPlate.Fill = brush;
            });
                
        }

        private void SetupEventListeners()
        {
            gamecubeController.Event_ButtonChanged += (button, pressed) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    switch (button)
                    {
                        case GamecubeControllerButton.A:
                            Element_ButtonA.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonA);
                            break;
                        case GamecubeControllerButton.B:
                            Element_ButtonB.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonB);
                            break;
                        case GamecubeControllerButton.X:
                            Element_ButtonX.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonX);
                            break;
                        case GamecubeControllerButton.Y:
                            Element_ButtonY.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonY);
                            break;
                        case GamecubeControllerButton.Z:
                            Element_ButtonZ.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonZ);
                            break;
                        case GamecubeControllerButton.Start:
                            Element_ButtonStart.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonStart);
                            break;
                        case GamecubeControllerButton.DPadUp:
                            Element_DPadUp.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadDown:
                            Element_DPadDown.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadLeft:
                            Element_DPadLeft.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.DPadRight:
                            Element_DPadRight.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.ButtonDPad);
                            break;
                        case GamecubeControllerButton.LeftBumper:
                            Element_LeftTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.Trigger);
                            break;
                        case GamecubeControllerButton.RightBumper:
                            Element_RightTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ControllerButtonPressed : ControllerInputColors.Trigger);
                            break;
                    }
                });
            };

            gamecubeController.Event_StickChanged += (side, axis, value) =>
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

                            Canvas.SetLeft(Element_LeftStick, _leftStickX * leftStickCanvasMultiplier);
                            Canvas.SetTop(Element_LeftStick, _leftStickY * -1 * leftStickCanvasMultiplier);
                            break;

                        case ControllerSide.Right:
                            if (axis == ControllerStickAxis.X)
                                _rightStickX = value;
                            else
                                _rightStickY = value;

                            Canvas.SetLeft(Element_RightStick, _rightStickX * rightStickCanvasMultiplier);
                            Canvas.SetTop(Element_RightStick, _rightStickY * rightStickCanvasMultiplier * -1);
                            break;
                    }
                });
            };

            gamecubeController.Event_TriggerChanged += (side, value) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    switch (side)
                    {
                        case ControllerSide.Left:
                            Canvas.SetTop(LeftTrigger, value * triggerCanvasMultiplier);
                            break;
                        case ControllerSide.Right:
                            Canvas.SetTop(Element_RightTrigger, value * triggerCanvasMultiplier);
                            break;
                    }
                });
                    
            };
        }
    }
}
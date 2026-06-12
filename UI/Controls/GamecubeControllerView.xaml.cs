using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
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
                        case ControllerButton.A:
                            ButtonA.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonA);
                            break;
                        case ControllerButton.B:
                            ButtonB.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonB);
                            break;
                        case ControllerButton.X:
                            ButtonX.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonX);
                            break;
                        case ControllerButton.Y:
                            ButtonY.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonY);
                            break;
                        case ControllerButton.Z:
                            ButtonZ.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonZ);
                            break;
                        case ControllerButton.Start:
                            ButtonStart.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonStart);
                            break;
                        case ControllerButton.DPadUp:
                            DPadUp.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonDPadUp);
                            break;
                        case ControllerButton.DPadDown:
                            DPadDown.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonDPadDown);
                            break;
                        case ControllerButton.DPadLeft:
                            DPadLeft.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonDPadLeft);
                            break;
                        case ControllerButton.DPadRight:
                            DPadRight.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorButtonDPadRight);
                            break;
                        case ControllerButton.LeftShoulder:
                            LeftTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorLeftTrigger);
                            break;
                        case ControllerButton.RightShoulder:
                            RightTriggerBase.Fill = App.StringToSolidColorBrush(pressed ? Colors.ButtonPressed : ColorRightTrigger);
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

        private string ColorButtonA => ControllerInputColors.ButtonA;
        private string ColorButtonB => ControllerInputColors.ButtonB;
        private string ColorButtonX => ControllerInputColors.ButtonX;
        private string ColorButtonY => ControllerInputColors.ButtonY;
        private string ColorButtonZ => ControllerInputColors.ButtonZ;
        private string ColorButtonStart => ControllerInputColors.ButtonStart;
        private string ColorButtonDPadUp => ControllerInputColors.ButtonDPadUp;
        private string ColorButtonDPadDown => ControllerInputColors.ButtonDPadDown;
        private string ColorButtonDPadLeft => ControllerInputColors.ButtonDPadLeft;
        private string ColorButtonDPadRight => ControllerInputColors.ButtonDPadRight;
        private string ColorLeftTrigger => ControllerInputColors.LeftTrigger;
        private string ColorRightTrigger => ControllerInputColors.RightTrigger;

    }
}
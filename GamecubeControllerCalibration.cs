namespace ConsoleApp1
{
    internal class GamecubeControllerCalibration
    {
        public int LeftStickXMin { get; set; }
        public int LeftStickXMax { get; set; }
        public int LeftStickXCenter { get; set; }

        public int LeftStickYMin { get; set; }
        public int LeftStickYMax { get; set; }
        public int LeftStickYCenter { get; set; }

        public int RightStickXMin { get; set; }
        public int RightStickXMax { get; set; }
        public int RightStickXCenter { get; set; }

        public int RightStickYMin { get; set; }
        public int RightStickYMax { get; set; }
        public int RightStickYCenter { get; set; }

        public int LeftTriggerMin { get; set; }
        public int LeftTriggerMax { get; set; }

        public int RightTriggerMin { get; set; }
        public int RightTriggerMax { get; set; }

        public GamecubeControllerCalibration(
            int leftStickXMin, int leftStickXMax, int leftStickXCenter,
            int leftStickYMin, int leftStickYMax, int leftStickYCenter,
            int rightStickXMin, int rightStickXMax, int rightStickXCenter,
            int rightStickYMin, int rightStickYMax, int rightStickYCenter,
            int leftTriggerMin, int leftTriggerMax,
            int rightTriggerMin, int rightTriggerMax)
        {
            LeftStickXMin = leftStickXMin;
            LeftStickXMax = leftStickXMax;
            LeftStickXCenter = leftStickXCenter;

            LeftStickYMin = leftStickYMin;
            LeftStickYMax = leftStickYMax;
            LeftStickYCenter = leftStickYCenter;

            RightStickXMin = rightStickXMin;
            RightStickXMax = rightStickXMax;
            RightStickXCenter = rightStickXCenter;

            RightStickYMin = rightStickYMin;
            RightStickYMax = rightStickYMax;
            RightStickYCenter = rightStickYCenter;

            LeftTriggerMin = leftTriggerMin;
            LeftTriggerMax = leftTriggerMax;

            RightTriggerMin = rightTriggerMin;
            RightTriggerMax = rightTriggerMax;
        }
    }
}
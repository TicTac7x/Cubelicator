namespace Cubelicator
{
    public class GamecubeControllerCalibration
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

        public GamecubeControllerCalibration() { }

        public GamecubeControllerCalibration(
            int LeftStickXMin, int LeftStickXMax, int LeftStickXCenter,
            int LeftStickYMin, int LeftStickYMax, int LeftStickYCenter,
            int RightStickXMin, int RightStickXMax, int RightStickXCenter,
            int RightStickYMin, int RightStickYMax, int RightStickYCenter,
            int LeftTriggerMin, int LeftTriggerMax,
            int RightTriggerMin, int RightTriggerMax)
        {
            this.LeftStickXMin = LeftStickXMin;
            this.LeftStickXMax = LeftStickXMax;
            this.LeftStickXCenter = LeftStickXCenter;

            this.LeftStickYMin = LeftStickYMin;
            this.LeftStickYMax = LeftStickYMax;
            this.LeftStickYCenter = LeftStickYCenter;

            this.RightStickXMin = RightStickXMin;
            this.RightStickXMax = RightStickXMax;
            this.RightStickXCenter = RightStickXCenter;

            this.RightStickYMin = RightStickYMin;
            this.RightStickYMax = RightStickYMax;
            this.RightStickYCenter = RightStickYCenter;

            this.LeftTriggerMin = LeftTriggerMin;
            this.LeftTriggerMax = LeftTriggerMax;

            this.RightTriggerMin = RightTriggerMin;
            this.RightTriggerMax = RightTriggerMax;
        }
    }
}
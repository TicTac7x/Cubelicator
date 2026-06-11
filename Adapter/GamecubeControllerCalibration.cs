namespace Cubelicator
{
    public class GamecubeControllerCalibration
    {
        public int leftStickXMin { get; set; }
        public int leftStickXMax { get; set; }
        public int leftStickXCenter { get; set; }

        public int leftStickYMin { get; set; }
        public int leftStickYMax { get; set; }
        public int leftStickYCenter { get; set; }

        public int rightStickXMin { get; set; }
        public int rightStickXMax { get; set; }
        public int rightStickXCenter { get; set; }

        public int rightStickYMin { get; set; }
        public int rightStickYMax { get; set; }
        public int rightStickYCenter { get; set; }

        public int leftTriggerMin { get; set; }
        public int leftTriggerMax { get; set; }

        public int rightTriggerMin { get; set; }
        public int rightTriggerMax { get; set; }

        public GamecubeControllerCalibration() { }

        public GamecubeControllerCalibration(
            int leftStickXMin, int leftStickXMax, int leftStickXCenter,
            int leftStickYMin, int leftStickYMax, int leftStickYCenter,
            int rightStickXMin, int rightStickXMax, int rightStickXCenter,
            int rightStickYMin, int rightStickYMax, int rightStickYCenter,
            int leftTriggerMin, int leftTriggerMax,
            int rightTriggerMin, int rightTriggerMax)
        {
            this.leftStickXMin = leftStickXMin;
            this.leftStickXMax = leftStickXMax;
            this.leftStickXCenter = leftStickXCenter;

            this.leftStickYMin = leftStickYMin;
            this.leftStickYMax = leftStickYMax;
            this.leftStickYCenter = leftStickYCenter;

            this.rightStickXMin = rightStickXMin;
            this.rightStickXMax = rightStickXMax;
            this.rightStickXCenter = rightStickXCenter;

            this.rightStickYMin = rightStickYMin;
            this.rightStickYMax = rightStickYMax;
            this.rightStickYCenter = rightStickYCenter;

            this.leftTriggerMin = leftTriggerMin;
            this.leftTriggerMax = leftTriggerMax;

            this.rightTriggerMin = rightTriggerMin;
            this.rightTriggerMax = rightTriggerMax;
        }
    }
}
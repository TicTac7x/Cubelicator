namespace Cubelicator
{
    public enum AppView
    {
        Dashboard,
        Controller1,
        Controller2,
        Controller3,
        Controller4,
        Calibration
    }

    public enum AdapterPort
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public abstract record GamecubeControllerInput;
    public record GamecubeControllerButtonInput(GamecubeControllerButton value) : GamecubeControllerInput;
    public record GamecubeControllerStickInput(GamecubeControllerStick value) : GamecubeControllerInput;
    public record GamecubeControllerTriggerInput(GamecubeControllerTrigger value) : GamecubeControllerInput;

    public enum GamecubeControllerButton
    {
        A,
        B,
        X,
        Y,
        Z,
        Start,

        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,

        LeftBumper,
        RightBumper
    }

    public enum GamecubeControllerTrigger
    {
        LeftTrigger,
        RightTrigger,
    }

    public enum GamecubeControllerStick
    {
        LeftStick,
        RightStick,
    }

    public enum Axis
    {
        X,
        Y
    }

    public abstract record XboxControllerInput;
    public record XboxControllerButtonInput(XboxControllerButton value) : XboxControllerInput;
    public record XboxControllerStickInput(XboxControllerStick value) : XboxControllerInput;
    public record XboxControllerTriggerInput(XboxControllerTrigger value) : XboxControllerInput;

    public enum XboxControllerButton
    {
        A,
        B,
        X,
        Y,

        Back,
        Start,
        Guide,
        Upload,

        LeftStickDown,
        RightStickDown,

        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,

        LeftBumper,
        RightBumper,

        None,
    }

    public enum XboxControllerTrigger
    {
        LeftTrigger,
        RightTrigger,

        None,
    }

    public enum XboxControllerStick
    {
        LeftStick,
        RightStick,

        None,
    }

    public enum ControllerColor
    {
        Indigo,
        JetBlack,
        SpiceOrange,
        Platinum,
        EmeraldBlue,
        White,
        StarlightGold,
        SymphonicGreen,
        LuigiGreen,
        MarioRed,
        WarioYellow,
        GundamChar
    }
}

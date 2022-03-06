//This module's imports.
using System;
using System.Drawing;
using System.Windows.Forms;

//This namespace contains the analog clock control.
namespace Analog_Clock.cs
{
    //This control contains the procedures for drawing an analog clock.
    public partial class AnalogClockControl : UserControl
    {
        private const int CLOCK_LINE_WIDTH = 2;              //Defines the width of the lines used to draw the clock.
        private const int CLOCK_SIZE = 120;                  //private const int s the clock face's diameter in pixels.
        private const int HAND_NUT_SIZE = 3;                 //private const int s the hands' nut size.
        private const int HOURS_TO_DEGREES = 30;             //private const int s the value used to convert hours to degrees.
        private const int LARGE_MARK_INTERVAL = 3;           //private const int s the interval between the large marks in hours.
        private const int MINUTES_TO_DEGREES = 6;            //private const int s the value used to convert minutes to degrees.
        private const double MINUTES_TO_FRACTION = 1 / 60;   //private const int s the value used to convert minutes to the fractional part of an hour.
        private const int SECONDS_TO_DEGREES = 6;            //private const int s the value used to convert seconds to degrees.
        private const int TWELVE_HOUR_ANGLE = -90;           //private const int s the angle for noon/midnight in degrees.

        private readonly int CLOCK_X = Convert.ToInt32((CLOCK_SIZE * 1.1));                        //Defines the clock face's horizontal center in pixels.
        private readonly int CLOCK_Y = Convert.ToInt32(CLOCK_SIZE * 1.1);                          //Defines the clock face's vertical center in pixels.
        private readonly double DEGREES_PER_RADIAN = 180 / Math.PI;                                //Defines the number of degrees per radian.
        private readonly static int HOUR_HAND_LENGTH = Convert.ToInt32(CLOCK_SIZE / 1.6);          //Defines the hour hand's length.
        private readonly static int LARGE_MARK_LENGTH = Convert.ToInt32(HOUR_HAND_LENGTH / 2.5);   //Defines the size of the markings used to mark every third hour.
        private readonly int MINUTE_HAND_LENGTH = Convert.ToInt32(HOUR_HAND_LENGTH * 1.5);         //Defines the minutes hand//s length.
        private readonly int SECOND_HAND_LENGTH = Convert.ToInt32(HOUR_HAND_LENGTH * 1.5);         //Defines the seconds hand//s length.
        private readonly int SMALL_MARK_LENGTH = Convert.ToInt32(LARGE_MARK_LENGTH / 2);           //Defines the size of the marking//s used to mark the hours.

        //This structure defines the time displayed by the clock.
        private struct TimeStr
        {
            public int Hour;     //Contains the hour.
            public int Minute;   //Contains the minute.
            public int Second;   //Contains the second.
        }

        public delegate void HandleErrorEvent(Exception ExceptionO);                                //Defines the error event handler's signature.
        public event HandleErrorEvent HandleError;                                                  //Defines the error event.
        private ToolTip ControlToolTip = new ToolTip();                                             //Contains this control's tooltip.
        private Timer AnalogClockTimer = new Timer() { Enabled = true, Interval = 1000};            //Contains the timer that powers the clock.
        private int HourHandX, HourHandY, MinuteHandX, MinuteHandY, SecondHandX, SecondHandY = 0;   //Contains the clock hands' positions.
        private TimeStr Time = new TimeStr();                                                       //Contains the current time.

        //This procedure initializes this control.
        public AnalogClockControl()
        {
            try
            {
                InitializeComponent();

                this.KeyDown += AnalogClockControl_KeyDown;
                this.MouseUp += AnalogClockControl_MouseUp;
                this.VisibleChanged += AnalogClockControl_VisibleChanged;
                AnalogClockTimer.Tick += AnalogClockTimer_Tick;

                DrawClock(CurrentTime(Advance: false));
                ControlToolTip.SetToolTip(this, "Click near the face's edge or use the plus key to set the time.");

                DateTime CurrentDate = DateTime.Now;
                Time = new TimeStr {Hour = CurrentDate.Hour, Minute = CurrentDate.Minute, Second = CurrentDate.Second};
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure handles the user's keystrokes.
        private void AnalogClockControl_KeyDown(Object sender, KeyEventArgs e)
        {
            try
            {
                TimeStr Time = CurrentTime();

                if (e.KeyCode == Keys.Add)
                {
                    if (e.Shift)
                    {
                        if (Time.Hour == 11)
                        {
                            Time.Hour = 0;
                        }
                        else
                        {
                            Time.Hour++;
                        };
                    }
                    else
                    {
                        if (Time.Minute == 59)
                        {
                            Time.Minute = 0;
                            if (Time.Hour == 11)
                            {   
                                Time.Hour = 0;
                            }
                            else
                            {
                                Time.Hour++;
                            };
                        }
                        else
                        {
                            Time.Minute++;
                        };
                    };
                    DrawClock(CurrentTime(NewHour: Time.Hour, NewMinute: Time.Minute));
                };
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure gives the command to change the time being displayed.
        private void AnalogClockControl_MouseUp(Object sender, MouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case MouseButtons.Middle:
                        CurrentTime(NewHour: Convert.ToInt32((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / HOURS_TO_DEGREES), NewMinute: 0);
                        break;
                    case MouseButtons.Left:
                        CurrentTime(NewHour: Convert.ToInt32((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / HOURS_TO_DEGREES));
                        break;
                    case MouseButtons.Right:
                        CurrentTime(NewMinute: Convert.ToInt32((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / MINUTES_TO_DEGREES));
                        break;
                }
                DrawClock(CurrentTime(Advance: false));
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure sets this control's size.
        private void AnalogClockControl_VisibleChanged(Object sender, EventArgs e)
        {
            try
            {
                this.Size = new Size(Convert.ToInt32(CLOCK_SIZE * 2.2), Convert.ToInt32(CLOCK_SIZE * 2.2));
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure gives the command to display an analog clock.
        private void AnalogClockTimer_Tick(Object sender, EventArgs e)
        {
            try
            {
              DrawClock(CurrentTime(Advance: true));
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure manages the time displayed by the clock.
        private TimeStr CurrentTime(bool Advance = false, int? NewHour = null, int? NewMinute = null, int? NewSecond = null) 
        {         
            try
            {
                if (Advance) 
                {
                    if (Time.Second == 59)
                    {
                        Time.Second = 0;
                        if (Time.Minute == 59)
                        {
                            Time.Minute = 0;
                            Time.Hour = (Time.Hour == 11) ? 0 : Time.Hour + 1;
                        }
                        else
                        {
                            Time.Minute++;
                        }
                    }
                    else
                    {
                        Time.Second++;
                    }
                }
                else
                {
                    if (NewHour != null) Time.Hour = NewHour.Value;
                    if (NewMinute != null) Time.Minute = NewMinute.Value;
                    if (NewSecond != null) Time.Second = NewSecond.Value;
                }

                return Time;
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }

            return new TimeStr(); 
        }

        //This procedure draws an analog clock displaying the specified time.
        private void DrawClock(TimeStr DisplayedTime)
        {
            try
            {
                Graphics Canvas = this.CreateGraphics();
                double HourAsRadians = 0.0;
                int MarkLength = 0;
                double MinuteAsRadians = 0.0;
                double SecondAsRadians = 0.0;

                Canvas.DrawLine(new Pen(this.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, HourHandX, HourHandY);
                Canvas.DrawLine(new Pen(this.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, MinuteHandX, MinuteHandY);
                Canvas.DrawLine(new Pen(this.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, SecondHandX, SecondHandY);

                for (int HourOnFace = 0; HourOnFace < 12; HourOnFace++)
                {
                    HourAsRadians = ((HourOnFace * HOURS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN;
                    MarkLength = (HourOnFace % LARGE_MARK_INTERVAL == 0) ? LARGE_MARK_LENGTH : SMALL_MARK_LENGTH;
                    Canvas.DrawLine(new Pen(Color.Yellow, CLOCK_LINE_WIDTH), Convert.ToInt32((Math.Cos(HourAsRadians) * CLOCK_SIZE) + CLOCK_X), Convert.ToInt32((Math.Sin(HourAsRadians) * CLOCK_SIZE) + CLOCK_Y), Convert.ToInt32((Math.Cos(HourAsRadians) * (CLOCK_SIZE - MarkLength)) + CLOCK_X), Convert.ToInt32((Math.Sin(HourAsRadians) * (CLOCK_SIZE - MarkLength)) + CLOCK_Y));
                };
                Canvas.DrawEllipse(new Pen(Color.Blue, CLOCK_LINE_WIDTH), 12, 12, CLOCK_SIZE * 2, CLOCK_SIZE * 2);


                HourAsRadians = (((DisplayedTime.Hour + DisplayedTime.Minute * MINUTES_TO_FRACTION) * HOURS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN;
                SecondAsRadians = ((DisplayedTime.Second * SECONDS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN;
                MinuteAsRadians = ((DisplayedTime.Minute * MINUTES_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN;

                HourHandX = Convert.ToInt32((Math.Cos(HourAsRadians) * HOUR_HAND_LENGTH) + CLOCK_X);
                HourHandY = Convert.ToInt32((Math.Sin(HourAsRadians) * HOUR_HAND_LENGTH) + CLOCK_Y);
                MinuteHandX = Convert.ToInt32((Math.Cos(MinuteAsRadians) * MINUTE_HAND_LENGTH) + CLOCK_X);
                MinuteHandY = Convert.ToInt32((Math.Sin(MinuteAsRadians) * MINUTE_HAND_LENGTH) + CLOCK_Y);
                SecondHandX = Convert.ToInt32((Math.Cos(SecondAsRadians) * SECOND_HAND_LENGTH) + CLOCK_X);
                SecondHandY = Convert.ToInt32((Math.Sin(SecondAsRadians) * SECOND_HAND_LENGTH) + CLOCK_Y);

                Canvas.DrawLine(new Pen(Color.Green, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, HourHandX, HourHandY);
                Canvas.DrawLine(new Pen(Color.Green, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, MinuteHandX, MinuteHandY);
                Canvas.DrawLine(new Pen(Color.Red, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, SecondHandX, SecondHandY);
                Canvas.DrawEllipse(Pens.White, Convert.ToInt32(CLOCK_X - (HAND_NUT_SIZE / 2)), Convert.ToInt32(CLOCK_Y - (HAND_NUT_SIZE / 2)), HAND_NUT_SIZE, HAND_NUT_SIZE);
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }

        }

        //This procedure converts the specified hand tip position to an angle relative to the clock's center.
        private int? GetAngle(int HandTipX, int HandTipY)
        {
            try
            {
                Point HandTip = new Point(HandTipX - CLOCK_X, HandTipY - CLOCK_Y);
                double Hypotenuse = Math.Sqrt(Math.Pow(HandTip.X, 2) + Math.Pow(HandTip.Y, 2));
                double Cosine = Math.Min(HandTip.X, Hypotenuse) / Math.Max(HandTip.X, Hypotenuse);
                double Sine = Math.Min(HandTip.Y, Hypotenuse) / Math.Max(HandTip.Y, Hypotenuse);
                double Angle = Math.Asin(Sine);
                
                if ((Sine <= 0 && Cosine <= 0) || (Sine >= 0 && Cosine <= 0))
                {
                    Angle = Math.PI - Angle;
                }
                else if (Sine <= 0 && Cosine >= 0)
                {
                    Angle = (Math.PI * 2) + Angle;
                }
                else
                {
                    Angle = 0;
                }

                return Convert.ToInt32(Angle * (180 / Math.PI));
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }

            return null;
       }
    }
}

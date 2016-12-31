////////////////////////////////////////////////////////////////////////////
//
//  This file is usually not part of Rpi.SenseHat.Demo
//
//  Copyright (c) 2016, Mattias Larsson
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in 
//  the Software without restriction, including without limitation the rights to use, 
//  copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
//  Software, and to permit persons to whom the Software is furnished to do so, 
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all 
//  copies or substantial portions of the Software.
//
//  This Demo was created on 2016-12-30 by Stefan-Xp

using System;
using System.Collections.Generic;
using Windows.UI;
using Emmellsoft.IoT.Rpi.SenseHat;
using Emmellsoft.IoT.Rpi.SenseHat.Fonts;
using Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor;
using System.Globalization;
using System.Threading;

namespace RPi.SenseHat.Demo.Demos
{
    /// <summary>
    /// Single color scroll-text.
    /// Click on the joystick to change drawing mode!
    /// </summary>
    public class BinaryClock : SenseHatDemo
	{
		private string lstrScreenText;                     // String used to update the ScreenText
        private static Color lcolorOff = Colors.Black;

        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="senseHat"></param>
        /// <param name="scrollText"></param>
		public BinaryClock(ISenseHat senseHat, Action<string> scrollText)
			: base(senseHat, scrollText)
		{
		}

        /// <summary>
        /// This updates a Binary Bit @[xPos, yPos]
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="value">Byte Value</param>
        /// <param name="checkedValue">Pattern to check against</param>
        /// <param name="lColor">If Bit should be set then True then set this Color</param>
        /// <param name="DefaultColor"> ... else use this Color</param>
        private void updateBinaryBit(byte xPos, byte yPos, byte value, byte checkedValue, Color lColor, Color DefaultColor)
        {
            if ((value & checkedValue) != 0)
            {
                SenseHat.Display.Screen[xPos, yPos] = lColor; // Draw the pixel.
            }
            else
            {
                SenseHat.Display.Screen[xPos, yPos] = DefaultColor; // Delete the pixel.
            }
        }

        /// <summary>
        /// This sub updates a row of the Display
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="value"></param>
        /// <param name="lColor"></param>
        private void updateBinaryDigit(byte xPos, byte value, Color lColor)
        {
            byte yPos = 0;
            Color DefaultColor = lcolorOff;

            updateBinaryBit(xPos, yPos, value, 1, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 2, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 4, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 8, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 16, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 32, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 64, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 128, lColor, DefaultColor);

            // If the Value is 0 use an "E"
            if (value == 0)
            {
                updateBinaryDigit(xPos, 15, lColor);
            }
        }

        /// <summary>
        /// This updates only the upper part of a Row
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="value"></param>
        /// <param name="lColor"></param>
        private void updateUpperBinaryDigit(byte xPos, byte value, Color lColor)
        {
            byte yPos = 4;
            Color DefaultColor = lcolorOff;

            updateBinaryBit(xPos, yPos, value, 1, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 2, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 4, lColor, DefaultColor); yPos++;
            updateBinaryBit(xPos, yPos, value, 8, lColor, DefaultColor); 


            // If the Value is 0 use an "E"
            if (value == 0)
            {
                updateUpperBinaryDigit(xPos, 15, lColor);
            }
        }

        /// <summary>
        /// This updates the binary Clock with myTime. Change Colors here, if you want to
        /// </summary>
        /// <param name="myTime"></param>
        private void updateBinaryClock(DateTime myTime)
        {
            byte yPos = 0;
            Color myColor = Colors.MidnightBlue;

            // overwrite unused Rows
            updateBinaryDigit(0, 15, lcolorOff);
            updateBinaryDigit(1, 0, lcolorOff);

            // Show Time
            updateBinaryDigit(2, (byte)Math.Floor((double)myTime.Hour / 10), myColor);
            updateBinaryDigit(3, (byte)(myTime.Hour % 10), myColor);
            myColor = Colors.SlateBlue;
            updateBinaryDigit(4, (byte)Math.Floor((double)myTime.Minute / 10), myColor);
            updateBinaryDigit(5, (byte)(myTime.Minute % 10), myColor);
            myColor = Colors.Aqua;
            updateBinaryDigit(6, (byte)Math.Floor((double)myTime.Second / 10), myColor);
            updateBinaryDigit(7, (byte)(myTime.Second % 10), myColor);

            // Show Date in upper Row
            yPos = 0;
            myColor = Colors.Yellow;
            updateUpperBinaryDigit(yPos, (byte)(Math.Floor((double)myTime.Year / 1000)%10), myColor); yPos++;
            updateUpperBinaryDigit(yPos, (byte)(Math.Floor((double)myTime.Year / 100)%10), myColor); yPos++;
            updateUpperBinaryDigit(yPos, (byte)Math.Floor(((double)myTime.Year / 10)%10), myColor); yPos++;
            updateUpperBinaryDigit(yPos, (byte)(myTime.Year % 10), myColor); yPos++;
            myColor = Colors.Orange;
            updateUpperBinaryDigit(yPos, (byte)Math.Floor((double)myTime.Month / 10), myColor); yPos++;
            updateUpperBinaryDigit(yPos, (byte)(myTime.Month % 10), myColor); yPos++;
            myColor = Colors.Red;
            updateUpperBinaryDigit(yPos, (byte)Math.Floor((double)myTime.Day / 10), myColor); yPos++;
            updateUpperBinaryDigit(yPos, (byte)(myTime.Day % 10), myColor);

            SenseHat.Display.Update(); // Update the physical display
        }

        public override void Run()
		{
			// Flip the Display (I wanted to watch it mirrored)
            SenseHat.Display.FlipHorizontal = true;
            SenseHat.Display.FlipVertical = true;

			while (isRunning)
			{
                // do that
                updateBinaryClock(DateTime.Now);

                // Change the Screentext to DateTime now.
                lstrScreenText = DateTime.Now.ToString("HH:mm:ss yyyy-MM-dd");

                // set the Text of the GUI
                SetScreenText(lstrScreenText);

                // Pause for a short while.
                Sleep(TimeSpan.FromMilliseconds(200));
			}
		}	
	}
}
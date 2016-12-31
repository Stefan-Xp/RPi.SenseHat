////////////////////////////////////////////////////////////////////////////
//
//  This file is part of Rpi.SenseHat.Demo
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
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//  PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
//  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace RPi.SenseHat.Demo
{
	

    public sealed partial class MainPage : Page
	{
        enum enumDemoNumber
        {
            DiscoLights,
            JoystickPixel,
            WriteTemperature,
            GravityBlob,
            Compass,
            SingleColorScrollText,
            MultiColorScrollText,
            SpriteAnimation,
            ReadAllSensors,
            ScrollingClock,
            BinaryClock,
            GammaTest
        }


        enumDemoNumber DemoNumber = enumDemoNumber.BinaryClock;
        DemoRunner myDemoRunner = new DemoRunner();


		public MainPage()
		{
            InitializeComponent();
            DemoNameText.Text = DemoNumber.ToString();
            if(DemoNumber != enumDemoNumber.ReadAllSensors)
            {
                ScreenText.FontSize = 72;
            }
            else
            {
                ScreenText.FontSize = 28;
            }
            myDemoRunner.Run(senseHat => DemoSelector.GetDemo((int) DemoNumber, senseHat, SetScreenText));

		}

		private async void SetScreenText(string text)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
				CoreDispatcherPriority.Normal,
				() =>
				{
					ScreenText.Text = text;

					// Feel free to add more UI stuff here! :-)
				});
		}

        private void btnNext_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(DemoNumber <= enumDemoNumber.GammaTest-1)
            {
                DemoNumber++;
            }
            else
            {
                DemoNumber = 0;
            }
            myDemoRunner.CancelTask();

            // Adjust FontSize if needed
            if (DemoNumber != enumDemoNumber.ReadAllSensors)
            {
                ScreenText.FontSize = 72;
            }
            else
            {
                ScreenText.FontSize = 28;
            }
            DemoNameText.Text = "Now showing Demo #" + (int) DemoNumber + " " + DemoNumber.ToString();
            myDemoRunner.Run(senseHat => DemoSelector.GetDemo((int)(DemoNumber), senseHat, SetScreenText));
        }
    }
}

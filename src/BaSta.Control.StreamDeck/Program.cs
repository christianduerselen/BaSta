using System;
using System.Drawing;
using OpenMacroBoard.Examples.Basketball24;
using OpenMacroBoard.SDK;

namespace BaSta.Control.StreamDeck;

public class Program
{
    private static Shotclock _shotClock;

    public static void Main()
    {
        _shotClock = new Shotclock();

        using (IMacroBoard deck = StreamDeckSharp.StreamDeck.OpenDevice())
        {
            deck.SetBrightness(100);

            deck.KeyStateChanged += (s, e) =>
            {
                if (!e.IsDown)
                    return;

                int pressedKeyId = e.Key;

                switch (pressedKeyId)
                {
                    case 0:
                        _shotClock.Set14();
                        break;
                    case 1:
                        _shotClock.Set24();
                        break;
                    case 3:
                        _shotClock.SetState(!_shotClock.IsRunning);
                        break;
                    case 4:
                        if (!_shotClock.IsRunning)
                            _shotClock.Set(_shotClock.Clock - TimeSpan.FromMilliseconds(100));
                        break;
                    case 5:
                        if (!_shotClock.IsRunning)
                            _shotClock.Set(_shotClock.Clock + TimeSpan.FromMilliseconds(100));
                        break;
                }

                DrawKeys(deck, _shotClock);
            };

            _shotClock.ClockChanged += (s, e) =>
            {
                DrawKeys(deck, _shotClock);
            };

            DrawKeys(deck, _shotClock);

            Console.WriteLine("Please press any key (on PC keyboard) to exit this example.");
            Console.ReadKey();

            _shotClock.SetState(false);
        }
    }

    private static void DrawKeys(IMacroBoard deck, Shotclock shotClock)
    {
        deck.SetKeyBitmap(0, GetText("14s", Brushes.White));
        deck.SetKeyBitmap(1, GetText("24s", Brushes.White));
        deck.SetKeyBitmap(4, GetText("-0.1", Brushes.White));
        deck.SetKeyBitmap(5, GetText("+0.1", Brushes.White));
        deck.SetKeyBitmap(2, GetText(shotClock.Clock.ToString("ss\\.f"), shotClock.IsRunning ? Brushes.Green : Brushes.Red));
        deck.SetKeyBitmap(3, IconLoader.LoadIconByName(shotClock.IsRunning ? "stop.png" : "play.png", true));
    }

    private static KeyBitmap GetText(string text, Brush brush)
    {
        Font font = new Font("Arial", 36, FontStyle.Bold);
        PointF origin = new PointF(0, 20);
        return KeyBitmap.Create.FromGraphics(100, 100, g =>
        {
            g.DrawString(text, font, brush, origin);
        });
    }
}
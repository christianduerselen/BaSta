using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace BaSta.StramatelSoundBoard;

public static class Program
{
    public static void Main()
    {
        while (true)
        {
            IList<DirectSoundDeviceInfo> devices = DirectSoundOut.Devices.ToList();

            Console.WriteLine("Select output device:");
            for (int i = 1; i < devices.Count; i++)
            {
                Console.WriteLine($"> [{i}] {devices[i].Description} ({devices[i - 1].ModuleName})");
            }

            Console.Write("Index: ");
            string index = Console.ReadLine();
            int idx = int.Parse(index);
            Guid deviceGuid = devices[idx - 1].Guid;

            Console.WriteLine();
            Console.WriteLine($"Starting playback on device \"{devices[idx-1].Description}\"!");

            var audioFile = new AudioFileReader(@"C:\Users\Christian\Desktop\GameHornDefault_2.wav");
            audioFile.Volume = 3f;
            
            using (var loopedAudioFile = new LoopStream(audioFile))
            using (var outputDevice = new DirectSoundOut(deviceGuid))
            {
                outputDevice.Init(loopedAudioFile);
                outputDevice.Play();
                
                Console.WriteLine("Press any key to stop playback...");
                Console.ReadKey(true);
                
                outputDevice.Stop();
            }
        }
    }
}
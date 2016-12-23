using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTunesWrapper;

namespace iTunesConsole
{
    class Program
    {
        static iTunes iTunesPlayer = iTunes.Instance;

        static private void PlayHandler(object sender, iTunesEventArgs args)
        {
            Console.WriteLine("PlayHandler - " + args.Title);
        }
        static private void StopHandler(object sender, iTunesEventArgs args)
        {
            Console.WriteLine("StopHandler - " + args.Title);
        }

        static void Main(string[] args)
        {
            iTunesPlayer.Play += PlayHandler;
            iTunesPlayer.Stop += StopHandler;

            string invokedVerb = "";
            object invokedVerbInstance;

            var options = new Options();
            if (args.Count() != 0)
            {
                if (!CommandLine.Parser.Default.ParseArguments(args, options,
                    (verb, subOptions) =>
                    {
                        invokedVerb = verb;
                        invokedVerbInstance = subOptions;
                    }))
                {
                    Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
                }

                if (invokedVerb == "play")
                {
                    Console.WriteLine("Verb: Play");
                    iTunesPlayer.PlayPause();
                }

                if (invokedVerb == "listen")
                {
                    Console.WriteLine("Verb: Listen - Press any key to cancel...");
                    Console.ReadKey();
                }

                if (invokedVerb == "playlist")
                {
                    Console.WriteLine("Verb: Playlist");
                    Playlist pl = iTunesPlayer.GetCurrentPlaylist();
                    var duration = new TimeSpan(0, 0, pl.Duration);
                    Console.WriteLine($"Playlist [{pl.Name}] [Tracks {pl.TrackCount:N0}] " + 
                        $"[Duration {duration.ToString(@"%d' Tage, 'hh\:mm\:ss' Stunden'")}] " +
                        $"[Size {pl.Size / (1024*1024*1024):F2} GBytes]");
                }
            }
        }
    }
}

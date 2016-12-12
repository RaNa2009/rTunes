using CommandLine;
using CommandLine.Text;


namespace iTunesConsole
{
    class PlaySubOptions { }
    class PlaylistSubOptions { }

    class Options
    {
        public Options()
        {
            PlayVerb = new PlaySubOptions();
        }

        [VerbOption("play", HelpText = "Play or pause iTunes")]
        public PlaySubOptions PlayVerb { get; set; }

        [VerbOption("playlist", HelpText = "Show current playlist")]
        public PlaylistSubOptions PlaylistVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("<<app title>>", "<<app version>>"),
                Copyright = new CopyrightInfo("<<app author>>", 2016),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: app -p Someone");
            help.AddOptions(this);
            return help;
        }
    }

}

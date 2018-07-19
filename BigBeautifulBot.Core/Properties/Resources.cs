namespace BigBeautifulBot.Properties
{
    using System;
    internal class Resources
    {
        internal static readonly string ErrorAccessDenied = ":warning: You do not have access to this command!";

        public static string FeedErrorTooFewArgs
        {
            get => @":warning: Please specify your feedee!";
        }
        public static string FeedErrorTooManyArgs
        { get => @":warning: You can't feed more than one person!"; }

        public static string MentionUnknown
        { get => @"Hi, yes! Did you mention me?"; }

        public static string SelfWeightComment100
        { get => @"T-the scale must be broken :fearful: there's no way I'm actually **{0}** kilograms!"; }

        public static string SelfWeightComment110
        { get => @"I guess that's why I can't fit into my binaries folder anymore :sweat:"; }

        public static string SelfWeightComment120
        { get => @"Oh jeez, I really need to stop accepting food from strangers :astonished:"; }

        public static string SelfWeightComment150
        { get => @"A-are you guys fattening me up?! I'm HUGE."; }

        public static string SelfWeightComment60
        { get => @"I suppose I'm about average, if perhaps a bit chubby"; }

        public static string SelfWeightComment70
        { get => @"I think I've put on a bit... Might be time to lay off the shortcake :flushed:"; }

        public static string SelfWeightComment80
        { get => @"I-I guess I'm a little on the pudgy side, but I mean, I have a stressful job, you know!"; }

        public static string SelfWeightComment90
        { get => @"W-well I'm not fat per se, I'm just a little extra woman than most are expecting..."; }

        public static string SelfWeightComment130
        { get => @"{0}kgs? I need to stop stuffing myself with delicious, digital foods :fearful:"; }

        public static string SelfWeightComment140
        { get => @"W-well it's not really my fault! I'm just always hungry :frowning:"; }

        public static string SelfWeightCommentGT150
        { get => @"It's you guys' fault I'm this fat; and now I just can't help myself!"; }

        public static string SelfWeightCommentLT60
        { get => @"I'm a little malnourished actually..."; }

        public static string UseCupcake
        { get => @"Ooh! Cupcake :grinning:"; }

        public static string UseCustard
        { get => @"Ooh my, I can really taste the calories :sweat_smile:"; }

        public static string UsePancake
        { get => @"These look yum, thanks~"; }

        public static string UseScales
        { get => @"I weigh **{0}kgs**."; }

        public static string UseShortcake
        { get => @"Yum! Thanks! Cake is my favorite :3"; }

        public static string UseUnknown
        { get => @"I um... Can't figure out what this is... Beep boop :sweat_smile:"; }

        public static string UseFoodUnknown
        { get => @"Oh, thanks, that was yummy :smile:"; }

        public static string UsePizza
        { get => @"Mm, that was good, thanks!"; }

        public static string MentionGreeting
        { get => @"Oh, hi there!"; }

        public static string MentionWhoIs
        { get => @"O-oh, I-I am~"; }

        public static string OverfedComment
        { get => @"G-guys \*huff\* I, I can't possibly eat \*huff\* this. I-I'm far too full... S-sorry~"; }

        public static string UseCherry
        { get => @"Damn, that was good."; }

        public static string AppetiteComment0
        { get => @"Mm, I feel perfectly full \*afue\*"; }

        public static string AppetiteComment1
        { get => @"\*gha\* It's nice, I don't feel so hungry anymore :relieved:"; }

        public static string AppetiteComment10
        { get => @"I'd kill for a {0} ~"; }

        public static string AppetiteComment2
        { get => @"Mm. I'm still a little bit hungry. Dessert ~ ?"; }

        public static string AppetiteComment3
        { get => @"\*uwah\* I'm going to need to loosen some buttons here..."; }

        public static string AppetiteComment4
        { get => @"I really need an extra large portion, I've worked up quite the appetite!"; }

        public static string AppetiteComment5
        { get => @"Hmm, I'm still pretty hungry. How about some {0}?"; }

        public static string AppetiteComment6
        { get => @"Thinking about :bacon: makes me :drooling_face:"; }

        public static string AppetiteComment7
        { get => @"Wanna order some food?"; }

        public static string AppetiteComment8
        { get => @"Feed me lots of sweets, 'kay? :heart:"; }

        public static string AppetiteComment9
        { get => @"Mm, I feel better, but I'm still really hungry  :tired_face:"; }

        public static string AppetiteCommentFull
        { get => @"I'm full but \*huff\* I could probably eat one more..."; }

        public static string AppetiteCommentHungry
        { get => @"Guuuuuuuuy's I'm really huungry :tired_face: feeed meeee ~ :two_hearts:"; }

        public static string FattyErrorNoResults
        { get => @":warning: No results found."; }

        public static string MentionGoodnight
        { get => @"Goodnight, {0}"; }

        public static string UseErrorTooFewArgs
        { get => @":warning: Please specify an item (emoji)."; }

        public static string WeightConvertErrorTooFewArgs
        { get => @":warning: Please enter a weight in {0}."; }

        public static string FattyErrorUnknownSource
        { get => @":warning: Unknown image source: {0}."; }

        public static string MentionBully
        { get => @";w; no bulli"; }

        public static string MentionThatsRight
        { get => @"Absolutely!"; }

        public static string RegexBully
        { get => @"you('?| a)re( a)?[^\.]*(fat|pork|glutton|chubby|pig)"; }

        public static string RegexGoodnight
        { get => @"goodnight|'night|お(やす|休)み"; }

        public static string RegexGreeting
        { get => @"hi|hello|hey"; }

        public static string RegexThatsRight
        { get => @"isn('?)t that right\?"; }

        public static string RegexWhoIs
        { get => @"who('?)s a (good|cute) (little )?(fatty|porker|porkchop|piggy)\?"; }

        public static string MentionFalseAlarm
        { get => @"Standing down :mute:"; }

        public static string RegexFalseAlarm
        { get => @"false alarm"; }

        public static string MentionLove
        { get => @"I love you too, {0}!"; }

        public static string RegexLove
        { get => @"I love (yo)?u"; }

        public static string UseLabombe
        { get => @"Oh my, that's a lot of butter :dizzy_face:"; }

        public const string SaveFatErrorTooFewArgs = @":warning: Invalid parameters.";
        public const string SaveFatErrorAccessDenied = @":warning: You do not have permission to save files to the BBB server.";
    }
}
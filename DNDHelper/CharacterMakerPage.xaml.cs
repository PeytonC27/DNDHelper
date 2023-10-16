using Org.W3c.Dom;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;

namespace DNDHelper;

public partial class CharacterMakerPage : ContentPage
{
    private List<Entry> textBoxes;
    private string intRegex = @"^\d+$";
    private string skillListRegex = @"^{([a-zA-Z]+\s*)+}$";
    private string attackListRegex = @"^{([a-zA-Z]+, [0-9]d((+|-)[0-9])?+}$";

    public CharacterMakerPage()
	{
        textBoxes = new List<Entry>();

        // main 6, AC, HP, SPD, SKLS/PROF, CR/LEVEL, ATKS
        string[] importantElements =
        {
            "Name",
            "STR",
            "DEX",
            "CON",
            "INT",
            "WIS",
            "CHA",
            "Hit Points",
            "Armor Class",
            "Speed",
            "Prof Bonus",
            "Skills",
            "Combat Rating",
            "Attacks",
        };

        InitializeComponent();

        // setting up the two vertical stack layouts
        VerticalStackLayout vsl1 = new VerticalStackLayout()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start
        };
        VerticalStackLayout vsl2 = new VerticalStackLayout()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.End
        };
        InternalLayout.Add(vsl1);
        InternalLayout.Add(vsl2);
        VerticalStackLayout currentLayout = vsl1;

        // create all the labels and entries for the GUI
        for (int i = 0; i < importantElements.Length; i++)
        {
            if (i == 7) currentLayout = vsl2;
            HorizontalStackLayout hsl = new HorizontalStackLayout()
            {
                Padding = 10,
                Spacing = 5,
                HorizontalOptions = LayoutOptions.Center,
            };
            Label l = new Label()
            {
                Text = importantElements[i],
                FontSize = 24,
                MinimumWidthRequest = 175,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            if (i < 7) { l.MinimumWidthRequest = 75; l.HorizontalTextAlignment = TextAlignment.Start; }

            Entry e = new Entry()
            {
                PlaceholderColor = new Color(255, 255, 255, 75),
                TextColor = Colors.White,
                BackgroundColor = Colors.SlateGray,
                WidthRequest = 300,
                HeightRequest = 0,
                FontSize = 12,
                IsSpellCheckEnabled = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            textBoxes.Add(e);

            // assigning the labels
            if (i == 0)
                e.Placeholder = "Enter a name";
            else if (i < 11 || i == 12)
                e.Placeholder = "Enter a number";
            else if (i == 11)
                e.Placeholder = "{SKILLNAME SKILLNAME...}";
            else
                e.Placeholder = "{NAME ROLL, NAME ROLL}";

            hsl.Add(l);
            hsl.Add(e);
            currentLayout.Add(hsl);
        }
    }

    /// <summary>
    /// When the button is pressed, it should evaluate whether the provided data can be parsed
    /// and saved into a DnD character sheet. If it can't, an error should be displayed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SubmitCharacterButton(object sender, EventArgs e)
    {
        string name = "";
        int[] stats = new int[10];
        string skills = "";
        string attacks = "";
        int index = 0;
        int statIndex = 0;

        // ensure the provided data was valid
        foreach (Entry entry in textBoxes)
        {
            // string check
            if (index == 0)
            {
                if (!Regex.IsMatch(entry.Text, @"[a-zA-Z0-9_]+"))
                    HandleSubmissionError("Name is not in a valid format.");
                stats[statIndex] = int.Parse(entry.Text);
            }
            // skill checks
            else if (index == 11)
            {
                if (!Regex.IsMatch(entry.Text, skillListRegex))
                    HandleSubmissionError("The skill list is not in the correct format.");
                skills = entry.Text;
            }
            // attack checks
            else if (index == 13)
            {
                if (!Regex.IsMatch(entry.Text, attackListRegex))
                    HandleSubmissionError("The attack list is not in the correct format.");
                attacks = entry.Text;
            }
            // number check
            else
            {
                if (!Regex.IsMatch(entry.Text, intRegex))
                    HandleSubmissionError("The attack list is not in the correct format.");
                name = entry.Text;
            }
            index++;
        }

        DNDCharacter character = new DNDCharacter(name, stats, skills, attacks);
    }

    /// <summary>
    /// Given an error message, change and display the given error
    /// </summary>
    /// <param name="s">The error message</param>
    private void HandleSubmissionError(string s)
    {

    }

    private class DNDCharacter
    {
        private string name;
        private int STR, DEX, CON, INT, WIS, CHA, HP, AC, SPD, PB;
        private string[] skills;
        private Attack[] attacks;

        public DNDCharacter(string name, int[] stats, string skills, string attacks)
        {
            this.name = name;

            STR = stats[0];
            DEX = stats[1];
            CON = stats[2];
            INT = stats[3];
            WIS = stats[4];
            CHA = stats[5];

            ParseSkillsAndAttacks(skills, attacks);
        }

        private void ParseSkillsAndAttacks(string skills, string attacks)
        {

        }

        private class Attack
        {
            private string name;
            private int dieCount;
            private int dieToRoll;
            private int modifier;

            public Attack(string name, string roll)
            {
                this.name = name;
                Parse(roll);
            }

            private void Parse(string roll)
            {
                dieCount = 0;
                dieToRoll = 0;
                modifier = 0;
            }
        }
    }
}
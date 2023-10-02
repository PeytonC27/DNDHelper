using System.IO.IsolatedStorage;

namespace DNDHelper;

public partial class CharacterMakerPage : ContentPage
{
	public CharacterMakerPage()
	{
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
            if (i < 11 || i == 12)
                e.Placeholder = "Enter a number";
            else if (i == 11)
                e.Placeholder = "{SKILLNAME SKILLNAME...}";
            else if (i == 13)
                e.Placeholder = "{NAME ROLL, NAME ROLL}";
            else
                e.Placeholder = "Enter a name";

            hsl.Add(l);
            hsl.Add(e);
            currentLayout.Add(hsl);
        }
        MainLayout.Add(new Button()
        {
          Text = "Submit Character",
          FontSize = 18,
          HorizontalOptions = LayoutOptions.Center,
        });
    }

    private class DNDCharacter
    {
        private string name;
        private int STR, DEX, CON, INT, WIS, CHA, HP, AC, SPD, PB;
        private string[] skills;
        private Attack[] attacks;


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
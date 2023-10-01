using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace DNDHelper
{
    public partial class MainPage : ContentPage
    {
        private List<Player> playerList;
        private bool combatStarted = false;
        private Queue<Player> queue;
        private readonly uint animationFadeTime = 3000;

        public MainPage()
        {
            playerList = new();
            queue = new();
            InitializeComponent();
        }

        /// <summary>
        /// Adds the text in the label as a player, throws an exception if not possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlayerButtonPressed(object sender, EventArgs e)
        {
            string text = InitiativeEntry.Text;
            string name = "";
            int initiative;
            try
            {
                if (text == "" || text == null)
                    throw new IllegalInputException();

                // parse data and check for errors
                string[] data = text.Split(" ");
                if (data.Length != 2)
                    throw new IllegalInputException();
                if (data[1] == "" || data[1] == null)
                    throw new IllegalInputException();
                if (!int.TryParse(data[1], out initiative))
                    throw new IllegalInputException();

                // find initiative (wildcards "+" and "-" means the initiative tracker should roll for the user
                if (data[1].StartsWith("+") || data[1].StartsWith("-"))
                {
                    name = data[0];
                    initiative = (new Random()).Next(1, 21) + int.Parse(data[1]);
                }
                else
                {
                    name = data[0];
                    initiative = int.Parse(data[1]);
                }


                // if player is already in list, throw error
                Player player = new Player(name, initiative);
                foreach (Player combatants in playerList)
                    if (combatants.Name == data[0])
                        throw new ExistingNameException();

                playerList.Add(player);

                // displaying combatant count
                if (playerList.Count == 1)
                    CurrentPlayerLabel.Text = playerList.Count + " Regsitered Combatant";
                else
                    CurrentPlayerLabel.Text = playerList.Count + " Registered Combatants";

                // removing current text in the box
                InitiativeEntry.Text = "";
                RunFadeAnimation(AddingPlayerMessageLabel, Color.FromRgb(0, 255, 0), "Successfully added " + data[0] + " to the initiative list.");
            }
            catch (IllegalInputException)
            {
                RunFadeAnimation(AddingPlayerMessageLabel, new Color(255, 0, 0), "Could not add \"" + text + "\" to the initiative list.");
            }
            catch (ExistingNameException)
            {
                RunFadeAnimation(AddingPlayerMessageLabel, new Color(255, 0, 0), name + " was already registered into combat.");
            }
        }

        /// <summary>
        /// When the combat needs to start or continue. This starts combat and grabs the next
        /// player in order of initiative. This wraps around after the last player is reached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnGetPlayerButtonPressed(object sender, EventArgs e)
        {
            if (playerList.Count == 0)
            {
                CurrentPlayerLabel.Opacity = 100;
                CurrentPlayerLabel.Text = "Can not start combat, not enough players added.";
                await CurrentPlayerLabel.FadeTo(0, animationFadeTime, Easing.Linear);
                return;
            }

            if (queue.Count == 0)
            {
                playerList.Sort();
                foreach(var player in playerList)
                {
                    queue.Enqueue(player);

                    InitiativeStackLayout.Add(new Label
                    {
                        Text = player.Name,
                        FontSize = 18,
                        FontFamily = "MouldyCheese",
                        TextColor = new Color(255, 255, 255),
                        HorizontalOptions = LayoutOptions.Center
                    });
                }
            }

            // Setting up the stack layout
            Player first = queue.Dequeue();
            InitiativeStackLayout.IsVisible = true;
            for (int i = 0; i < InitiativeStackLayout.Count; i++)
            {
                if (InitiativeStackLayout[i] is Label)
                {
                    var label = (Label)InitiativeStackLayout[i];
                    if (label.Text == first.Name)
                        label.TextColor = Color.FromArgb("#95DFED");
                    else
                        label.TextColor = new Color(255, 255, 255);
                }
            }
            CurrentPlayerLabel.Text = string.Format("{0}'s Turn", first.Name, first.Initiative);


            // adds player to the end of the queue
            queue.Enqueue(first);

            if (!combatStarted)
            {
                // hide the ability to add more players to combat
                AddPlayerButton.IsVisible = false;
                AddingPlayerMessageLabel.IsVisible = false;
                InitiativeEntry.IsVisible = false;
                EntryPromptLabel.IsVisible = false;

                // change button and title text
                BigDisplay.Text = "Combat Start";
                GetPlayerButton.Text = "Get Next Player";
            }

            combatStarted = true;
        }

        /// <summary>
        /// Resets the combat scenario by removing everything from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResetButtonPressed(object sender, EventArgs e)
        {
            // reset all values for the initiative counter
            combatStarted = false;
            playerList = new();
            queue = new();

            // no player name should be displayed
            CurrentPlayerLabel.Text = "0 Registered Combatants";
            InitiativeStackLayout.IsVisible = false;
            InitiativeStackLayout.Clear();

            // re-enable all entry labels, entries, and buttons
            EntryPromptLabel.IsVisible = true;
            InitiativeEntry.IsVisible = true;
            AddPlayerButton.IsVisible = true;
            AddingPlayerMessageLabel.IsVisible = true;

            // change the text
            BigDisplay.Text = "Initiative Tracker";
            GetPlayerButton.Text = "Start Combat";
        }

        /// <summary>
        /// Performs a fading animation on a visual element
        /// </summary>
        /// <param name="l">The element to apply the animation to</param>
        /// <param name="text">The text to be displayed on the visual element</param>
        private async void RunFadeAnimation(Label l, string text)
        {
            l.Opacity = 100;
            l.Text = text;
            await AddingPlayerMessageLabel.FadeTo(0, animationFadeTime, Easing.Linear);
        }

        /// <summary>
        /// Performs a fading animation on a visual element
        /// </summary>
        /// <param name="l">The element to apply the animation to</param>
        /// <param name="c">The color to apply to the visual element</param>
        /// <param name="text">The text to be displayed on the visual element</param>
        private async void RunFadeAnimation(Label l, Color c, string text)
        {
            l.Opacity = 100;
            l.Text = text;
            l.TextColor = c;
            await l.FadeTo(0, animationFadeTime, Easing.Linear);
        }
    }

    /// <summary>
    /// A class that holds the data of a player
    /// </summary>
    class Player : IComparable<Player>
    {
        public string Name { get; private set; }
        public int Initiative { get; private set; }

        public Player(string name, int initiative)
        {
            this.Name = name;
            this.Initiative = initiative;
        }

        public int CompareTo(Player other)
        {
            return other.Initiative - this.Initiative;
        }
    }

    class IllegalInputException : Exception { }
    class ExistingNameException : Exception { }
}
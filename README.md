# DND Helper
This is a small project dedicated to helping dungeon masters manage combat situations, NPCs, and stat tracking.

## Initiative Tracker
This part of the application is used to manage the initiatives of each player during combat. You can add players into a queue with their initiative and let this application do its work:

You add a new player by typing their name followed by the initiative they rolled:
```
John 20
Gus 10
Chester 16
```
You do not have to worry about the order, the tracker will automatically sort these at the start of combat.

If you don't want to roll manually, you can have the initiative tracker automatically roll for you by adding the initiative modifier instead. Examples below:
```
Goblin1 -3      // the program automatically rolls a random number from 1-20 minus 3
James +2        // same as above, but plus 2
Chester -10     // same as above, but minus 10
```


# I do not plan on working on this project anymore. A new version will be created in Java.

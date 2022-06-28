using System;


public static class DialogueVariables
{
    // this is a static class that keeps track of events in the bar, other scripts access this class for saving and triggering events
    // this probably could be a singleton or Scriptable object but it is honestly easier just to keep it as a static class.
    // tldr it works dont touch it
    public static bool dialogueIsPlaying;
    public static int drinkChoice;
    public static int dialogueOption;
    public static bool readyToDrink;
    public static int currentCharater;
    public static bool drinkConsumed;
    public static int amountDrunk;


}




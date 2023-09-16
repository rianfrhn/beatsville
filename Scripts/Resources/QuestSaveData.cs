using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public class QuestSaveData : Resource
{
    /*Activated:
     * [
     *  {
     * ]
     */

    public Array<Dictionary> activated = new Array<Dictionary>() { };
    public Array<string> completed = new Array<string>() { };
    public void AddActivated(string questId, Array<string>completed_goals)
    {
        activated.Add(new Dictionary()
        {
            {"id", questId },
            {"completed_goals", completed_goals}
        });
    }
    public void AddCompleted(string questId)
    {
        completed.Add(questId);
    }

}

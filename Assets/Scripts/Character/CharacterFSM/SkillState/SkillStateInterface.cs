using System.Collections.Generic;
using UnityEngine;

public abstract class SkillStateInterface : BehaviorStateInterface
{
    public SkillStateInterface(
        BehaviorEnumSet.State stateName, 
        GameObject characterRoot, 
        BehaviorEnumSet.AttackLevel attackLevel)
        : base(stateName, characterRoot, attackLevel)
    {
        CommandManager = characterRoot.GetComponent<CommandProcessor>();
    }

    protected CommandProcessor CommandManager;
    
    public abstract List<BehaviorEnumSet.Button> Command { get; protected set; }
}
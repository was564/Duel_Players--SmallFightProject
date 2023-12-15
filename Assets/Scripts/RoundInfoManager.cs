using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class RoundInfoManager
{
    private RoundData _roundData;

    public RoundInfoManager()
    {
        _roundData = new RoundData();
        _roundData.Round = new List<EntryInput>();
    }
    
    public void SaveRoundInfoToJson()
    {
        string json = JsonUtility.ToJson(_roundData, true);

        File.WriteAllText("./Assets/RoundJson/PreviousRound.json", json);

        Debug.Log("JSON 파일이 생성되었습니다.");
    }

    public void LoadPreviousRoundInfoFromJson(Queue<BehaviorEnumSet.Button> playerInputQueue, Queue<BehaviorEnumSet.Button> enemyInputQueue)
    {
        string json = File.ReadAllText("./Assets/RoundJson/PreviousRound.json");

        _roundData = JsonUtility.FromJson<RoundData>(json);

        int frame = -1;
        for (int index = 0; index < _roundData.Round.Count; index++)
        {
            EntryInput input = _roundData.Round[index];
            if (frame != input.Frame)
            {
                playerInputQueue.Enqueue(BehaviorEnumSet.Button.Null);
                enemyInputQueue.Enqueue(BehaviorEnumSet.Button.Null);
                frame = input.Frame;
            }
            if(input.TagName == "Enemy") enemyInputQueue.Enqueue((BehaviorEnumSet.Button)input.Button);
            else if(input.TagName == "Player") playerInputQueue.Enqueue((BehaviorEnumSet.Button)input.Button);
        }
    }

    public void EnqueueEntryInput(string tagName, BehaviorEnumSet.Button button, int frame)
    {
        _roundData.Round.Add(new EntryInput(tagName, button, frame));
    }

    public void Clear()
    {
        _roundData.Round.Clear();
        
        //GC.Collect();
    }
}

[System.Serializable]
public class RoundData
{
    public List<EntryInput> Round;
}

[System.Serializable]
public class EntryInput
{
    public EntryInput(string tagName, BehaviorEnumSet.Button button, int frame)
    {
        TagName = tagName;
        Button = (int)button;
        Frame = frame;
    }

    public string TagName;
    public int Frame;
    public int Button;
}
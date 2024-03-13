using System;
using System.Collections.Generic;
using System.IO;
using GameRound;
using UnityEngine;


public class RoundInfoManager
{
    public enum DataType
    {
        Input = 0,
        State,
        Size
    }

    public DataType Type { get; set; }
    
    private RoundDataByInput _roundDataByInput;
    private RoundDataByState _roundDataByState;

    public RoundInfoManager()
    {
        Type = DataType.State;
        _roundDataByInput = new RoundDataByInput();
        _roundDataByInput.Round = new List<EntryInput>();
        
        _roundDataByState = new RoundDataByState();
        _roundDataByState.Round = new List<EntryState>();
    }
    
    public void SaveRoundInfoToJson()
    {
        string json = JsonUtility.ToJson(_roundDataByInput, true);

        File.WriteAllText("./Assets/RoundJson/PreviousRound.json", json);

        Debug.Log("JSON 파일이 생성되었습니다.");
    }

    public void LoadPreviousRoundInfoFromJson(Queue<int> playerInputQueue, Queue<int> enemyInputQueue)
    {
        string json = File.ReadAllText("./Assets/RoundJson/PreviousRound.json");

        _roundDataByInput = JsonUtility.FromJson<RoundDataByInput>(json);

        switch (Type)
        {
            case DataType.Input:
                ConvertReplayDataByInput(playerInputQueue, enemyInputQueue);
                break;
            case DataType.State:
                ConvertReplayDataByState(playerInputQueue, enemyInputQueue);
                break;
        }
    }

    private void ConvertReplayDataByState(Queue<int> playerInputQueue, Queue<int> enemyInputQueue)
    {
        int frame = -1;
        for (int index = 0; index < _roundDataByState.Round.Count; index++)
        {
            EntryState input = _roundDataByState.Round[index];
            if (frame != input.Frame)
            {
                playerInputQueue.Enqueue((int)BehaviorEnumSet.Button.Null);
                enemyInputQueue.Enqueue((int)BehaviorEnumSet.Button.Null);
                frame = input.Frame;
            }
            if(input.CharacterIndex == (int)PlayersInRoundControlManager.CharacterIndex.Enemy) enemyInputQueue.Enqueue(input.State);
            else if(input.CharacterIndex == (int)PlayersInRoundControlManager.CharacterIndex.Player) playerInputQueue.Enqueue(input.State);
        }
    }
    
    private void ConvertReplayDataByInput(Queue<int> playerInputQueue, Queue<int> enemyInputQueue)
    {
        int frame = -1;
        for (int index = 0; index < _roundDataByInput.Round.Count; index++)
        {
            EntryInput input = _roundDataByInput.Round[index];
            if (frame != input.Frame)
            {
                playerInputQueue.Enqueue((int)BehaviorEnumSet.Button.Null);
                enemyInputQueue.Enqueue((int)BehaviorEnumSet.Button.Null);
                frame = input.Frame;
            }
            if(input.TagName == "Enemy") enemyInputQueue.Enqueue(input.Button);
            else if(input.TagName == "Player") playerInputQueue.Enqueue(input.Button);
        }
    }

    public void EnqueueEntryInput(string tagName, BehaviorEnumSet.Button button)
    {
        _roundDataByInput.Round.Add(new EntryInput(tagName, button, FrameManager.CurrentFrame));
    }

    public void EnqueueEntryState(PlayersInRoundControlManager.CharacterIndex index, BehaviorEnumSet.State state, Vector2 position, Vector2 velocity)
    {
        _roundDataByState.Round.Add(new EntryState(index, state, FrameManager.CurrentFrame, position, velocity));
    }

    public void Clear()
    {
        _roundDataByInput.Round.Clear();
        
        //GC.Collect();
    }
}

[System.Serializable]
public class RoundDataByState
{
    public List<EntryState> Round;
}

[System.Serializable]
public class EntryState
{
    public EntryState(PlayersInRoundControlManager.CharacterIndex index, BehaviorEnumSet.State state, int frame, Vector2 position, Vector2 velocity)
    {
        CharacterIndex = (int)index;
        State = (int)state;
        Frame = frame;
        PositionX = position.x;
        PositionY = position.y;
        VelocityX = velocity.x;
        VelocityY = velocity.y;
    }

    public int CharacterIndex;
    public int Frame;
    public int State;
    public float PositionX;
    public float PositionY;
    public float VelocityX;
    public float VelocityY;
}


[System.Serializable]
public class RoundDataByInput
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
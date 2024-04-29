using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character.PlayerMode;
using GameRound;
using Unity.VisualScripting;
using UnityEngine;


public class RoundRecordManager
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
    
    private Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> _players;

    public bool IsGameRecorded { get; set; } = true;
    
    public RoundRecordManager()
    {
        Type = DataType.State;
        _roundDataByInput = new RoundDataByInput();
        _roundDataByInput.Round = new List<EntryInput>();

        _roundDataByState = new RoundDataByState();
        _roundDataByState.Round = new List<EntryState>();
        _roundDataByState.Round.Add(
            new EntryState(PlayerCharacter.CharacterIndex.Player, (int)BehaviorEnumSet.State.StandingIdle,
                -1,1000, false, Vector3.zero, Vector2.zero));
        
        _players = new Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter>();
        PlayerCharacter[] players = GameObject.FindObjectsOfType<PlayerCharacter>();
        foreach (var player in players)
        {
            _players.Add(player.PlayerUniqueIndex, player);
            
            player.StateManager.RegisterNotifyObserver(AcceptStateChangeNotify);
            player.GetComponentInChildren<HitBox>().RegisterNotifyObserver(AcceptStateChangeNotify);
        }

        _playerAnimator = _players[PlayerCharacter.CharacterIndex.Player].GetComponent<CharacterAnimator>();
        _enemyAnimator = _players[PlayerCharacter.CharacterIndex.Enemy].GetComponent<CharacterAnimator>();
    }
    
    public void AcceptStateChangeNotify(PlayerCharacter.CharacterIndex characterIndex)
    {
        EnqueueEntryState(characterIndex, _players[characterIndex].StateManager.CurrentState.StateName, _players[characterIndex].Hp, 
            _players[characterIndex].GetPlayerMode() == PlayerModeManager.PlayerMode.FramePause,
            _players[characterIndex].transform.position, _players[characterIndex].RigidBody.velocity);
    }
    
    public void SaveRoundInfoToJson()
    {
        if (!IsGameRecorded) return;
        
        string json = JsonUtility.ToJson(_roundDataByState, true);

        File.WriteAllText("./Assets/RoundJson/PreviousRound.json", json);

        Debug.Log("JSON 파일이 생성되었습니다.");
        
        Clear();
    }

    public void LoadPreviousRoundInfoFromJsonToPlayers()
    {
        string json = File.ReadAllText("./Assets/RoundJson/PreviousRound.json");

        _roundDataByState = JsonUtility.FromJson<RoundDataByState>(json);
        
        Queue<EntryState> playerInputQueue = new Queue<EntryState>();
        Queue<EntryState> enemyInputQueue = new Queue<EntryState>();
        
        ConvertReplayDataByState(_roundDataByState, playerInputQueue, enemyInputQueue);
        
        _players[PlayerCharacter.CharacterIndex.Player].SetReplayingInputQueue(playerInputQueue);
        _players[PlayerCharacter.CharacterIndex.Enemy].SetReplayingInputQueue(enemyInputQueue);
        
        /*
        switch (Type)
        {
            
            case DataType.Input:
                ConvertReplayDataByInput(playerInputQueue, enemyInputQueue);
                break;
            case DataType.State:
                ConvertReplayDataByState(_roundDataByState, playerInputQueue, enemyInputQueue);
                break;
        }
        */
    }

    private void ConvertReplayDataByState(RoundDataByState data, Queue<EntryState> playerInputQueue, Queue<EntryState> enemyInputQueue)
    {
        for (int index = 0; index < _roundDataByState.Round.Count; index++)
        {
            EntryState input = _roundDataByState.Round[index];
            if(input.Frame == -1) continue;
            
            if (input.CharacterIndex == (int)PlayerCharacter.CharacterIndex.Enemy)
                enemyInputQueue.Enqueue(input);
            else if (input.CharacterIndex == (int)PlayerCharacter.CharacterIndex.Player)
                playerInputQueue.Enqueue(input);
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

            if (input.TagName == "Enemy") enemyInputQueue.Enqueue(input.Button);
            else if (input.TagName == "Player") playerInputQueue.Enqueue(input.Button);
        }
    }

    public void EnqueueEntryInput(string tagName, BehaviorEnumSet.Button button)
    {
        _roundDataByInput.Round.Add(new EntryInput(tagName, button, FrameManager.CurrentFrame));
    }

    private void EnqueueEntryState(PlayerCharacter.CharacterIndex index, BehaviorEnumSet.State state, int hp,
        bool frameStopped, Vector2 position, Vector2 velocity)
    {
        EntryState lastEntry = GetLastEntryState(index, FrameManager.CurrentFrame);
        if (lastEntry == null)
        {
            _roundDataByState.Round.Add(
                new EntryState(index, state, FrameManager.CurrentFrame, hp, frameStopped, position, velocity));
            return;
        }

        lastEntry.State = (int)state;
        lastEntry.PositionX = position.x;
        lastEntry.PositionY = position.y;
        lastEntry.FrameStopped = frameStopped;
        lastEntry.Hp = hp;

        if (lastEntry.VelocityX == 0f && lastEntry.VelocityY == 0f)
        {
            lastEntry.VelocityX = velocity.x;
            lastEntry.VelocityY = velocity.y;
        }
    }

    private EntryState GetLastEntryState(PlayerCharacter.CharacterIndex index, int frame)
    {
        for(int i=1;i<_roundDataByState.Round.Count;i++)
        {
            EntryState entry = _roundDataByState.Round[^i];
            if(entry.Frame < frame) return null;
            
            if (entry.CharacterIndex == (int)index) return entry;
        }

        return null;
    }
    
    /*
       public void EnqueueRoundInput(string tagName, BehaviorEnumSet.Button button, int frame)
       {
           _roundInfoManager.EnqueueEntryInput(tagName, button);
       }
    */

    private CharacterAnimator _playerAnimator;
    private CharacterAnimator _enemyAnimator;
    
    private BehaviorEnumSet.State _previousPlayerState = BehaviorEnumSet.State.Null;
    private BehaviorEnumSet.State _previousEnemyState = BehaviorEnumSet.State.Null;

    public void EnqueueCharacterInfoWithStateInRound(PlayerCharacter.CharacterIndex index, BehaviorEnumSet.State state,
        int frame, int hp, bool frameStopped, Vector2 position, Vector2 velocity)
    {
        bool doEnqueue = true;
        switch (index)
        {
            case PlayerCharacter.CharacterIndex.Player:
                if (state == _previousPlayerState && _playerAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) != 0.0f) doEnqueue = false;
                else _previousPlayerState = state;
                break;
            case PlayerCharacter.CharacterIndex.Enemy:
                if (state == _previousEnemyState && _enemyAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) != 0.0f) doEnqueue = false;
                else _previousEnemyState = state;
                break;
            default:
                return;
        }

        if (doEnqueue) EnqueueEntryState(index, state, hp, frameStopped, position, velocity);
    }

    public void Clear()
    {
        _roundDataByState.Round.Clear();
        _roundDataByState.Round.Add(
            new EntryState(PlayerCharacter.CharacterIndex.Player, (int)BehaviorEnumSet.State.StandingIdle,
                -1,1000, false, Vector3.zero, Vector2.zero));
    }
    
    /*
    private void RecordGameByState()
    {
        foreach (var player in _players)
        {
            player.Value
        }
    }
    */
    
    /*
    private Queue<int> _playersInputQueueForReplay = new Queue<int>();
    private Queue<int> _enemyInputQueueForReplay = new Queue<int>();
    private CharacterInputManager _playerInput;
    private CharacterInputManager _enemyInput;

    private void ReplayGameByInput()
    {
        bool endedPlayerInputEnqueue = false;
        bool endedEnemyInputEnqueue = false;
        while (true)
        {
            if (!endedEnemyInputEnqueue)
            {
                if (_enemyInputQueueForReplay.Count == 0)
                {
                    endedEnemyInputEnqueue = true;
                }
                else
                {
                    BehaviorEnumSet.Button enemyInput = (BehaviorEnumSet.Button)_enemyInputQueueForReplay.Dequeue();
                    if (enemyInput == BehaviorEnumSet.Button.Null)
                    {
                        endedEnemyInputEnqueue = true;
                    }
                    else _enemyInput.EnqueueInputQueue(enemyInput);
                }
            }
            if (!endedPlayerInputEnqueue)
            {
                if (_playersInputQueueForReplay.Count == 0)
                {
                    endedPlayerInputEnqueue = true;
                }
                else
                {
                    BehaviorEnumSet.Button playerInput = (BehaviorEnumSet.Button)_playersInputQueueForReplay.Dequeue();
                    if (playerInput == BehaviorEnumSet.Button.Null)
                    {
                        endedPlayerInputEnqueue = true;
                    }
                    else _playerInput.EnqueueInputQueue(playerInput);
                }
            }

            if (endedPlayerInputEnqueue && endedEnemyInputEnqueue)
                break;
        }
    }
    */
}

[System.Serializable]
public class RoundDataByState
{
    public List<EntryState> Round;
}

[System.Serializable]
public class EntryState
{
    public EntryState(PlayerCharacter.CharacterIndex index, BehaviorEnumSet.State state, int frame, int hp,
        bool frameStopped, Vector2 position, Vector2 velocity)
    {
        CharacterIndex = (int)index;
        State = (int)state;
        Frame = frame;
        Hp = hp;
        FrameStopped = frameStopped;
        PositionX = position.x;
        PositionY = position.y;
        VelocityX = velocity.x;
        VelocityY = velocity.y;
    }
    
    public int CharacterIndex;
    public int Frame;
    public int State;
    public int Hp;
    public bool FrameStopped;
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
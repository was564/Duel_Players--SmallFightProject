using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class RoundInfoManager
{
    private RoundInfo _roundData;

    public RoundInfoManager()
    {
        _roundData = new RoundInfo();
        _roundData.InfoElements = new List<RoundInfoElement>();
    }
    
    public void SaveRoundInfoToJson()
    {
        string json = JsonUtility.ToJson(_roundData, true);

        File.WriteAllText("./Assets/RoundJson/PreviousRound.json", json);

        Debug.Log("JSON 파일이 생성되었습니다.");
    }

    public void LoadPreviousRoundInfoFromJson(
        Queue<UnionInfo> playerInputQueue, Queue<UnionInfo> enemyInputQueue,
        Queue<UnionInfo> playerPositionQueue, Queue<UnionInfo> enemyPositionQueue,
        Transform playerTransform, Transform enemyTransform)
    {
        string json = File.ReadAllText("./Assets/RoundJson/PreviousRound.json");

        _roundData = JsonUtility.FromJson<RoundInfo>(json);

        int frame = -1;
        for (int index = 0; index < _roundData.InfoElements.Count; index++)
        {
            RoundInfoElement element = _roundData.InfoElements[index];
            
            switch (element.DataType)
            {
                case (int)DataType.Input:
                    UnionInfo input = element.Info;
                    if(input.TagName == "Enemy") enemyInputQueue.Enqueue(input);
                    else if(input.TagName == "Player") playerInputQueue.Enqueue(input);
                    break;
                case (int)DataType.Position:
                    UnionInfo position = element.Info;
                    if(position.TagName == "Enemy") enemyPositionQueue.Enqueue(position);
                    else if(position.TagName == "Player") playerPositionQueue.Enqueue(position);
                    break;
            }
        }
    }

    public void EnqueueInfoInput(string tagName, BehaviorEnumSet.Button button, int frame)
    {
        _roundData.InfoElements.Add(new RoundInfoElement(new UnionInfo(tagName, button, frame), (int)DataType.Input));
    }

    public void EnqueueInfoPosition(string tagName, float x, float y, int frame)
    {
        _roundData.InfoElements.Add(new RoundInfoElement(new UnionInfo(tagName, x, y, frame), (int)DataType.Position) );
    }

    public void Clear()
    {
        _roundData.InfoElements.Clear();
        
        //GC.Collect();
    }
}

public enum DataType
{
    Position = 0,
    Input,
    Size
}

[System.Serializable]
public class RoundInfo
{
    public List<RoundInfoElement> InfoElements;
}

[System.Serializable]
public class RoundInfoElement
{
    public RoundInfoElement(UnionInfo info, int dataType)
    {
        Info = info;
        DataType = dataType;
    }
    
    public UnionInfo Info;
    public int DataType;
}

[System.Serializable]
public class UnionInfo
{
    public UnionInfo(string tagName, BehaviorEnumSet.Button button, int frame)
    {
        TagName = tagName;
        Button = (int)button;
        Frame = frame;
    }

    public UnionInfo(string tagName, float x, float y, int frame)
    {
        TagName = tagName;
        X = x;
        Y = y;
        Frame = frame;
    }

    public string TagName;
    public int Frame;
    public int Button;
    public float X;
    public float Y;
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.CharacterFSM
{
    // Behavior State들을 하나의 묶음으로 관리해주는 클래스
    // 나중에 캐릭터별 State 추가할 때 사용하기
    public static class BehaviorStateSetManager 
    {
        public enum BehaviorStateSetIndex
        {
            Kohaku = 0,
            Size
        }
        
        public static BehaviorStateSetInterface GetStateSet(BehaviorStateSetIndex name, GameObject characterRoot)
        {
            GameObject wall = GameObject.FindGameObjectWithTag("Wall");
            switch (name)
            {
                default:
                case BehaviorStateSetIndex.Kohaku:
                    return new KohakuStateSet(characterRoot, wall);
            }
        }
    }
}
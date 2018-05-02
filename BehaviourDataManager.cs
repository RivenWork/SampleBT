using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class BehaviourDataManager
    {
        public static readonly BehaviourDataManager Instance = new BehaviourDataManager();
        Dictionary<uint, string> BeTreeDataDic;

        BehaviourDataManager()
        {
            BeTreeDataDic = new Dictionary<uint, string>();
        }

        public TextAsset GetBehaviourTreeData(uint id)
        {
            if (BeTreeDataDic.ContainsKey(id))
            {
                return Resources.Load(BeTreeDataDic[id]) as TextAsset;
            }
            return null;
        }
    }
}

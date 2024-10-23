using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
	//#region PlayerData
	//[Serializable]
	//public class PlayerData
	//{
	//	public int level;
	//	public int maxHp;
	//	public int attack;
	//	public int totalExp;
	//}

	//[Serializable]
	//public class PlayerDataLoader : ILoader<int, PlayerData>
	//{
	//	public List<PlayerData> stats = new List<PlayerData>();

	//	public Dictionary<int, PlayerData> MakeDict()
	//	{
	//		Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
	//		foreach (PlayerData stat in stats)
	//			dict.Add(stat.level, stat);
	//		return dict;
	//	}
	//}
	//#endregion


	#region PlayerData
	public class PlayerData
	{
		[XmlAttribute] //얻으려는 xml데이터안에 이러한 데이터들이 들어가있음을 알려줘야함 
		public int level;
		[XmlAttribute]
		public int maxHp;
        [XmlAttribute]
		public int attack;
		[XmlAttribute]
		public int totalExp;
	}

    [Serializable, XmlRoot("PlayerDatas")]//플레이어 데이터스 안에
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        [XmlElement("PlayerData")]//플레이어 데이터가 있다
        public List<PlayerData> stats = new List<PlayerData>(); //플레이어에 이러한 스택들을 내가 들고있을 예정이라고 선언

        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach (PlayerData stat in stats)
                dict.Add(stat.level, stat); //레벨을 키값으로 전체 플레이어 데이터를 딕셔너리에 저장
            return dict;//딕셔너리 반환
        }
    }
    #endregion
    #region MonsterData

    public class MonsterData
    {
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public string prefab;
        [XmlAttribute]
        public int level;
        [XmlAttribute]
        public int maxHp;
        [XmlAttribute]
        public int attack;
        [XmlAttribute]
        public float speed;
        // DropData
        // - 일정 확률로
        // - 어떤 아이템을 (보석, 스킬 가차, 골드, 고기)
        // - 몇 개 드랍할지?


    }
    #endregion

    #region SkillData
    public class SkillData
    {
        [XmlAttribute] //얻으려는 xml데이터안에 이러한 데이터들이 들어가있음을 알려줘야함 
        public int templateID;

        [XmlAttribute(AttributeName = "type")]
        //public string skillTypeStr;
        public Define.SkillType skillType = Define.SkillType.None;

        [XmlAttribute]
        public int nextID;
        public int prevID = 0; // TODO

        [XmlAttribute]
        public string prefab;

        // 아주 많이
        [XmlAttribute]
        public int damage;
    }

    [Serializable, XmlRoot("SkillDatas")]//플레이어 데이터스 안에
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        [XmlElement("SkillData")]//플레이어 데이터가 있다
        public List<SkillData> skills = new List<SkillData>(); //플레이어에 이러한 스택들을 내가 들고있을 예정이라고 선언

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.templateID, skill); //레벨을 키값으로 전체 플레이어 데이터를 딕셔너리에 저장
            return dict;//딕셔너리 반환
        }
    }
}
    #endregion
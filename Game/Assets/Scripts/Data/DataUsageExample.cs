using UnityEngine;
using System.Collections.Generic;
using EmberKeepers.Data;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 数据使用示例 - 展示如何使用导入的数据
    /// </summary>
    public class DataUsageExample : MonoBehaviour
    {
        private void Start()
        {
            // 确保DataManager已初始化
            if (DataManager.Instance == null)
            {
                Debug.LogError("DataManager未初始化！");
                return;
            }

            // 示例1: 获取特定英雄数据
            HeroData ignis = DataManager.Instance.GetHeroData("hero_fire_dps_ignis");
            if (ignis != null)
            {
                Debug.Log($"英雄: {ignis.heroNameCN} ({ignis.heroNameEN})");
                Debug.Log($"元素: {ignis.elementType}, 职业: {ignis.heroClass}");
                Debug.Log($"基础生命: {ignis.baseHealth}, 基础攻击: {ignis.baseAttackDamage}");
            }

            // 示例2: 获取所有火元素英雄
            List<HeroData> fireHeroes = DataManager.Instance.GetHeroesByElement(ElementType.Fire);
            Debug.Log($"火元素英雄数量: {fireHeroes.Count}");

            // 示例3: 获取所有武器
            List<EquipmentData> weapons = DataManager.Instance.GetEquipmentByType(EquipmentType.Weapon);
            Debug.Log($"武器数量: {weapons.Count}");

            // 示例4: 获取传奇装备
            List<EquipmentData> legendary = DataManager.Instance.GetEquipmentByRarity(Rarity.Legendary);
            Debug.Log($"传奇装备数量: {legendary.Count}");

            // 示例5: 获取火力手可用装备
            List<EquipmentData> dpsEquipment = DataManager.Instance.GetEquipmentForClass(HeroClass.DPS);
            Debug.Log($"火力手可用装备数量: {dpsEquipment.Count}");

            // 示例6: 获取怪物数据
            MonsterData whisper = DataManager.Instance.GetMonsterData("monster_whisper_lvl1");
            if (whisper != null)
            {
                Debug.Log($"怪物: {whisper.monsterName}");
                Debug.Log($"基础生命: {whisper.baseHealth}, 基础攻击: {whisper.baseAttackDamage}");
            }

            // 示例7: 获取所有精英怪
            List<MonsterData> elites = DataManager.Instance.GetEliteMonsters();
            Debug.Log($"精英怪数量: {elites.Count}");

            // 示例8: 获取关卡数据
            WaveDataEntry wave1 = DataManager.Instance.GetWaveData(1);
            if (wave1 != null)
            {
                Debug.Log($"第1波难度: {wave1.difficulty}");
                Debug.Log($"怪物生成数量: {wave1.monsterSpawns.Count}");
            }

            // 示例9: 检查数据是否加载完成
            if (DataManager.Instance.IsDataLoaded())
            {
                Debug.Log("所有数据已成功加载！");
            }
        }
    }
}


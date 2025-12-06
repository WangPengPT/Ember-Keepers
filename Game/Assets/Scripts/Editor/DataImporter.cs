using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using EmberKeepers.Data;
using EmberKeepers.Monsters;

namespace EmberKeepers.Editor
{
    /// <summary>
    /// 数据导入工具 - 从CSV文件导入游戏数据
    /// </summary>
    public class DataImporter : EditorWindow
    {
        private string csvPath = "";
        private string outputPath = "Assets/Resources/Data";

        [MenuItem("EmberKeepers/数据导入工具")]
        public static void ShowWindow()
        {
            GetWindow<DataImporter>("数据导入工具");
        }

        private void OnGUI()
        {
            GUILayout.Label("CSV数据导入工具", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("CSV文件路径（相对于项目根目录）:");
            csvPath = EditorGUILayout.TextField(csvPath);
            
            EditorGUILayout.LabelField("输出路径（相对于Assets）:");
            outputPath = EditorGUILayout.TextField(outputPath);

            EditorGUILayout.Space();

            if (GUILayout.Button("导入英雄数据"))
            {
                ImportHeroes();
            }

            if (GUILayout.Button("导入装备数据"))
            {
                ImportEquipment();
            }

            if (GUILayout.Button("导入怪物数据"))
            {
                ImportMonsters();
            }

            if (GUILayout.Button("导入关卡数据"))
            {
                ImportWaves();
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("一键导入所有数据", MessageType.Info);
            if (GUILayout.Button("一键导入所有数据"))
            {
                ImportAll();
            }
        }

        private void ImportAll()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string documentPath = Path.Combine(projectRoot, "Document");

            // 导入英雄
            string heroPath = Path.Combine(documentPath, "Heroes.csv");
            if (File.Exists(heroPath))
            {
                ImportHeroesFromFile(heroPath);
            }

            // 导入装备
            string equipmentPath = Path.Combine(documentPath, "Equipment.csv");
            if (File.Exists(equipmentPath))
            {
                ImportEquipmentFromFile(equipmentPath);
            }

            // 导入怪物
            string monsterPath = Path.Combine(documentPath, "Monsters.csv");
            if (File.Exists(monsterPath))
            {
                ImportMonstersFromFile(monsterPath);
            }

            // 导入关卡
            string wavePath = Path.Combine(documentPath, "Waves.csv");
            if (File.Exists(wavePath))
            {
                ImportWavesFromFile(wavePath);
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("导入完成", "所有数据已成功导入！", "确定");
        }

        private void ImportHeroes()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string filePath = Path.Combine(projectRoot, "Document", "Heroes.csv");
            ImportHeroesFromFile(filePath);
        }

        private void ImportEquipment()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string filePath = Path.Combine(projectRoot, "Document", "Equipment.csv");
            ImportEquipmentFromFile(filePath);
        }

        private void ImportMonsters()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string filePath = Path.Combine(projectRoot, "Document", "Monsters.csv");
            ImportMonstersFromFile(filePath);
        }

        private void ImportWaves()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string filePath = Path.Combine(projectRoot, "Document", "Waves.csv");
            ImportWavesFromFile(filePath);
        }

        private void ImportHeroesFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("错误", $"文件不存在: {filePath}", "确定");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                EditorUtility.DisplayDialog("错误", "CSV文件格式错误", "确定");
                return;
            }

            string[] headers = ParseCSVLine(lines[0]);
            string outputDir = Path.Combine(Application.dataPath, outputPath, "Heroes");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int imported = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = ParseCSVLine(lines[i]);
                if (values.Length < headers.Length) continue;

                HeroData hero = ScriptableObject.CreateInstance<HeroData>();
                
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    SetHeroField(hero, header, value);
                }

                string assetPath = Path.Combine(outputPath, "Heroes", $"{hero.heroId}.asset").Replace("\\", "/");
                AssetDatabase.CreateAsset(hero, assetPath);
                imported++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"成功导入 {imported} 个英雄数据");
        }

        private void ImportEquipmentFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("错误", $"文件不存在: {filePath}", "确定");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                EditorUtility.DisplayDialog("错误", "CSV文件格式错误", "确定");
                return;
            }

            string[] headers = ParseCSVLine(lines[0]);
            string outputDir = Path.Combine(Application.dataPath, outputPath, "Equipment");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int imported = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = ParseCSVLine(lines[i]);
                if (values.Length < headers.Length) continue;

                EquipmentData equipment = ScriptableObject.CreateInstance<EquipmentData>();
                
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    SetEquipmentField(equipment, header, value);
                }

                string assetPath = Path.Combine(outputPath, "Equipment", $"{equipment.equipmentId}.asset").Replace("\\", "/");
                AssetDatabase.CreateAsset(equipment, assetPath);
                imported++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"成功导入 {imported} 个装备数据");
        }

        private void ImportMonstersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("错误", $"文件不存在: {filePath}", "确定");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                EditorUtility.DisplayDialog("错误", "CSV文件格式错误", "确定");
                return;
            }

            string[] headers = ParseCSVLine(lines[0]);
            string outputDir = Path.Combine(Application.dataPath, outputPath, "Monsters");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int imported = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = ParseCSVLine(lines[i]);
                if (values.Length < headers.Length) continue;

                MonsterData monster = ScriptableObject.CreateInstance<MonsterData>();
                
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    SetMonsterField(monster, header, value);
                }

                string assetPath = Path.Combine(outputPath, "Monsters", $"{monster.monsterId}.asset").Replace("\\", "/");
                AssetDatabase.CreateAsset(monster, assetPath);
                imported++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"成功导入 {imported} 个怪物数据");
        }

        private void ImportWavesFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("错误", $"文件不存在: {filePath}", "确定");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                EditorUtility.DisplayDialog("错误", "CSV文件格式错误", "确定");
                return;
            }

            string outputDir = Path.Combine(Application.dataPath, outputPath, "Waves");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            WaveDataAsset waveAsset = ScriptableObject.CreateInstance<WaveDataAsset>();
            waveAsset.waveEntries = new List<WaveDataEntry>();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = ParseCSVLine(lines[i]);
                if (values.Length < 5) continue;

                WaveDataEntry entry = new WaveDataEntry();
                entry.waveNumber = int.Parse(values[0]);
                entry.difficulty = float.Parse(values[1]);
                entry.hasBoss = bool.Parse(values[2]);
                entry.bossId = values[3];
                entry.monsterSpawns = ParseMonsterSpawns(values[4]);

                waveAsset.waveEntries.Add(entry);
            }

            string assetPath = Path.Combine(outputPath, "Waves", "MainWaves.asset").Replace("\\", "/");
            AssetDatabase.CreateAsset(waveAsset, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"成功导入 {waveAsset.waveEntries.Count} 个关卡数据");
        }

        private List<MonsterSpawnData> ParseMonsterSpawns(string spawnString)
        {
            List<MonsterSpawnData> spawns = new List<MonsterSpawnData>();
            
            if (string.IsNullOrEmpty(spawnString)) return spawns;

            string[] spawnEntries = spawnString.Split(',');
            foreach (string entry in spawnEntries)
            {
                string[] parts = entry.Split(':');
                if (parts.Length >= 4)
                {
                    MonsterSpawnData spawn = new MonsterSpawnData();
                    spawn.monsterId = parts[0];
                    spawn.count = int.Parse(parts[1]);
                    spawn.level = int.Parse(parts[2]);
                    spawn.spawnDelay = float.Parse(parts[3]);
                    spawn.spawnInterval = parts.Length > 4 ? float.Parse(parts[4]) : 0.5f;
                    spawns.Add(spawn);
                }
            }

            return spawns;
        }

        private string[] ParseCSVLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string current = "";

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            result.Add(current);
            return result.ToArray();
        }

        private void SetHeroField(HeroData hero, string fieldName, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            switch (fieldName)
            {
                case "heroId": hero.heroId = value; break;
                case "heroNameCN": hero.heroNameCN = value; break;
                case "heroNameEN": hero.heroNameEN = value; break;
                case "elementType": hero.elementType = ParseEnum<ElementType>(value); break;
                case "heroClass": hero.heroClass = ParseEnum<HeroClass>(value); break;
                case "baseStrength": hero.baseStrength = int.Parse(value); break;
                case "baseAgility": hero.baseAgility = int.Parse(value); break;
                case "baseIntelligence": hero.baseIntelligence = int.Parse(value); break;
                case "baseElementMastery": hero.baseElementMastery = int.Parse(value); break;
                case "baseHealth": hero.baseHealth = float.Parse(value); break;
                case "baseAttackDamage": hero.baseAttackDamage = float.Parse(value); break;
                case "baseAttackSpeed": hero.baseAttackSpeed = float.Parse(value); break;
                case "baseMoveSpeed": hero.baseMoveSpeed = float.Parse(value); break;
                case "basePhysicalDefense": hero.basePhysicalDefense = float.Parse(value); break;
                case "baseElementResistance": hero.baseElementResistance = float.Parse(value); break;
                case "baseMaxEnergy": hero.baseMaxEnergy = float.Parse(value); break;
                case "baseEnergyRegenRate": hero.baseEnergyRegenRate = float.Parse(value); break;
                case "skillId": hero.skillId = value; break;
                case "skillName": hero.skillName = value; break;
                case "skillCooldown": hero.skillCooldown = float.Parse(value); break;
                case "skillEnergyCost": hero.skillEnergyCost = float.Parse(value); break;
                case "skillDescription": hero.skillDescription = value; break;
                case "aiLogicType": hero.aiLogicType = value; break;
            }
        }

        private void SetEquipmentField(EquipmentData equipment, string fieldName, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            switch (fieldName)
            {
                case "equipmentId": equipment.equipmentId = value; break;
                case "equipmentName": equipment.equipmentName = value; break;
                case "rarity": equipment.rarity = ParseEnum<Rarity>(value); break;
                case "equipmentType": equipment.equipmentType = ParseEnum<EquipmentType>(value); break;
                case "requiredClass": 
                    if (value == "None" || string.IsNullOrEmpty(value))
                        equipment.requiredClass = (HeroClass)(-1); // 使用-1表示全职业
                    else
                        equipment.requiredClass = ParseEnum<HeroClass>(value); 
                    break;
                case "attackDamage": equipment.attackDamage = float.Parse(value); break;
                case "skillPower": equipment.skillPower = float.Parse(value); break;
                case "attackSpeed": equipment.attackSpeed = float.Parse(value); break;
                case "physicalDefense": equipment.physicalDefense = float.Parse(value); break;
                case "elementResistance": equipment.elementResistance = float.Parse(value); break;
                case "maxHealth": equipment.maxHealth = float.Parse(value); break;
                case "energyRegen": equipment.energyRegen = float.Parse(value); break;
                case "critChance": equipment.critChance = float.Parse(value); break;
                case "elementPenetration": equipment.elementPenetration = float.Parse(value); break;
                case "moveSpeed": equipment.moveSpeed = float.Parse(value); break;
                case "effectIds": 
                    if (!string.IsNullOrEmpty(value))
                        equipment.effectIds = value.Split(',').Select(s => s.Trim()).ToArray();
                    break;
                case "description": equipment.description = value; break;
            }
        }

        private void SetMonsterField(MonsterData monster, string fieldName, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            switch (fieldName)
            {
                case "monsterId": monster.monsterId = value; break;
                case "monsterName": monster.monsterName = value; break;
                case "family": 
                    if (string.IsNullOrEmpty(value) || value == "None")
                        monster.family = MonsterFamily.None;
                    else if (System.Enum.TryParse<MonsterFamily>(value, true, out MonsterFamily family))
                        monster.family = family;
                    break;
                case "isElite": monster.isElite = bool.Parse(value); break;
                case "isBoss": monster.isBoss = bool.Parse(value); break;
                case "baseHealth": monster.baseHealth = float.Parse(value); break;
                case "baseAttackDamage": monster.baseAttackDamage = float.Parse(value); break;
                case "baseAttackSpeed": monster.baseAttackSpeed = float.Parse(value); break;
                case "baseMoveSpeed": monster.baseMoveSpeed = float.Parse(value); break;
                case "fireResistance": monster.fireResistance = float.Parse(value); break;
                case "iceResistance": monster.iceResistance = float.Parse(value); break;
                case "thunderResistance": monster.thunderResistance = float.Parse(value); break;
                case "earthResistance": monster.earthResistance = float.Parse(value); break;
                case "physicalResistance": monster.physicalResistance = float.Parse(value); break;
                case "abilityIds": 
                    if (!string.IsNullOrEmpty(value))
                        monster.abilityIds = value.Split(',').Select(s => s.Trim()).ToArray();
                    break;
                case "description": monster.description = value; break;
                case "minGoldDrop": monster.minGoldDrop = int.Parse(value); break;
                case "maxGoldDrop": monster.maxGoldDrop = int.Parse(value); break;
                case "equipmentDropChance": monster.equipmentDropChance = float.Parse(value); break;
            }
        }

        private T ParseEnum<T>(string value) where T : struct
        {
            if (System.Enum.TryParse<T>(value, true, out T result))
                return result;
            return default(T);
        }
    }
}


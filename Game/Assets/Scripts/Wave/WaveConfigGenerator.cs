using UnityEngine;
using System.Collections.Generic;
using EmberKeepers.Data;

namespace EmberKeepers.Wave
{
    /// <summary>
    /// 波次配置生成器，根据设计文档生成20波主线+10波无尽循环
    /// </summary>
    public class WaveConfigGenerator : MonoBehaviour
    {
        /// <summary>
        /// 生成主线20波配置
        /// </summary>
        public static List<WaveDataEntry> GenerateMainWaves()
        {
            List<WaveDataEntry> waves = new List<WaveDataEntry>();

            // 波次1-4：基础教学
            for (int i = 1; i <= 4; i++)
            {
                waves.Add(CreateBasicWave(i));
            }

            // 波次5-9：元素引入
            for (int i = 5; i <= 9; i++)
            {
                waves.Add(CreateElementalWave(i));
            }

            // 波次10：Boss 1
            waves.Add(CreateBoss1Wave());

            // 波次11-14：精英初见
            for (int i = 11; i <= 14; i++)
            {
                waves.Add(CreateEliteWave(i));
            }

            // 波次15：Boss 2
            waves.Add(CreateBoss2Wave());

            // 波次16-19：全面威胁
            for (int i = 16; i <= 19; i++)
            {
                waves.Add(CreateAdvancedWave(i));
            }

            // 波次20：终极Boss
            waves.Add(CreateFinalBossWave());

            return waves;
        }

        /// <summary>
        /// 生成无尽循环10波配置
        /// </summary>
        public static List<WaveDataEntry> GenerateEndlessWaves()
        {
            List<WaveDataEntry> waves = new List<WaveDataEntry>();

            // 10波循环配置
            waves.Add(CreateEndlessWave(21, "清理", new[] { "whisper", "cluster" }, 6));
            waves.Add(CreateEndlessWave(22, "清理", new[] { "whisper", "cluster" }, 6));
            waves.Add(CreateEndlessWave(23, "加速", new[] { "corroder", "bonewall" }, 6));
            waves.Add(CreateEndlessWave(24, "加速", new[] { "corroder", "bonewall" }, 6));
            waves.Add(CreateEndlessWave(25, "元素潮I", new[] { "fire_aberration", "ice_aberration", "chaos_mage" }, 6));
            waves.Add(CreateEndlessWave(26, "精英潮I", new[] { "flame_commander", "earth_devourer" }, 6));
            waves.Add(CreateEndlessWave(27, "精英潮I", new[] { "flame_commander", "earth_devourer" }, 6));
            waves.Add(CreateEndlessWave(28, "聚合与自爆", new[] { "cluster", "electric_fission" }, 6));
            waves.Add(CreateEndlessWave(29, "元素潮II", new[] { "frost_golem", "shadow_assassin" }, 6));
            waves.Add(CreateEndlessWave(30, "终极考验", new[] { "void_aggregate" }, 6));

            return waves;
        }

        // 波次1-4：基础教学
        private static WaveDataEntry CreateBasicWave(int waveNumber)
        {
            return new WaveDataEntry
            {
                waveNumber = waveNumber,
                difficulty = 1.0f + (waveNumber - 1) * 0.15f,
                hasBoss = false,
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "whisper",
                        count = 5 + waveNumber * 2,
                        level = Mathf.Min(waveNumber, 2),
                        spawnDelay = 0f,
                        spawnInterval = 0.5f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "corroder",
                        count = 2 + waveNumber,
                        level = 1,
                        spawnDelay = 3f,
                        spawnInterval = 1f
                    }
                }
            };
        }

        // 波次5-9：元素引入
        private static WaveDataEntry CreateElementalWave(int waveNumber)
        {
            return new WaveDataEntry
            {
                waveNumber = waveNumber,
                difficulty = 1.0f + (waveNumber - 1) * 0.15f,
                hasBoss = false,
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "bonewall",
                        count = 3,
                        level = 2,
                        spawnDelay = 0f,
                        spawnInterval = 2f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "fire_aberration",
                        count = 4,
                        level = waveNumber - 3,
                        spawnDelay = 2f,
                        spawnInterval = 1f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "ice_aberration",
                        count = 4,
                        level = waveNumber - 3,
                        spawnDelay = 5f,
                        spawnInterval = 1f
                    }
                }
            };
        }

        // 波次10：Boss 1 - 荒芜之主
        private static WaveDataEntry CreateBoss1Wave()
        {
            return new WaveDataEntry
            {
                waveNumber = 10,
                difficulty = 2.5f,
                hasBoss = true,
                bossId = "boss_desolate_lord",
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "corroder",
                        count = 10,
                        level = 3,
                        spawnDelay = 5f,
                        spawnInterval = 0.3f
                    }
                }
            };
        }

        // 波次11-14：精英初见
        private static WaveDataEntry CreateEliteWave(int waveNumber)
        {
            return new WaveDataEntry
            {
                waveNumber = waveNumber,
                difficulty = 1.0f + (waveNumber - 1) * 0.15f,
                hasBoss = false,
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "thunder_aberration",
                        count = 6,
                        level = 3,
                        spawnDelay = 0f,
                        spawnInterval = 1f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "chaos_mage",
                        count = 3,
                        level = 2,
                        spawnDelay = 3f,
                        spawnInterval = 2f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "electric_fission",
                        count = 1,
                        level = 1,
                        spawnDelay = 8f,
                        spawnInterval = 0f
                    }
                }
            };
        }

        // 波次15：Boss 2 - 永冻之灾
        private static WaveDataEntry CreateBoss2Wave()
        {
            return new WaveDataEntry
            {
                waveNumber = 15,
                difficulty = 3.5f,
                hasBoss = true,
                bossId = "boss_eternal_frost",
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "cluster",
                        count = 8,
                        level = 4,
                        spawnDelay = 5f,
                        spawnInterval = 1f
                    }
                }
            };
        }

        // 波次16-19：全面威胁
        private static WaveDataEntry CreateAdvancedWave(int waveNumber)
        {
            return new WaveDataEntry
            {
                waveNumber = waveNumber,
                difficulty = 1.0f + (waveNumber - 1) * 0.15f,
                hasBoss = false,
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "whisper",
                        count = 12,
                        level = 5,
                        spawnDelay = 0f,
                        spawnInterval = 0.3f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "shadow_assassin",
                        count = 1,
                        level = 1,
                        spawnDelay = 5f,
                        spawnInterval = 0f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "void_aggregate",
                        count = 1,
                        level = 1,
                        spawnDelay = 10f,
                        spawnInterval = 0f
                    }
                }
            };
        }

        // 波次20：终极Boss - 裂隙之心
        private static WaveDataEntry CreateFinalBossWave()
        {
            return new WaveDataEntry
            {
                waveNumber = 20,
                difficulty = 5.0f,
                hasBoss = true,
                bossId = "boss_rift_heart",
                monsterSpawns = new List<MonsterSpawnData>
                {
                    new MonsterSpawnData
                    {
                        monsterId = "flame_commander",
                        count = 1,
                        level = 1,
                        spawnDelay = 3f,
                        spawnInterval = 0f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "frost_golem",
                        count = 1,
                        level = 1,
                        spawnDelay = 6f,
                        spawnInterval = 0f
                    },
                    new MonsterSpawnData
                    {
                        monsterId = "earth_devourer",
                        count = 1,
                        level = 1,
                        spawnDelay = 9f,
                        spawnInterval = 0f
                    }
                }
            };
        }

        // 无尽模式波次
        private static WaveDataEntry CreateEndlessWave(int waveNumber, string theme, string[] monsterIds, int baseLevel)
        {
            // 计算无尽等级（每10波提升1级）
            int loopCount = (waveNumber - 21) / 10;
            int actualLevel = baseLevel + loopCount;

            WaveDataEntry wave = new WaveDataEntry
            {
                waveNumber = waveNumber,
                difficulty = actualLevel * 0.2f,
                hasBoss = false,
                monsterSpawns = new List<MonsterSpawnData>()
            };

            // 根据主题生成怪物
            foreach (string monsterId in monsterIds)
            {
                wave.monsterSpawns.Add(new MonsterSpawnData
                {
                    monsterId = monsterId,
                    count = 8 + loopCount * 2,
                    level = actualLevel,
                    spawnDelay = Random.Range(0f, 3f),
                    spawnInterval = 0.5f
                });
            }

            return wave;
        }
    }
}


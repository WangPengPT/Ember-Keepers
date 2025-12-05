# 余烬守卫 (Ember Keepers) - 代码架构文档

## 项目结构

### 核心系统 (Core/)
- **GameManager.cs** - 游戏主管理器，控制游戏状态和流程
- **WaveManager.cs** - 波次管理器，控制怪物波次的生成和进度
- **ResourceManager.cs** - 资源管理器，管理金币、星火碎片、星火精粹
- **HeroManager.cs** - 英雄管理器，管理所有英雄的创建、部署、状态

### 数据系统 (Data/)
- **ElementType.cs** - 元素类型、职业类型、稀有度等枚举定义
- **HeroData.cs** - 英雄数据表（ScriptableObject）
- **EquipmentData.cs** - 装备数据表（ScriptableObject）
- **MonsterData.cs** - 怪物数据表（ScriptableObject）
- **WaveDataAsset.cs** - 波次数据表（ScriptableObject）
- **DataManager.cs** - 数据管理器，负责加载和管理所有游戏数据

### 英雄系统 (Heroes/)
- **HeroBase.cs** - 英雄基类，包含属性、战斗、装备系统
- **HeroAI.cs** - 英雄AI组件，处理自动攻击、移动、技能释放

### 装备系统 (Equipment/)
- **EquipmentBase.cs** - 装备基类（ScriptableObject）

### 怪物系统 (Monsters/)
- **MonsterBase.cs** - 怪物基类，包含元素抗性、AI行为

### 基地系统 (Base/)
- **BaseCore.cs** - 基地核心（星火核心），包含升级、主动技能、防御系统

### 战斗系统 (Combat/)
- **CombatSystem.cs** - 战斗系统，处理伤害计算、元素克制

### 商店系统 (Shop/)
- **ShopSystem.cs** - 商店系统，处理波次间隙的购买逻辑

### 元进度系统 (MetaProgression/)
- **MetaProgressionManager.cs** - 元进度管理器，管理局外永久升级

## 游戏流程

1. **主菜单** → 开始新游戏
2. **策略阶段**（波次间隙）→ 购买英雄、属性书、装备，部署英雄
3. **战斗阶段** → 英雄自动战斗，怪物自动生成
4. **波次结束** → 返回策略阶段
5. **游戏结束** → 结算奖励，返回主菜单

## 待实现功能

1. Excel数据导入工具
2. 12个具体英雄实现（4元素×3职业）
3. 50件装备实现（18武器+18护甲+14饰品）
4. 30种怪物实现（6家族×5等级）
5. UI系统（RTS风格底部面板）
6. 英雄拖拽部署系统
7. 技能系统实现
8. 装备词条效果系统

## 开发规范

- 基于Unity 6.2开发
- 数据从Excel导入
- UI都做成Unity的Prefab，按需求动态加载
- 使用命名空间组织代码：`EmberKeepers.系统名`


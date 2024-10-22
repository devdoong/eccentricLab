// Created on: 2024-10-23
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum Scene
    {
        Unknown,
        DevScene,
        gameScenen
    }

    public enum Sound
    {
        BGM,
        Effect,
    }

    public enum ObjectType
    {
        Player,
        Monster,
        Projectile,
        Env
    }

    public enum SkillType
    {
        None,
        Melee,
        Projectile,
        Etc,
    }

    public const int PLAYER_DATA_ID = 1;
    public const string EXP_GEM_PREFAB = "EXPGem.prefab";
}

﻿namespace Throne.World.Network.Messages
{
    public enum ActionType : ushort
    {
        ObserveKnownPerson = 54,
        ConfirmLocation = 74,
        ConfirmAssets = 75,
        ConfirmAssociations = 76,
        ConfirmWeaponSkills = 77,
        ConfirmMagics = 78,
        ChangeDirection = 79,
        ChangeStance = 81,
        ChangeMap = 85,
        Teleport = 86,
        Levelup = 92,
        XpEnd = 93,
        Revive = 94,
        DeleteCharacter = 95,
        UpdateCombatMode = 96,
        ConfirmGuild = 97,
        SwingPickaxe = 99,
        BotChecks = 100,
        TeamMemberLocation = 101,
        QueryEntity = 102,
        MagicCastEnd = 103,
        MapEnvironmentColor = 104,
        MapStatus = 105,
        FindTeammate = 106,
        NewCoordinate = 108,
        RemoveMagic = 109,
        RemoveWeaponSkill = 110,
        ItemBooth = 111,
        SuspendBooth = 112,
        ResumeBooth = 113,
        GetSurroundings = 114,
        OpenCustom = 116,
        ObserveGear = 117,
        EndTransformation = 118,
        EndFly = 120,
        ObtainMoney = 121,
        ViewEnemyInfo = 123,
        OpenWindow = 126,
        Resurrect = 134,
        RemoveEntity = 135,
        InvisibleEntity = 136,
        Jump = 137,
        TeleportReply = 138,
        Die = 145,
        TeleportEnd = 146,
        ViewFriendInfo = 148,
        ChangeFace = 151,
        ViewPartnerInfo = 152,
        DetainWindowRequest = 153,
        PKItemDetainEffect = 154,
        PKItemRewardEffect = 155,
        FlashStep = 156,
        UpdatePosition = 157,
        HideGUI = 158,
        CountDown = 159,
        OpenWindow2 = 160,
        Away = 161,
        Pathfind = 162,
        ProgressBar = 164,
        DragonBallDropped = 165,
        PickTarget = 172,
        CancelPickTarget = 173,
        SendToTwinCity = 177,
        SetAppearance = 178,
        CompleteLogin = 251,
        LevelUpSpell = 252,
        LevelUpProficiency = 253,
        ObserveAssociateGear = 310,
        BeginSteedRace = 401,
        FinishSteedRace = 402,
        RequestItemStatus = 408
    }
}
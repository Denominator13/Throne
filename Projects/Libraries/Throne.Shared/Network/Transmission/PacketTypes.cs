﻿namespace Throne.Shared.Network.Transmission
{
    public enum PacketTypes : short
    {
        None = 0,
        Register = 1001,
        ChatMessage = 1004,
        CharacterInformation = 1006,
        ItemInformation = 1008,
        ItemAction = 1009,
        MsgTick = 1012,
        StringData = 1015,
        WeatherInformation = 1016,
        Friend = 1019,
        Interact = 1022,
        TimeSynchronize = 1033,
        ViewEquipment = 1040,
        LoadMap = 1044,
        MailOperation = 1045,
        MailList = 1046,
        MailNotify = 1047,
        MailContent = 1048,
        AuthenticateClient = 1052,
        AuthenticationAction = 1055,
        LoginSeed = 1059,
        AuthenticationRequest = 1086,
        RemoteMachineIdentification = 1100,
        MapItem = 1101,
        MapInfo = 1110,
        QuestStatus = 1134,
        QuestDetailInfo = 1135,
        GenericRanking = 1151,
        ThroneProtocolHandshake = 1213,
        ThroneProtocolReceiprocation = 1214,
        Auction = 1320,
        Auctionitem = 1321,
        AuctionQuery = 1322,
        SRP6ProtocolAuthenticationReqeust = 1542,
        NpcInfo = 2030,
        Npc = 2031,
        TaskDialog = 2032,
        FriendInfo = 2033,
        ServerInfo = 2079,


        GroundMovement = 10005,
        Action = 10010,
        RoleInfo = 10014
    }
}
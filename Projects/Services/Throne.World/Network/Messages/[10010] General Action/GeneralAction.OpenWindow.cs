using Throne.World.Structures.Objects;
using Throne.World.Structures.Objects.Actors;

namespace Throne.World.Network.Messages
{
    public partial class GeneralAction
    {
        public enum WindowId
        {
            Compose = 1,
            Craft = 2,
            Warehouse = 4,
            FirstCredit = 255,
            DetainRedeem = 336,
            DetainClaim = 337,
            VIPWarehouse = 341,
            Breeding = 368,
            PurificationWindow = 455,
            StabilizationWindow = 459,
            TalismanUpgrade = 347,
            GemComposing = 422,
            Blessing = 426,
            TortoiseGemComposing = 438,
            RefineryStabilization = 448,
            HorseRacingStore = 464,
            Reincarnation = 485,
            ChangeName = 489,
            GarmentShop = 502,
            DegradeEquipment = 506,
            GuildRecruitment = 542,
            SecondaryPasswordVerification = 568,
            Auction = 572,
            AuctionBrowse = 573,
            AuctionCreate = 574,
            AuctionWatch = 575,
            MailBox = 576,
            JiangHu = 617
        }

        public GeneralAction OpenWindow(WindowId id, IWorldObject obj)
        {

            return OpenWindow(id);
        }

        public GeneralAction OpenWindow(WindowId id)
        {
            Argument = (long) id;
            ShortArgumentEx1 = Character.Location.Position.X;
            ShortArgumentEx2 = Character.Location.Position.Y;
            ObjectId = Character.ID;
            return this;
        }
    }
}
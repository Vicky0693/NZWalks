namespace NZWalks.API.Model.Domain
{
    public class Walk
    {

        public Guid ID { get; set; }
        public String Name { get; set; }
        public Double Length { get; set; }

        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }



        //navigatation Propraty

        public Region Region { get; set; }

        public WalkDifficulty WalkDifficulty { get; set; }
    }
}

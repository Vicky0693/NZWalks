﻿namespace NZWalks.API.Model.DTO
{
    public class AddwalkRequest
    {


        public String Name { get; set; }
        public Double Length { get; set; }

        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}

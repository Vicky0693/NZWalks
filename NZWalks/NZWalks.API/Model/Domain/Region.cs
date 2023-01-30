namespace NZWalks.API.Model.Domain
{
    public class Region
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public Double Lat { get; set; }
        public Double Long { get; set; }
        public long Population { get; set; }
         

        //navigatation Propraty

        public IEnumerable<Walk>Walks { get; set; }
    }
}

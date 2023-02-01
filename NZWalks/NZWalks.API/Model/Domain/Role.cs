namespace NZWalks.API.Model.Domain
{
    public class Role
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        //nevigataton Property

        public List<User_Role>UserRoles { get; set; }
    }
}

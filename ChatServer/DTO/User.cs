namespace ChatServer.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }

        public long? GroupId { get; set; }
    }
}
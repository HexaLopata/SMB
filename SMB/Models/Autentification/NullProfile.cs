namespace SMB.Models.Autentification
{
    public class NullProfile : IProfile
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public ProfileRank Rank { get; set; }

        public NullProfile()
        {
            Name = "I am null profile";
            Password = "I am null password";
            Rank = ProfileRank.Null;
        }
    }
}
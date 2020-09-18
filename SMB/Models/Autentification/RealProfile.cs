using System.ComponentModel.DataAnnotations;

namespace SMB.Models.Autentification
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class RealProfile : IProfile
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public ProfileRank Rank { get; set; }
    }
}
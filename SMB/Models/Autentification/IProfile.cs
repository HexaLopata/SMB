using System.ComponentModel.DataAnnotations;

namespace SMB.Models.Autentification
{
    /// <summary>
    /// Ранг пользователя, может использоваться, для ограничения прав
    /// </summary>
    public enum ProfileRank
    {
        Null, // Используется только для NullObject
        Admin,
    }

    public interface IProfile
    {
        [Key]
        string Name { get; set; }
        [Required]
        string Password { get; set; }
        ProfileRank Rank { get; set; }
    }
}
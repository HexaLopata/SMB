using SMB.Models.DataBases;

namespace SMB.Models.Autentification
{
    /// <summary>
    /// Реализовывает IProfileManager с хэшированием паролей
    /// </summary>
    public class HashedProfileManager : IProfileManager
    {
        SMBContext _db;

        /// <summary>
        /// Сверяет логин с базой данных и проверяет хэш пароля, если хэши совпадают, то возвращает профиль с указанным логином
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Не хэшированный пароль</param>
        /// <param name="db">База данных с профилями</param>
        /// <returns>Если профиль найден, то возвращает его, иначе возвращает NullObject</returns>
        public IProfile ReturnProfileFromDB(string login, string password)
        {
            var profile = _db.Profiles.Find(login);
            if (profile == null)
                return new NullProfile();

            PasswordHasher passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.ReturnHashedPasswordAsString(password);

            if(hashedPassword == profile.Password)
            {
                return profile;
            }

            return new NullProfile();
        }

        public HashedProfileManager(SMBContext db)
        {
            _db = db;
        }
    }
}
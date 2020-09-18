namespace SMB.Models.Autentification
{
    interface IProfileManager
    {
        IProfile ReturnProfileFromDB(string login, string password);
    }
}
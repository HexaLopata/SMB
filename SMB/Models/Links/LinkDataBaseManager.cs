using SMB.Models.DataBases;
using System.Data.Entity;

namespace SMB.Models.Links
{
    public class LinkDataBaseManager : ILinkDataBaseManager
    {
        /// <summary>
        /// Adds the object in data base.
        /// </summary>
        /// <returns><c>true</c>, if object in data base was added, <c>false</c> otherwise.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="modelSet">Model set.</param>
        /// <param name="db">Db.</param>
        /// <typeparam name="T">Model Type</typeparam>
        public bool AddObjectInDataBase<T>(T obj, DbSet<T> modelSet, DbContext db) where T : class
        {
            modelSet.Add(obj);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Deletes the object by identifier.
        /// </summary>
        /// <returns><c>true</c>, if object by identifier was deleted, <c>false</c> otherwise.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="modelSet">Model set.</param>
        /// <param name="db">Db.</param>
        /// <typeparam name="T">Model Type</typeparam>
        public bool DeleteObjectById<T>(int id, DbSet<T> modelSet, DbContext db) where T : class
        {
            var model = modelSet.Find(id);
            if (model != null)
            {
                modelSet.Remove(model);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

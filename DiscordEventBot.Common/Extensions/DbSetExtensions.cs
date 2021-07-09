using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class DbSetExtensions
    {
        #region Public Methods

        /// <summary>
        /// finds an entity described by it's key values. If not found one will be created and added
        /// to the DbSet
        /// </summary>
        /// <typeparam name="TEntity"> </typeparam>
        /// <param name="entities">  </param>
        /// <param name="keyValues"> </param>
        /// <returns> </returns>
        public static async Task<TEntity> FindOrCreateAsync<TEntity>(this DbSet<TEntity> entities, params object[] keyValues) where TEntity : class
        {
            TEntity entity = await entities.FindAsync(keyValues);
            if (entity == null)
            {
                var constructor = typeof(TEntity).GetConstructor(keyValues.Select(v => v.GetType()).ToArray());
                if (constructor == null) throw new NotSupportedException($"{typeof(TEntity).FullName} must provide a constructor that matches the key types");

                entity = constructor.Invoke(keyValues) as TEntity;
                entities.Add(entity);
            }

            return entity;
        }

        #endregion Public Methods
    }
}
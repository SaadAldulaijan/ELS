using ELS.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Data
{
    public class DataAccess
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DataAccess()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Location).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Location)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Location>> GetItemsAsync()
        {
            return Database.Table<Location>().ToListAsync();
        }

        //public Task<List<Location>> GetItemsNotDoneAsync()
        //{
        //    return Database.QueryAsync<Location>("SELECT * FROM [Location] WHERE [Done] = 0");
        //}

        public Task<Location> GetItemAsync(int id)
        {
            return Database.Table<Location>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Location item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Location item)
        {
            return Database.DeleteAsync(item);
        }
    }
}

using E_Commerce.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DbInitliazer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ECommerceDbContext _Context;

        public DbInitializer(ECommerceDbContext context)
        {
            _Context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_Context.Database.GetPendingMigrations().Count() > 0)
                {
                    _Context.Database.Migrate();
                }
            }
            catch (Exception ex) { }
        }
    }
}

using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class ApplicationUserRepo(DataContext context) 
    {
        private readonly DataContext context = context;

        public async Task<ApplicationUser> SaveApplicationUserToDb(ApplicationUser user){
            context.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUsers() => await context.ApplicationUsers.ToListAsync();
        public async Task<ApplicationUser?> GetUserById(int id) => await context.ApplicationUsers.FindAsync(id);
        public async Task<bool> IsUserExistsInDB(int id) => await context.ApplicationUsers.AnyAsync(x => x.Id == id);
        public async Task<bool> UpdateApplicationUser(int id, ApplicationUser user){
            bool isIdsMatch = id == user.Id;
            bool userExistsInDB = await IsUserExistsInDB(id);

            if (!isIdsMatch || !userExistsInDB){
                return false;
            }
            
            context.Update(user);
            int changesCount = await context.SaveChangesAsync();
            return changesCount == 1;
        }
        public async Task<bool> DeleteUserById(int id){
            ApplicationUser? user = await GetUserById(id);
            if (user == null){
                return false;
            }

            context.Remove(user);
            int changesCount = await context.SaveChangesAsync();
            return changesCount == 1;
        }


       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class ApplicationUserRepo(DataContext context) 
    {
        private readonly DataContext _context = context;

        public async Task<ApplicationUser> SaveApplicationUserToDb(ApplicationUser user){
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUsers() => await _context.ApplicationUsers.ToListAsync();
        public async Task<ApplicationUser?> GetUserById(int userId) => await _context.ApplicationUsers.FindAsync(userId);
        public async Task<bool> IsUserExistsInDB(string id) => await _context.ApplicationUsers.AnyAsync(x => x.Id == id);
        public async Task<bool> UpdateApplicationUser(string id, ApplicationUser user){
            bool isIdsMatch = id == user.Id;
            bool userExistsInDB = await IsUserExistsInDB(id);

            if (!isIdsMatch || !userExistsInDB){
                return false;
            }
            
            _context.Update(user);
            int changesCount = await _context.SaveChangesAsync();
            return changesCount == 1;
        }


       
    }
}
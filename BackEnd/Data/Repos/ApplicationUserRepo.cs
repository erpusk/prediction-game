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

        public async Task<List<ApplicationUser>> GetAllUsers() => await _context.ApplicationUsers.ToListAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class EventRepo
    {
        private readonly DataContext _context;

        public EventRepo(DataContext context){
            _context = context;
        }

        public async Task<List<Event>> GetAll(){
            IQueryable<Event> result = _context.Events.AsQueryable();
            return await result.ToListAsync();
        }

        public async Task<Event?> GetById(int id) => await _context.Events.FindAsync(id);
    }
}
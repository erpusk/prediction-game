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

        public async Task<Event> AddEvent(Event eventModel){
            await _context.AddAsync(eventModel);
            await _context.SaveChangesAsync();
            return eventModel;
        }

        public async Task<Event?> UpdateEvent(int Id, Event eventModel){
            bool idsMatch = Id == eventModel.Id;
            bool eventExists = await _context.Events.AnyAsync(x => x.Id == Id);
            if (!eventExists || !idsMatch){
                return null;
            }
            _context.Update(eventModel);
            await _context.SaveChangesAsync();
            return eventModel;
        }
        public async Task<bool> DeleteEvent(int Id){
            var result = await GetById(Id);
            if(result == null){
                return false;
            }
            _context.Remove(result);
            int changesCount = await _context.SaveChangesAsync();
            return changesCount == 1;
        }
    }
}
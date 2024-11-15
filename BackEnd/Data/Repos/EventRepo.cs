using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class EventRepo
    {
        private readonly DataContext _context;

        public EventRepo(DataContext context){
            _context = context;
        }

        public async Task<List<Event>> GetAll(int? PredictionGameId = null){
            IQueryable<Event> result = _context.Events.AsQueryable();
            if (PredictionGameId.HasValue){
                result = result.Where(x => x.PredictionGameId == PredictionGameId);
            }
            return await result.ToListAsync();
        }

        public async Task<Event?> GetById(int id) => await _context.Events.Include(e => e.Predictions).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Event> AddEvent(Event eventModel){
            await _context.AddAsync(eventModel);
            await _context.SaveChangesAsync();
            return eventModel;
        }

        public async Task<bool> UpdateEvent(int Id, Event eventModel){
            bool idsMatch = Id == eventModel.Id;
            bool eventExists = await _context.Events.AnyAsync(x => x.Id == Id);
            if (!eventExists || !idsMatch){
                return false;
            }
            _context.Update(eventModel);
            
            int changesCount = await _context.SaveChangesAsync();
            return changesCount > 0;
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
using APIMtChalet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Repositories {
    public class MtChaletRepository : IMtChaletRepository {

        private readonly MtChaletDBContext _context;
        public MtChaletRepository(MtChaletDBContext context) {
            _context = context;
        }
        public async Task<Reservation> CreateReservation(Reservation reservation) {
            reservation.CreationDate = DateTime.Now;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task DeleteReservation(int id) {
            var reservationToDelete = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservationToDelete);
            await _context.SaveChangesAsync();
        }


        public async Task<Reservation> GetReservation(int id) {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetReservation(DateTime date) {

            var t = await Task.Run(() => { return _context.Reservations.Where(s => s.StartingDate >= date && s.EndingDate <= date).AsEnumerable(); });
            return t;
        }

        public async Task<IEnumerable<Room>> GetRooms() {
            return await _context.Rooms.ToListAsync();
        }

        public async Task UpdateReservation(Reservation reservation) {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

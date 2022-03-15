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

            var t = await Task.Run(() => { return _context.Reservations.Where(s => s.StartingDate == date).AsEnumerable(); });
            return t;
        }

        public async Task<IEnumerable<Room>> GetRooms() {
            return await _context.Rooms.ToListAsync();
        }

        public async Task UpdateReservation(Reservation reservation) {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation> EditReservation(ReservationsEditHistory newReservation) {

            //pobierz poprzednia
            var oldReservation = await _context.Reservations.FindAsync(newReservation.OldReservationId);
            //dodaj nowa
            var temp = new Reservation {
                Name = newReservation.Name,
                Surname = newReservation.Surname,
                RoomId = newReservation.RoomId,
                NumberOfPeople = newReservation.NumberOfPeople,
                StartingDate = newReservation.StartingDate,
                EndingDate = newReservation.EndingDate,
                Phone = newReservation.Phone,
                Email = newReservation.Email,
                ExtraInfo = newReservation.ExtraInfo,
                EmployeeId = newReservation.EditedByEmployeeId,
                CreationDate = newReservation.CreationDate
            };
            _context.Reservations.Add(temp);
            await _context.SaveChangesAsync();

            //usuń poprzednia
            _context.Reservations.Remove(oldReservation);
            await _context.SaveChangesAsync();

            //poprzednia dodaj do historii
            _context.ReservationsEditsHistory.Add(new ReservationsEditHistory {
                OldReservationId = oldReservation.ReservationId,
                Name = oldReservation.Name,
                Surname = oldReservation.Surname,
                RoomId = oldReservation.RoomId,
                NumberOfPeople = oldReservation.NumberOfPeople,
                StartingDate = oldReservation.StartingDate,
                EndingDate = oldReservation.EndingDate,
                Phone = oldReservation.Phone,
                Email = oldReservation.Email,
                ExtraInfo = oldReservation.ExtraInfo,
                EmployeeId = oldReservation.EmployeeId,
                CreationDate = oldReservation.CreationDate,
                EditDate = DateTime.Now,
                EditedByEmployeeId = newReservation.EditedByEmployeeId,
                NewReservationId = _context.Reservations.Where(s => s.Surname == newReservation.Surname
                                                                && s.RoomId == newReservation.RoomId
                                                                && s.NumberOfPeople == newReservation.NumberOfPeople
                                                                && s.StartingDate == newReservation.StartingDate
                                                                && s.EndingDate == newReservation.EndingDate).FirstOrDefault().ReservationId
            });

            await _context.SaveChangesAsync();


            return temp;
        }
    }
}

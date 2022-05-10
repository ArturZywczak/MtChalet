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
            //Add reservation to DB
            reservation.CreationDate = DateTime.Now;
            var temp = _context.Reservations.Add(reservation);

            //Add creation info to EditHistory
            _context.ReservationsEditsHistory.Add(new ReservationsEditHistory {
                OldReservationId = 0,
                Name = "",
                RoomId = 0,
                NumberOfPeople = 0,
                StartingDate = DateTime.Now,
                EndingDate = DateTime.Now,
                EmployeeId = reservation.EmployeeId,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                EditedByEmployeeId = "SYSTEM",
                NewReservationId = temp.Entity.ReservationId
            }); ;  
            
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

            var t = await Task.Run(() => { 
                return _context.Reservations.Where(s => s.StartingDate == date).AsEnumerable(); //Used to convert from List to IEnumerable
                }); 
            return t;
        }

        public async Task<IEnumerable<Room>> GetRooms() {
            return await _context.Rooms.ToListAsync();
        }

        public async Task EditReservation(Reservation reservation) {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation> EditReservation(ReservationsEditHistory newReservation) {
            //Get previous reservation to temp
            var oldReservation = await _context.Reservations.FindAsync(newReservation.OldReservationId);
            
            //Add new reservation
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

            //Remove previous reservation
            _context.Reservations.Remove(oldReservation);
            await _context.SaveChangesAsync();

            //Add previous reservation to edit history
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

        public async Task<IEnumerable<ReservationsEditHistory>> GetReservationEditHistory(int id) {

            List<ReservationsEditHistory> result = new List<ReservationsEditHistory>();

            var temp = _context.ReservationsEditsHistory.Where(s => s.NewReservationId == id).FirstOrDefault();
            result.Add(temp);

            while (temp.OldReservationId != 0) {
                temp = await _context.ReservationsEditsHistory
                    .Where(s => s.NewReservationId == temp.OldReservationId)
                    .FirstOrDefaultAsync();
                result.Add(temp);
            }
            var t = await Task.Run(() => { return result; }); //Used to convert from List to IEnumerable
            return t;

        }

        public async Task<Employee> GetEmployee(string auth0ID) {

            return await _context.Employees.Where(s => s.Auth0ID == auth0ID).FirstOrDefaultAsync();
        }
    }
}

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
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            
            //TODO W DOMU sprawdź czy można podmienić wstawianie NewReservationID tym:
            //var test = _context.Reservations.Add(reservation);
            //NewReservationID = test.Entity.ReservationId
            //Dodatkowo wtedy lepiej jakby zwracał tego testa

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
                NewReservationId = _context.Reservations.Where(s => s.Surname == reservation.Surname
                                                                && s.RoomId == reservation.RoomId
                                                                && s.NumberOfPeople == reservation.NumberOfPeople
                                                                && s.StartingDate == reservation.StartingDate
                                                                && s.EndingDate == reservation.EndingDate).FirstOrDefault().ReservationId
            });
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task DeleteReservation(int id) {
            var reservationToDelete = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservationToDelete);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteReservationWithBody(ReservationForDeleting data) {
            var reservationToDelete = await _context.Reservations.FindAsync(data.ReservationId);
            _context.Reservations.Remove(reservationToDelete);
            await _context.SaveChangesAsync();

            //TODO po zmienieniu ReservationEditsHistory zmień to tak aby wykorzystywało nowy konstruktor z rezerwacją
            //Add delete information to EditHistory
            _context.ReservationsEditsHistory.Add(new ReservationsEditHistory {
                OldReservationId = data.ReservationId,
                Name = reservationToDelete.Name,
                Surname = reservationToDelete.Surname,
                RoomId = reservationToDelete.RoomId,
                NumberOfPeople = reservationToDelete.NumberOfPeople,
                StartingDate = reservationToDelete.StartingDate,
                EndingDate = reservationToDelete.EndingDate,
                Phone = reservationToDelete.Phone,
                Email = reservationToDelete.Email,
                ExtraInfo = reservationToDelete.ExtraInfo,
                EmployeeId = reservationToDelete.EmployeeId,
                CreationDate = reservationToDelete.CreationDate,
                EditDate = DateTime.Now,
                EditedByEmployeeId = data.EmployeeId,
                NewReservationId = 0
            });
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

        public async Task UpdateReservation(Reservation reservation) {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation> EditReservation(ReservationsEditHistory newReservation) {
            //TODO niekoniecznie w domu sprawdź które SaveChangesAsync można usunąć
            //Get previous reservation to temp
            var oldReservation = await _context.Reservations.FindAsync(newReservation.OldReservationId);
            
            //Add new reservation
            //TODO remake with new method from ReservationEditHistory
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

            //TODO niekoniecznie upewnij się że zwraca pustą jeśli błąd
            return temp;
        }

        public async Task<IEnumerable<ReservationsEditHistory>> GetReservationEditHistory(int id) {

            List<ReservationsEditHistory> result = new List<ReservationsEditHistory>();

            var temp = _context.ReservationsEditsHistory.Where(s => s.NewReservationId == id).FirstOrDefault();
            result.Add(temp);

            while (temp.OldReservationId != 0) {
                temp = await _context.ReservationsEditsHistory.Where(s => s.NewReservationId == temp.OldReservationId).FirstOrDefaultAsync();
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

using APIMtChalet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Repositories {
    public interface IMtChaletRepository {

        //RESERVATIONS

        Task<IEnumerable<Reservation>> GetReservation(DateTime date);
        Task<Reservation> GetReservation(int id);
        Task<Reservation> CreateReservation(Reservation reservation);
        Task DeleteReservation(int id);

        //ROOMS
        Task<IEnumerable<Room>> GetRooms();

        //RESERVATION EDITS
        Task<Reservation> EditReservation(ReservationsEditHistory newReservation);
        Task<IEnumerable<ReservationsEditHistory>> GetReservationEditHistory(int id);

        //EMPLOYEES
        Task<Employee> GetEmployee(string auth0ID);
        Task DeleteReservationWithBody(ReservationForDeleting data);
        Task EditReservation(Reservation reservation);
    }
}

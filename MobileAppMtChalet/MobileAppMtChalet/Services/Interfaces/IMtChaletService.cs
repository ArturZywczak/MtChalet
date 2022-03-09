using MobileAppMtChalet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAppMtChalet.Services {
    public interface IMtChaletService {

        Task<IEnumerable<Reservation>> GetReservationOnDate(string date);
        Task AddReservation(Reservation reservation);
        Task EditReservation(int id, Reservation reservation);
        Task DeleteReservation(int reservationID);
        Task<IEnumerable<Room>> GetRooms();
        Task<Reservation> GetReservation(string id);

    }
}
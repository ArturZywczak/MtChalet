using MobileAppMtChalet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAppMtChalet.Services {
    public interface IMtChaletService {

        Task<IEnumerable<Reservations>> GetReservationOnDate(string date);
        Task AddReservation(Reservations reservation);
        Task EditReservation(Reservations oldReservation, Reservations newReservation);
        Task DeleteReservation(Reservations reservation);

    }
}
using MobileAppMtChalet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAppMtChalet.Services {
    public interface IMtChaletService {

        ///<summary> Gets list of reservation on given date using API</summary>
        ///<param name="date"> date as string in format ddMMrrrr </param>
        ///<returns> List of reservations on this day </returns>
        Task<IEnumerable<Reservation>> GetReservationOnDate(string date);
        
        ///<summary> Adds reservation to db using API </summary>
        ///<param name="reservation"> Reservation to add </param>
        Task AddReservation(Reservation reservation);
        
        ///<summary> Edits reservation in db using API</summary>
        ///<param name="reservation"> Reservation with extra info, such as old reservation id and employee info </param>
        Task EditReservation(EditedReservation reservation);
        
        ///<summary> Removes reservation from db using API </summary>
        ///<param name="reservationID"> ID of reservation to delete </param>
        Task DeleteReservation(int reservationID);
        
        ///<summary> Get room info from db using API </summary>
        ///<param name="reservationID"> ID of reservation to delete </param>
        ///<returns> List of all avaliable rooms with details </returns>
        Task<IEnumerable<Room>> GetRooms();
        
        ///<summary> Gets reservation using reservation id from db using API </summary>
        ///<param name="id"> Reservation ID number </param>
        Task<Reservation> GetReservation(string id);
        
        ///<summary> Gets reservation edit history using reservation id from db using API </summary>
        ///<param name="id"> Reservation ID number </param>
        ///<returns> List of reservation edit history <returns>
        Task<IEnumerable<EditedReservation>> GetEditReservationDetails(int reservationID);
        
        ///<summary> Gets extra employee info using auth0 provided id from db using API </summary>
        ///<param name="auth0ID"> auth0 ID string </param>
        ///<returns> Extra employee info <returns>
        Task<Employee> GetEmployee(string auth0ID);

    }
}
using APIMtChalet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Repositories {
    public interface IMtChaletRepository {

        //RESERVATIONS

        ///<summary> Get reservations from DB </summary>
        ///<param name="date"> Date in which reservation appear </param>
        ///<returns> List of reservations on given date</returns>
        Task<IEnumerable<Reservation>> GetReservation(DateTime date);

        ///<summary> Get single reservations from DB </summary>
        ///<param name="id"> Reservation ID number </param>
        ///<returns> Reservation if found, empty reservation if not found</returns>
        Task<Reservation> GetReservation(int id);

        ///<summary> Add reservation to DB </summary>
        ///<param name="reservation"> Reservation to add </param>
        ///<returns> Added reservation </returns>
        Task<Reservation> CreateReservation(Reservation reservation);

        ///<summary> Edit reservation </summary>
        ///<param name="reservation"> Reservation to edit </param>
        ///<returns> Status code of operation</returns>
        Task UpdateReservation(Reservation reservation);

        ///<summary> Delete reservation from DB </summary>
        ///<param name="id"> Reservation id </param>
        ///<returns> Status code of operation </returns>
        Task DeleteReservation(int id);

        ///<summary> Delete reservation from DB </summary>
        ///<remarks> Using this instead of simple id for saving edit/delete history change </remarks>
        ///<param name="data"> Reservation id and Employee id </param>
        ///<returns> Status code of operation </returns>
        //TODO Niepotrzebne stworzenie nowego typu? może lepiej poprostu przekazać dwa parametry
        Task DeleteReservationWithBody(ReservationForDeleting data);

        //ROOMS
        ///<summary> Gets list of all avaliable rooms </summary>
        ///<returns> List of all rooms with details </returns>
        Task<IEnumerable<Room>> GetRooms();

        //RESERVATION HISTORY
        ///<summary> Edit reservation in DB </summary>
        ///<remarks> Usng Reservation Edit History helps pass extra data, such ass employee info </remarks>
        ///<param name="newReservation"> Edited reservation with employee info </param>
        ///<returns> Added reservation, empty if error </returns>
        Task<Reservation> EditReservation(ReservationsEditHistory newReservation);

        ///<summary> Get list of reservation edits </summary>
        ///<param name="id"> Reservation ID </param>
        ///<returns> List of reservation edits, empty if reservation not found </returns>
        //TODO upewnij sie że zwraca ppuste jak błąd
        Task<IEnumerable<ReservationsEditHistory>> GetReservationEditHistory(int id);

        //EMPLOYEES

        ///<summary> Get employee data using auth0 ID </summary>
        ///<param name="auth0ID"> ID created using auth0 authentication service </param>
        ///<returns> Employee data, empty if employee not found </returns>
        //TODO upewnij sie że zwraca ppuste jak błąd
        Task<Employee> GetEmployee(string auth0ID);
    }
}

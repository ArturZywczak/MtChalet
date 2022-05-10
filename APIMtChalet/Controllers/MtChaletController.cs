using APIMtChalet.Models;
using APIMtChalet.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MtChaletController : ControllerBase {

        private readonly IMtChaletRepository _mtChaletRepository;
        public MtChaletController(IMtChaletRepository mtChaletRepository) {

            _mtChaletRepository = mtChaletRepository;
        }

        ///<summary> Gets data as string in format ddMMrrrr, returns reservations on date</summary>
        ///<param name="dateRaw"> Reservation date from link, in ddMMrrrr format </param>
        ///<returns>List of reservations on given date </returns>
        ///<exception cref="ArgumentException"> Throws when input date is in wrong format </exception>
        [HttpGet("reservations/date/{dateRaw}")]
        public async Task<IEnumerable<Reservation>> GetReservationsByDate(string dateRaw) {

            //Check if date is in correct format, parse it to DateTime format
            if (dateRaw.Length == 8) {
                string dateStr = dateRaw[0].ToString();
                dateStr = dateStr + dateRaw[1] + "/" + dateRaw[2] + dateRaw[3] + "/" + dateRaw[4] + dateRaw[5] + dateRaw[6] + dateRaw[7];
                DateTime date = DateTime.Parse(dateStr);

                return await _mtChaletRepository.GetReservation(date);
            }
            else {
                throw new ArgumentException("Cos poszlo bardzo źle, data jest w zlym formacie");
            }
        }

        ///<summary> Get reservation from reservation id</summary>
        ///<param name="resId"> Reservation ID number </param>
        ///<returns>Reservation with correct ID, empty Reservation if not found</returns>
        [HttpGet("reservations/id/{resId}")]
        public async Task<Reservation> GetReservationsById(int resId) {

            return await _mtChaletRepository.GetReservation(resId);
        }

        ///<summary> Gets reservation edit history from reservation id </summary>
        ///<param name="resId"> Reservation ID number </param>
        ///<returns>List of reservation edits, empty if reservation not found. If no edits were made it still returns initial reservation</returns>
        [HttpGet("reservations/id/edit/{resId}")]
        public async Task<IEnumerable<ReservationsEditHistory>> GetReservationEditHistoryById(int resId) {

            return await _mtChaletRepository.GetReservationEditHistory(resId);
        }

        ///<summary> Add reservation </summary>
        ///<param name="reservation"> Reservation to add, passed in request body </param>
        ///<returns>Reservation added</returns>
        [HttpPost("reservations")]
        public async Task<ActionResult<Reservation>>PostReservation([FromBody] Reservation reservation) {
            var newReservation = await _mtChaletRepository.CreateReservation(reservation);
            return CreatedAtAction(nameof(PostReservation), new { id = newReservation.ReservationId }, newReservation);
        }

        ///<summary> Edits reservation</summary>
        ///<remarks> Using HttpPost instead of put because IS9 throws 405, tried to fix it but made this workaround instead </remarks>
        ///<param name="reservation"> Reservation to edit </param>
        ///<returns> Updated reservation</returns>
        [HttpPost("reservations/edit")]
        public async Task<ActionResult<Reservation>> UpdateReservation([FromBody] ReservationsEditHistory reservation) {

            var newReservation = await _mtChaletRepository.EditReservation(reservation);
            return CreatedAtAction(nameof(UpdateReservation), new { id = newReservation.ReservationId }, newReservation);
        }

        ///<summary> Edit reservation using HttpPut </summary>
        ///<param name="id"> ID of reservation to edit </param>
        ///<param name="reservation"> Edited reservation </param>
        ///<returns> <c>400</c> if <paramref name="id"/> doesnt match <paramref name="reservation"/>, 
        ///<c>200</c> if changes made succesfully </returns>
        [HttpPut]
        public async Task<ActionResult> PutReservation(int id, [FromBody] Reservation reservation) {
            if(id != reservation.ReservationId) {
                return BadRequest();
            }
            
            await _mtChaletRepository.EditReservation(reservation);

            return Ok();
        }

        ///<summary> Remove reservation </summary>
        ///<param name="id"> ID of reservation to edit </param>
        ///<returns> <c>404</c> if <paramref name="id"/> not found, 
        ///<c>200</c> if deleted succesfully </returns>
        [HttpDelete("reservations/{id}")]
        public async Task<ActionResult> DeleteReservation (int id) {
            var reservationToDelete = await _mtChaletRepository.GetReservation(id);
            if (reservationToDelete == null) return NotFound();

            await _mtChaletRepository.DeleteReservation(reservationToDelete.ReservationId);
            return Ok();
        }

        ///<summary> Remove reservation </summary>
        ///<remarks> Passing entre reservation in body instead of reservation id for creating edit history </remarks>
        ///<param name="data"> Reservation to delete </param>
        ///<returns> <c>404</c> if <paramref name="data"/> not found, 
        ///<c>200</c> if deleted succesfully </returns>
        [HttpDelete("reservations")]
        public async Task<ActionResult> DeleteReservationWithBody([FromBody] ReservationForDeleting data) {
            var reservationToDelete = await _mtChaletRepository.GetReservation(data.ReservationId);
            if (reservationToDelete == null) return NotFound();

            await _mtChaletRepository.DeleteReservationWithBody(data);
            return Ok();
        }

        ///<summary> Get information about rooms </summary>
        ///<returns> List of avaliable rooms with their details </returns>
        [HttpGet("rooms")]
        public async Task<IEnumerable<Room>> GetRooms() {
            return await _mtChaletRepository.GetRooms();
        }

        ///<summary> Get employee data </summary>
        ///<param name="auth0ID"> User ID given by auth0 authentication service </param>
        ///<returns> Acces permissions and extra personal details about Employee </returns>
        [HttpGet("employee/{auth0ID}")]
        public async Task<Employee> GetEmployee(string auth0ID) {
            return await _mtChaletRepository.GetEmployee(auth0ID);
        }
    }
}

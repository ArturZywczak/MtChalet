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


        [HttpGet("{dateRaw}")]
        public async Task<IEnumerable<Reservation>> GetReservationsByDate(string dateRaw) {

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

        [HttpGet("id/{resId}")]
        public async Task<Reservation> GetReservationsById(int resId) {

                return await _mtChaletRepository.GetReservation(resId);
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>>PostReservation([FromBody] Reservation reservation) {
            var newReservation = await _mtChaletRepository.CreateReservation(reservation);
            return CreatedAtAction(nameof(PostReservation), new { id = newReservation.ReservationId }, newReservation);
        }

        [HttpPut]
        public async Task<ActionResult> PutReservation(int id, [FromBody] Reservation reservation) {
            if(id != reservation.ReservationId) {
                return BadRequest();
            }

            await _mtChaletRepository.UpdateReservation(reservation);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation (int id) {
            var reservationToDelete = await _mtChaletRepository.GetReservation(id);
            if (reservationToDelete == null) return NotFound();

            await _mtChaletRepository.DeleteReservation(reservationToDelete.ReservationId);
            return Ok();
        }

        [HttpGet("rooms")]
        public async Task<IEnumerable<Room>> GetRooms() {
            return await _mtChaletRepository.GetRooms();
        }
    }
}

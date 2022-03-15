using System;
using System.Collections.Generic;

#nullable disable

namespace APIMtChalet.Models
{
    public partial class ReservationsEditHistory {
        public int ReservationEditId { get; set; }
        public int OldReservationId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoomId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ExtraInfo { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string EditedByEmployeeId { get; set; }
        public int? NewReservationId { get; set; }
    }
}

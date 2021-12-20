using System;
using System.Collections.Generic;

namespace MobileAppMtChalet.Models {
    public partial class Reservation {
        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoomId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ExtraInfo { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public partial class ReservationTemp {

        public List<ReservationTempSingle> BedsList {get;set;}

        public ReservationTemp() {
            this.BedsList = new List<ReservationTempSingle>();
        }
    }
    public class ReservationTempSingle {

        public int RoomID { get; set; }
        public int BedCount { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateOut { get; set; }
        
    }
}


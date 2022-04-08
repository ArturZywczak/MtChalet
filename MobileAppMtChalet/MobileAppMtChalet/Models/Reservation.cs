using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public string EmployeeId { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class ReservationsByRoom {

        public int FreeBeds { get; set; }
        public Room Room { get; set; }

        public List<Reservation> Reservations { get; set; }

        public ReservationsByRoom() {
            this.Reservations = new List<Reservation>();
        }
    }

    public class Grouping<K, T> : ObservableCollection<T> {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items) {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }

    public partial class EditedReservation : Reservation {
        public int ReservationEditId { get; set; }
        public int OldReservationId { get; set; }
        public DateTime EditDate { get; set; }
        public string EditedByEmployeeId { get; set; }
        public int NewReservationId { get; set; }
    }

    public class EditHistoryDetail {

        public string UserName { get; set; }
        public string ChangedType { get; set; }
        public string ChangedBefore { get; set; }
        public string ChangedAfter { get; set; }

        public DateTime Date { get; set; }

    }
}


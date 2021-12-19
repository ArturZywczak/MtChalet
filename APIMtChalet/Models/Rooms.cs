using System;
using System.Collections.Generic;

namespace APIMtChalet.Models {
    public partial class Rooms
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCap { get; set; }
        public bool HasBathroom { get; set; }
    }

    public partial class RoomsInfo {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCap { get; set; }
        public bool HasBathroom { get; set; }
        public int OcuppiedBeds { get; set; }
    }
}

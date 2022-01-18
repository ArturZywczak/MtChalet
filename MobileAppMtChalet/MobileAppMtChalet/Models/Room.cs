using System;
using System.Collections.Generic;

namespace MobileAppMtChalet.Models {
    public partial class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCap { get; set; }
        public bool HasBathroom { get; set; }

    }
}

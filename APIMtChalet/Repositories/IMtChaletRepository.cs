﻿using APIMtChalet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Repositories {
    public interface IMtChaletRepository {

        //RESERVATIONS
        Task<IEnumerable<Reservation>> GetReservation(DateTime date);
        Task<Reservation> GetReservation(int id);
        Task<Reservation> CreateReservation(Reservation reservation);
        Task UpdateReservation(Reservation reservation);
        Task DeleteReservation(int id);

        //ROOMS
        Task<IEnumerable<Room>> GetRooms();
    }
}

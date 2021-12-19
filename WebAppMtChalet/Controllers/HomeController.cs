using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppMtChalet.Models;

namespace WebAppMtChalet.Controllers {
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller {
        private readonly ILogger<HomeController> _logger;
        public static ReservationTemp _basket = new ReservationTemp();
        private readonly MtChaletDBContext _context;

        public HomeController(ILogger<HomeController> logger, MtChaletDBContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            RecordInSession();
            return View();
        }
        private void RecordInSession() {

            HttpContext.Session.SetString("actions", HttpContext.Session.Id);


        }

        public IActionResult Privacy() {

            var rooms = _context.Rooms.ToList();
            var result = new List<RoomsInfo>();
            for (int i = 0; i < rooms.Count; i++) {
                int temp = 0;

                result.Add(new RoomsInfo {
                    RoomId = rooms[i].RoomId,
                    RoomNumber = rooms[i].RoomNumber,
                    RoomCap = rooms[i].RoomCap,
                    HasBathroom = rooms[i].HasBathroom,
                    OcuppiedBeds = temp
                });
            }

            return View(result);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult ajaxTest(DateTime ammount) {
            var reservations = _context.Reservations.Where(s => s.StartingDate >= ammount && s.EndingDate <= ammount).ToList();
            var rooms = _context.Rooms.ToList();

            var result = new List<RoomsInfo>();
            for (int i = 0; i < rooms.Count; i++) {
                int temp = 0;
                var item = reservations.FindAll(o => o.RoomId == i + 1).ToList();
                foreach (var x in item) temp += x.NumberOfPeople;

                result.Add(new RoomsInfo {
                    RoomId = rooms[i].RoomId,
                    RoomNumber = rooms[i].RoomNumber,
                    RoomCap = rooms[i].RoomCap,
                    HasBathroom = rooms[i].HasBathroom,
                    OcuppiedBeds = temp
                });
            }


            return PartialView("Privacy2", result);

        }

        public Microsoft.AspNetCore.Mvc.ActionResult ajaxTest2(string elemId, DateTime date, bool checboxState) {

            //DateTime date = new DateTime(2020, 10, 10);
            if (elemId == null) return PartialView("ResBox", _basket);
            int roomID = int.Parse(elemId.Remove(0, 4));
            int bedID = roomID % 20;
            roomID = (roomID - bedID) / 20;
            ReservationTempSingle temp = new ReservationTempSingle {
                BedCount = 1,
                RoomID = roomID,
                Date = date,
                DateOut = date.AddDays(1)
            };
            var index = _basket.BedsList.FindIndex(c => c.RoomID == roomID && c.Date == date);

            if (checboxState) {

                if (index != -1) _basket.BedsList[index].BedCount += 1;
                else _basket.BedsList.Add(temp);
            }
            else {

                if (_basket.BedsList[index].BedCount == 1) _basket.BedsList.RemoveAt(index);
                else _basket.BedsList[index].BedCount -= 1;
            }
            return PartialView("ResBox", _basket);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult ajaxTest3(string elemId, DateTime date, bool checboxState) {

            return View("Confirm");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

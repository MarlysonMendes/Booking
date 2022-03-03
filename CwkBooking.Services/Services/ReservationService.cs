using CwkBooking.Dal;
using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkBooking.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IHotelsRepository _hotelRepository;
        private readonly DataContext _ctx;

        public ReservationService(IHotelsRepository hotelRepo, DataContext ctx)
        {
            _hotelRepository = hotelRepo;
            _ctx = ctx;
        }

        public async Task<Reservation> MakeReservation(Reservation reservation)
        {
            
            //Step 1: Get the hotel, including all rooms
            var hotel = await _hotelRepository.GetHotelByIdAsync(reservation.HotelId);

            //Step 2: Find the specified room
            var room = hotel.Rooms.Where(r => r.RoomId == reservation.RoomId).FirstOrDefault();

            //Step 3: Make sure the room is available
            var roomBusyFrom = room.BusyFrom == null ? default(DateTime) : room.BusyFrom;
            var roomBusyTo = room.BusyTo == null ? default(DateTime) : room.BusyTo;
            var isBusy = reservation.CheckInDate >= room.BusyFrom.Value
                || reservation.CheckInDate <= room.BusyTo.Value;
            if (isBusy)
                return null;
            if (room.NeedsRepair)
                return null;

            //Step 4: Set busyfrom and busyto on the room
            room.BusyFrom = reservation.CheckInDate;
            room.BusyTo = reservation.CheckoutDate;

            //Step 5: Persist all changes to the database
            _ctx.Rooms.Update(room);
            _ctx.Reservations.Add(reservation);

            await _ctx.SaveChangesAsync();

            return reservation;
            throw new NotImplementedException();
        }
    }
}

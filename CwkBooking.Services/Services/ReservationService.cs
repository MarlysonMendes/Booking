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

        public async Task<Reservation> MakeReservation(int hotelId, int roomId, DateTime checkIn, DateTime checkout, string custumer)
        {
            //Step 1: Create a reservation instance
            var reservation = new Reservation
            {
                HotelId = hotelId,
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckoutDate = checkout,
                Customer = custumer
            };

            //Step 2: Get the hotel, including all rooms
            var hotel = await _hotelRepository.GetHotelByIdAsync(reservation.HotelId);

            //Step 3: Find the specified room
            var room = hotel.Rooms.Where(r => r.RoomId == reservation.RoomId).FirstOrDefault();

            //Step 4: Make sure the room is available
            var isBusy = reservation.CheckInDate >= room.BusyFrom.Value
                && reservation.CheckInDate <= room.BusyTo.Value;
            if (isBusy && room.NeedsRepair)
                return null;

            //Step 5: Set busyfrom and busyto on the room
            room.BusyFrom = reservation.CheckInDate;
            room.BusyTo = reservation.CheckoutDate;

            //Step 6: Persist all changes to the database
            _ctx.Rooms.Update(room);
            _ctx.Reservations.Add(reservation);

            await _ctx.SaveChangesAsync();

            return reservation;
            throw new NotImplementedException();
        }
    }
}

using CwkBooking.Dal;
using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRoomsRepository _roomsRepository;

        public ReservationService(IHotelsRepository hotelRepo, IRoomsRepository roomRepo, DataContext ctx)
        {
            _hotelRepository = hotelRepo;
            _ctx = ctx;
            _roomsRepository = roomRepo;
        }

        public async Task<Reservation> MakeReservation(Reservation reservation)
        {
            
            //Step 1: Get the hotel, including all rooms
            var hotel = await _hotelRepository.GetHotelByIdAsync(reservation.HotelId);

            //Step 2: Find the specified room
            var room = await _roomsRepository.GetHotelRoomByIdAsync(reservation.HotelId, reservation.RoomId);

            if (hotel == null || room == null) return null;

            //Step 3: Make sure the room is available
            bool isBusy = await _ctx.Reservations.AnyAsync(r =>
                (reservation.CheckInDate >= r.CheckInDate && reservation.CheckInDate <= r.CheckoutDate)
                && (reservation.CheckoutDate >= r.CheckInDate && reservation.CheckoutDate <= r.CheckoutDate)
            );

            if (isBusy)
                return null;
            if (room.NeedsRepair)
                return null;


            //Step 4: Persist all changes to the database
            await _roomsRepository.UpdateHotelRoomAsync(reservation.HotelId, room);
            _ctx.Reservations.Add(reservation);

            await _ctx.SaveChangesAsync();

            return reservation;
        }
    }
}

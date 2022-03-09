using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkBooking.Dal.Repositories
{
    public class RoomRepository : IRoomsRepository
    {
        private readonly DataContext _ctx;

        public RoomRepository(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Room> CreateHotelRoomAsync(int hotelId, Room room)
        {
            var hotel = await _ctx.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            hotel.Rooms.Add(room);
            
            await _ctx.SaveChangesAsync();
            return room;
        }

        public async Task<Room> DeleteHotelRoomAsync(int hotelId, int roomId)
        {
            var room = await _ctx.Rooms.SingleOrDefaultAsync(r => r.RoomId == roomId && r.HotelId == hotelId);

            if (room == null)
                return null;

            _ctx.Rooms.Remove(room);
            await _ctx.SaveChangesAsync();

            return room;
        }

        public async Task<Room> GetHotelRoomByIdAsync(int hotelId, int roomId)
        {
            var room = await _ctx.Rooms.FirstOrDefaultAsync(r => r.HotelId == hotelId && r.RoomId == roomId);
            if (room == null)
                return null;

            return room;
        }

        public async Task<List<Room>> ListHotelRoomsAsync(int hotelId)
        {
            var rooms = await _ctx.Rooms.Where(r => r.HotelId ==hotelId).ToListAsync();
            return rooms;
        }

        public async Task<Room> UpdateHotelRoomAsync(int hotelId, Room updatedRoom)
        {
            var room = _ctx.Rooms.Update(updatedRoom);
            await _ctx.SaveChangesAsync();
            return updatedRoom;
        }
    }
}

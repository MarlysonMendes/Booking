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
    public class ReservationRepository : IReservationsRepository
    {
        private readonly DataContext _ctx;
        public ReservationRepository (DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Reservation> DeleteReservationAsync(int reservationId)
        {
            var reservation = await _ctx.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (reservation == null) return null;
            
            _ctx.Reservations.Remove(reservation);
            await _ctx.SaveChangesAsync();
            return reservation;

        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var reservations = await _ctx.Reservations.Include(r => r.Hotel).Include(r => r.Room).ToListAsync();

            return reservations;
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _ctx.Reservations
                .Include(r => r.Hotel)
                .Include(r =>r.Room)
                .FirstOrDefaultAsync(r =>r.ReservationId == reservationId);
           
            if(reservation == null) return null;
            
            return reservation;
        }
    }
}

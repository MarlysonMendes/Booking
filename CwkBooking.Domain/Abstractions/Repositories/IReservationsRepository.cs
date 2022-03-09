using CwkBooking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkBooking.Domain.Abstractions.Repositories
{
    public interface IReservationsRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task<Reservation> DeleteReservationAsync(int reservationId);
    }
}

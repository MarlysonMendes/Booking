using CwkBooking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkBooking.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Reservation> MakeReservation(Reservation reservation);
       

    }
}

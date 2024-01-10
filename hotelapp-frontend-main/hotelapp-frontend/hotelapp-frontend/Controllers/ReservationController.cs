using hotelapp_frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hotelapp_frontend.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HotelAppContext _context;

        public ReservationController(HotelAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ReservacionesAdmin()
        {
            return View(await _context.Reservaciones.ToListAsync());
        }


        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitacion.ToListAsync();

            foreach (var habitacion in habitaciones)
            {
                var reservacionExistente = await _context.Reservaciones.AnyAsync(r => r.IDHabitacion == habitacion.IDHabitacion);
                habitacion.Disponible = !reservacionExistente;
            }

            return View(habitaciones);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservacion = await _context.Habitacion.FindAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }



        [HttpGet]
        public async Task<IActionResult> Eliminar(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var reservacion = await _context.Reservaciones.FindAsync(id);
                if (reservacion == null)
                {
                    return NotFound();
                }
                _context.Reservaciones.Remove(reservacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReservacionesAdmin));


            }
            catch
            {
                return View();
            }
        } 
        

        private bool ReservacionExists(int id)
        {
            return _context.Reservaciones.Any(e => e.IDReservacion == id);
        }

        [HttpGet]
        public IActionResult ReservaHabitacion(int id)
        {
            var habitacion = _context.Habitacion.Find(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }
        public IActionResult ConfirmacionReserva(int id)
        {
            var habitacion = _context.Habitacion.Find(id);
            if (habitacion == null)
            {
                return NotFound();
            }
            
            var nuevaReserva = new Reservaciones
            {
                IDHabitacion = habitacion.IDHabitacion,
                IDUsuario = 2, // Cambiar esto al ID real del usuario
                CantidadNoches = 3,
                FechaInicio = DateTime.Now, // Cambiar esto a la fecha real de inicio
                FechaFin = DateTime.Now.AddDays(3), // Cambiar esto a la fecha real de fin
                Costo_Total = habitacion.CostoNoche * 3 // Cambiar esto al costo total real
            };

            return View(nuevaReserva);
        }


        [HttpPost]
        public IActionResult Reservar(int IDHabitacion, DateTime FechaInicio, DateTime FechaFin, int CantidadNoches, decimal CostoTotal)
        {

            var habitacion = _context.Habitacion.Find(IDHabitacion);
            if (habitacion == null)
            {
                return NotFound();
            }


            decimal CostoTotalCalculado = CantidadNoches * habitacion.CostoNoche;


            var nuevaReserva = new Reservaciones
            {
                IDHabitacion = IDHabitacion,
                IDUsuario = 1,
                CantidadNoches = CantidadNoches,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                Costo_Total = CostoTotalCalculado
            };


            _context.Reservaciones.Add(nuevaReserva);
            _context.SaveChanges();


            return RedirectToAction("ConfirmacionReserva", new { id = IDHabitacion });
        }

        public IActionResult DetalleAdmin(int id)
        {
            var reservacion = _context.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        [HttpGet]

        public async Task<IActionResult> EditarAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        [HttpPost]

        public async Task<IActionResult> EditarAdmin(Reservaciones reservacion)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Reservaciones.Update(reservacion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ReservacionesAdmin));
                }
                catch {
                    return View(reservacion);
                }
                }
            else
            {
                return View(reservacion);
            }
        }
       
}
}
    


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotelapp_frontend.Models;

namespace hotelapp_frontend.Controllers
{
    public class RoomController : Controller
    {
        private readonly HotelAppContext _context;

        public RoomController(HotelAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Habitacion.ToListAsync());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Habitacion habitacion)
        {
            habitacion.Disponible = true;

            _context.Habitacion.Add(habitacion);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Habitacion habitacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Habitacion.Update(habitacion);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(habitacion);
                }
            }
            catch
            {
                return View(habitacion);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var habitacion = await _context.Habitacion.FindAsync(id);
                if (habitacion == null)
                {
                    return NotFound();
                }

                _context.Habitacion.Remove(habitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ortamı ortadan kaldırma hatası: " + ex.Message);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reservar()
        {
            return View(await _context.Habitacion.ToListAsync());
        }

    }
}

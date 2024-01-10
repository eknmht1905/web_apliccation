using hotelapp_frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Configuration;


namespace hotelapp_frontend.Controllers
{
    public class UserController : Controller
    {
        private readonly HotelAppContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public UserController(HotelAppContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;on;
        }
            _configuration = configurati

        // Trabajar Metodos de Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
           
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
            {
                ModelState.AddModelError(string.Empty, "Lütfen e-postanızı ve şifrenizi girin.");
                return View();
            }
       
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Correo == correo);
        
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Girilen e-posta kayıtlı değil.");
                return View();
            }

            if (usuario.Contrasena != contrasena)
            {
                ModelState.AddModelError(string.Empty, "Yanlış Şifre");
                return View();
            }

            _contextAccessor.HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            _contextAccessor.HttpContext.Session.SetInt32("RolUsuario", usuario.IDRol);
            _contextAccessor.HttpContext.Session.SetInt32("IdUsuario", usuario.IDUsuario);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignOut()
        {
            _contextAccessor.HttpContext.Session.Clear(); // Clear all session variables
            return RedirectToAction("Index", "Home"); // Redirect to home page after sign-out
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            Usuario usuario = new Usuario
            {
                IDRol = 2 
            };

            ViewBag.RolesList = new SelectList(_context.Roles, "IDRol", "NombreRol");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario.Estado = true;
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                    int? loggedInUserRole = HttpContext.Session.GetInt32("RolUsuario");

                    if (loggedInUserRole.HasValue && loggedInUserRole.Value == 1)
                    {
                        // Admin user, redirect to User/Index
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Regular user, redirect to home page
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al crear el usuario: " + ex.Message);
            }

            ViewBag.RolesList = new SelectList(_context.Roles, "IDRol", "NombreRol");
            return View(usuario);
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(PasswordReset model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Usuarios.SingleOrDefault(u => u.Correo == model.Email);
                if (user != null)
                {
                    // Implement your logic here to generate a new password and update the user's password
                    // For example:
                    string newPassword = GenerateNewPassword();
                    user.Contrasena = newPassword;
                    _context.SaveChanges();

                    SendPasswordResetEmail(user.Correo, newPassword);

                    TempData["ResetPasswordSuccess"] = "Şifreniz sıfırlandı. Lütfen yeni şifre için e-postanızı kontrol edin.";
                    return RedirectToAction("Login");
                }
                TempData["ResetPasswordError"] = "Girilen e-posta kayıtlı değil.";
                return RedirectToAction("ForgotPassword");
            }

            TempData["ResetPasswordError"] = "Lütfen E-postanızı girin.";
            return RedirectToAction("ForgotPassword");
        }

        private void SendPasswordResetEmail(string recipientEmail, string newPassword)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpClient = new SmtpClient
            {
                Host = emailSettings["SmtpServer"],
                Port = int.Parse(emailSettings["Port"]),
                Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
                Subject = "Password Reset",
                Body = $"Your new password: {newPassword}"
            };

            mailMessage.To.Add(recipientEmail);

            smtpClient.Send(mailMessage);
        }

        private string GenerateNewPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int passwordLength = 10;

            StringBuilder newPassword = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(validChars.Length);
                newPassword.Append(validChars[index]);
            }

            return newPassword.ToString();
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.RolesList = new SelectList(_context.Roles, "IDRol", "NombreRol");
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Attach(usuario).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar los cambios: " + ex.Message);
            }

            ViewBag.RolesList = new SelectList(_context.Roles, "IDRol", "NombreRol");
            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FindAsync(usuario.IDRol);
            ViewBag.NombreRol = rol?.NombreRol;

            return View(usuario);
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

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcıyı ortadan kaldırırken hata oluştu:" + ex.Message);
            }

            return View();
        }
    }
}
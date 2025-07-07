using Crud.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crud.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Web;

namespace Crud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly FacturacionContext _dbContext;
        public UsuariosController(FacturacionContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<IActionResult> Guardar(UsuarioDTO usuario)
        {
            var responseApi = new ResponseAPI<int>();
            try
            {
                var buscar = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Cedula == usuario.Cedula || x.Email==usuario.Email);
                
                if (buscar==null)
                {
                    // Encriptar la contraseña
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

                    var dbUsuario = new Usuario
                    {
                        Cedula = usuario.Cedula,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Direccion = usuario.Direccion,
                        Password = usuario.Password,
                        Email = usuario.Email,
                        Telefono = usuario.Telefono,
                        FechaNacimiento = usuario.FechaNacimiento,
                        Activo = usuario.Activo,
                        IdRolPer = usuario.RolID
                    };

                    _dbContext.Usuarios.Add(dbUsuario);
                    await _dbContext.SaveChangesAsync();
                    if (dbUsuario.UsuarioId != 0)
                    {
                        responseApi.EsCorrecto = true;
                        responseApi.Valor = dbUsuario.UsuarioId;
                    }
                    else
                    {
                        responseApi.EsCorrecto = false;
                        responseApi.Mensaje = "No Creado";
                    }
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "El correo o la cedula ya está registrado";

                    var error = new ErrorLog();
                    error.Tabla = "Usuarios";
                    error.Error = responseApi.Mensaje;
                    error.FechaHora = DateTime.Now;
                    _dbContext.ErrorLogs.Add(error);
                    await _dbContext.SaveChangesAsync();                    
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpPost]
        [Route("ActualizarUsuario")]
        public async Task<IActionResult> Actualizar(UsuarioDTO usuario)
        {
            var responseApi = new ResponseAPI<bool>();
            try
            {
                // Buscar el usuario existente por UsuarioID
                var buscarPorId = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuario.UsuarioID);

                if (buscarPorId != null)
                {
                    // Verificar si el correo electrónico ya está registrado en otro usuario
                    var otroUsuarioConMismoEmail = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId != usuario.UsuarioID && x.Cedula == usuario.Cedula);

                    if (otroUsuarioConMismoEmail == null)
                    {
                        // Actualizar los datos del usuario existente
                        buscarPorId.Nombre = usuario.Nombre;
                        buscarPorId.Apellido = usuario.Apellido;
                        buscarPorId.Password = usuario.Password;
                        buscarPorId.Direccion = usuario.Direccion;
                        buscarPorId.Activo = usuario.Activo;
                        buscarPorId.Email = usuario.Email;
                        buscarPorId.Cedula = usuario.Cedula;
                        buscarPorId.Telefono = usuario.Telefono;

                        await _dbContext.SaveChangesAsync();

                        responseApi.EsCorrecto = true;
                        responseApi.Valor = true;
                    }
                    else
                    {
                        responseApi.EsCorrecto = false;
                        responseApi.Mensaje = "El correo electrónico ya está registrado con otro usuario.";
                        var error = new ErrorLog();
                        error.Tabla = "Usuarios";
                        error.Error = responseApi.Mensaje;
                        error.FechaHora = DateTime.Now;
                        _dbContext.ErrorLogs.Add(error);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Usuario no encontrado para actualizar.";
                    var error = new ErrorLog();
                    error.Tabla = "Usuarios";
                    error.Error = responseApi.Mensaje;
                    error.FechaHora = DateTime.Now;
                    _dbContext.ErrorLogs.Add(error);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                // Manejo de errores
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> ActualizarConFoto(UsuarioDTO usuario)
        {
            var responseApi = new ResponseAPI<bool>();
            try
            {
                // Buscar el usuario existente por UsuarioID
                var buscarPorId = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuario.UsuarioID);

                if (buscarPorId != null)
                {
                    // Verificar si el correo electrónico ya está registrado en otro usuario
                    var otroUsuarioConMismoEmail = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId != usuario.UsuarioID && x.Cedula == usuario.Cedula);

                    if (otroUsuarioConMismoEmail == null)
                    {
                        // Actualizar los datos del usuario existente
                        buscarPorId.Nombre = usuario.Nombre;
                        buscarPorId.Apellido = usuario.Apellido;
                        buscarPorId.Password = usuario.Password;
                        buscarPorId.Direccion = usuario.Direccion;
                        buscarPorId.Activo = usuario.Activo;
                        buscarPorId.Email = usuario.Email;
                        buscarPorId.Cedula = usuario.Cedula;
                        buscarPorId.Telefono = usuario.Telefono;
                        buscarPorId.Foto = usuario.Foto;

                        await _dbContext.SaveChangesAsync();

                        responseApi.EsCorrecto = true;
                        responseApi.Valor = true;
                    }
                    else
                    {
                        responseApi.EsCorrecto = false;
                        responseApi.Mensaje = "El correo electrónico ya está registrado con otro usuario.";
                    }
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Usuario no encontrado para actualizar.";
                }

            }
            catch (Exception ex)
            {
                // Manejo de errores
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }


        [HttpPost]
        [Route("ActualizarEstado")]
        public async Task<IActionResult> ActualizarEstado(UsuarioDTO usuario)
        {
            var responseApi = new ResponseAPI<bool>();
            try
            {
                var buscar = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuario.UsuarioID || x.Email == usuario.Email);

                if (buscar != null)
                {
                    buscar.Activo = usuario.Activo;

                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = true;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo actualizar el estado";
                    var error = new ErrorLog();
                    error.Tabla = "Usuarios";
                    error.Error = "No se pudo actualizar el estado del usuario";
                    error.FechaHora = DateTime.Now;
                    _dbContext.ErrorLogs.Add(error);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpDelete]
        [Route("EliminarUsuario{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var responseApi = new ResponseAPI<bool>();
            try
            {
                var buscar = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);

                if (buscar != null)
                {
                    _dbContext.Usuarios.Remove(buscar);
                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = true;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo eliminar al usuario";
                }

            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ListarUsuariosActivos")]
        public async Task<IActionResult> ListarUsuariosActivos()
        {
            var responseApi = new ResponseAPI<List<UsuarioDTO>>();
            var listaCli = new List<UsuarioDTO>();
            var listaUsers = await _dbContext.Usuarios.Where(p => p.Activo == true).ToArrayAsync();

            try
            {
                foreach (var usuDB in listaUsers)
                {

                    listaCli.Add(new UsuarioDTO
                    {
                        UsuarioID = usuDB.UsuarioId,
                        Cedula = usuDB.Cedula,
                        Nombre = usuDB.Nombre,
                        Apellido= usuDB.Apellido,
                        Activo = usuDB.Activo,
                        Direccion = usuDB.Direccion,
                        Email = usuDB.Email,
                        Password = usuDB.Password,
                        Telefono = usuDB.Telefono,
                        FechaNacimiento = usuDB.FechaNacimiento,
                        RolID = usuDB.IdRolPer
                    });
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listaCli;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ListarTodosUsuarios")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            var responseApi = new ResponseAPI<List<UsuarioDTO>>();
            var listaCli = new List<UsuarioDTO>();
            var listaUsers = await _dbContext.Usuarios.ToArrayAsync();

            try
            {
                foreach (var usuDB in listaUsers)
                {

                    listaCli.Add(new UsuarioDTO
                    {
                        UsuarioID = usuDB.UsuarioId,
                        Cedula = usuDB.Cedula,
                        Nombre = usuDB.Nombre,
                        Apellido = usuDB.Apellido,
                        Activo = usuDB.Activo,
                        Direccion = usuDB.Direccion,
                        Email = usuDB.Email,
                        Password = usuDB.Password,
                        Telefono = usuDB.Telefono,
                        FechaNacimiento = usuDB.FechaNacimiento,
                        RolID = usuDB.IdRolPer
                    });
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listaCli;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ObtenerUsuarioxCedula{cedula}")]
        public async Task<IActionResult> ObtenerUsuarioxCedula(string cedula)
        {
            var responseApi = new ResponseAPI<UsuarioDTO>();
            var usuario = new UsuarioDTO();
            var userDB = await _dbContext.Usuarios.Where(p => p.Activo == true && p.Cedula==cedula).FirstOrDefaultAsync();

            try
            {
                usuario.UsuarioID = userDB.UsuarioId;
                usuario.Cedula = userDB.Cedula;
                usuario.Nombre = userDB.Nombre;
                usuario.Apellido = userDB.Apellido;
                usuario.Activo = userDB.Activo;
                usuario.Direccion = userDB.Direccion;
                usuario.Email = userDB.Email;
                usuario.Password = userDB.Password;
                usuario.Telefono = userDB.Telefono;
                usuario.FechaNacimiento = userDB.FechaNacimiento;
                usuario.RolID = userDB.IdRolPer;
                responseApi.EsCorrecto = true;
                responseApi.Valor = usuario;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ObtenerFotoUsuarioxEmail{email}")]
        public async Task<IActionResult> ObtenerFotoxEmail(string email)
        {
            var responseApi = new ResponseAPI<UsuarioDTO>();
            var usuario = new UsuarioDTO();
            var userDB = await _dbContext.Usuarios.Where(p => p.Activo == true && p.Email == email).FirstOrDefaultAsync();

            try
            {
                usuario.UsuarioID = userDB.UsuarioId;
                usuario.Cedula = userDB.Cedula;
                usuario.Nombre = userDB.Nombre;
                usuario.Apellido = userDB.Apellido;
                usuario.Activo = userDB.Activo;
                usuario.Direccion = userDB.Direccion;
                usuario.Email = userDB.Email;
                usuario.Password = userDB.Password;
                usuario.Telefono = userDB.Telefono;
                usuario.FechaNacimiento = userDB.FechaNacimiento;
                usuario.RolID = userDB.IdRolPer;
                usuario.Foto = userDB.Foto;
                responseApi.EsCorrecto = true;
                responseApi.Valor = usuario;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ObtenerUsuarioxEmail{email}")]
        public async Task<IActionResult> ObtenerUsuarioxEmail(string email)
        {
            var responseApi = new ResponseAPI<UsuarioDTO>();
            var usuario = new UsuarioDTO();
            var userDB = await _dbContext.Usuarios.Where(p => p.Activo == true && p.Email == email).FirstOrDefaultAsync();

            try
            {
                usuario.UsuarioID = userDB.UsuarioId;
                usuario.Cedula = userDB.Cedula;
                usuario.Nombre = userDB.Nombre;
                usuario.Apellido = userDB.Apellido;
                usuario.Activo = userDB.Activo;
                usuario.Direccion = userDB.Direccion;
                usuario.Email = userDB.Email;
                usuario.Password = userDB.Password;
                usuario.Telefono = userDB.Telefono;
                usuario.FechaNacimiento = userDB.FechaNacimiento;
                usuario.RolID = userDB.IdRolPer;
                responseApi.EsCorrecto = true;
                responseApi.Valor = usuario;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Usuarios";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }
    }
}

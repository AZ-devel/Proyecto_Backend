using Crud.Server.Models;
using Crud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Error_Logs : ControllerBase
    {
        private readonly FacturacionContext _dbContext;
        public Error_Logs(FacturacionContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("ListarLogs")]
        public async Task<IActionResult> ListarLogs()
        {
            var responseApi = new ResponseAPI<List<ErrorLog>>();
            var listaErrors = new List<ErrorLog>();
            var listaErrDB = await _dbContext.ErrorLogs.ToArrayAsync();

            try
            {
                foreach (var error in listaErrDB)
                {
                    var errLog = new ErrorLog();
                    errLog.Tabla = error.Tabla;
                    errLog.Error = error.Error;
                    errLog.FechaHora = error.FechaHora;
                    listaErrors.Add(errLog);
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listaErrors;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Error_Logs";
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

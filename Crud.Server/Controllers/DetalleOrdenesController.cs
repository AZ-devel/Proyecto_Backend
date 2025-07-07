using Crud.Server.Models;
using Crud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Crud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleOrdenesController : ControllerBase
    {
        private readonly FacturacionContext _dbContext;
        public DetalleOrdenesController(FacturacionContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("ListaDetallesVentas")]
        public async Task<IActionResult> ListaDetallesOrdenVenta()
        {
            var responseApi = new ResponseAPI<List<DetalleOrdenDTO>>();
            var listDOV = new List<DetalleOrdenDTO>();
            var listaDOVBD = await _dbContext.DetalleOrdens.ToArrayAsync();
            try
            {
                foreach (var dovBD in listaDOVBD)
                {
                    listDOV.Add(new DetalleOrdenDTO
                    {
                        DetalleID = dovBD.DetalleId,
                        OrdenID = dovBD.OrdenId,
                        ProductoID = dovBD.ProductoId,
                        Cantidad = dovBD.Cantidad,
                        PrecioUnitario = dovBD.PrecioUnitario,
                        Subtotal = dovBD.Subtotal
                    });
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listDOV;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "DetalleVentas";
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
        [Route("ListaDetallesOrdenVentaPorOV{idOV}")]
        public async Task<IActionResult> ListaDetallesOrdenVenta(int idOV)
        {
            var responseApi = new ResponseAPI<List<DetalleOrdenDTO>>();
            var listDOV = new List<DetalleOrdenDTO>();
            var listaDOVBD = await _dbContext.DetalleOrdens.Where(x=>x.OrdenId==idOV).ToArrayAsync();
            try
            {
                if (listaDOVBD.Length>0)
                {
                    foreach (var dovBD in listaDOVBD)
                    {
                        listDOV.Add(new DetalleOrdenDTO
                        {
                            DetalleID = dovBD.DetalleId,
                            OrdenID = dovBD.OrdenId,
                            ProductoID = dovBD.ProductoId,
                            Cantidad = dovBD.Cantidad,
                            PrecioUnitario = dovBD.PrecioUnitario,
                            Subtotal = dovBD.Subtotal
                        });
                    }
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = listDOV;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Valor = null;
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "DetalleVentas";
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
        [Route("GuardarDetalleOrden")]
        public async Task<IActionResult> GuardarDetalleOrden(List<DetalleOrdenDTO> detalles)
        {
            var responseApi = new ResponseAPI<bool>();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in detalles)
                    {
                        var dbdetalle = new DetalleOrden
                        {
                            OrdenId = item.OrdenID,
                            ProductoId = item.ProductoID,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.PrecioUnitario,
                            Subtotal = item.Subtotal
                        };
                        _dbContext.DetalleOrdens.Add(dbdetalle);
                        await _dbContext.SaveChangesAsync();

                        if (dbdetalle.OrdenId != 0)
                        {
                            responseApi.EsCorrecto = true;
                            responseApi.Valor = true;
                        }
                        else
                        {
                            responseApi.EsCorrecto = false;
                            responseApi.Mensaje = "No se pudo guardar el detalle";
                        }
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    var error = new ErrorLog();
                    error.Tabla = "DetalleVentas";
                    error.Error = ex.Message;
                    error.FechaHora = DateTime.Now;
                    _dbContext.ErrorLogs.Add(error);
                    await _dbContext.SaveChangesAsync();
                    var listaproductos = _dbContext.Productos.ToList();
                    foreach (var item in detalles)
                    {
                        var prodBus = listaproductos.FirstOrDefault(x=>x.ProductoId==item.ProductoID);
                        var productoUpd = new Producto();
                        productoUpd.ProductoId = prodBus.ProductoId;
                        productoUpd.Nombre = prodBus.Nombre;
                        productoUpd.Precio = prodBus.Precio;
                        productoUpd.Stock = prodBus.Stock + item.Cantidad;
                        _dbContext.Productos.Update(productoUpd);
                        await _dbContext.SaveChangesAsync();
                    }
                    await transaction.RollbackAsync();
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = ex.Message;
                }
            }

            return Ok(responseApi);
        }

    }
}

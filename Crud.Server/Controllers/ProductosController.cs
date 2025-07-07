using Crud.Server.Models;
using Crud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Crud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly FacturacionContext _dbContext;
        public ProductosController(FacturacionContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("ListaProductos")]
        public async Task<IActionResult> ListaProductos()
        {
            var responseApi = new ResponseAPI<List<ProductoDTO>>();
            var listaProductos = new List<ProductoDTO>();
            
            var productosBD = await _dbContext.Productos.Where(p => p.Descontinuado==false).ToArrayAsync();

            try
            {
                foreach (var proBD in productosBD)
                {
                    var listaImgs = new List<ImgProdDTO>();
                    var imgsBD = await _dbContext.ImgsProductos.Where(i => i.IdProPer == proBD.ProductoId).ToArrayAsync();
                    foreach (var img in imgsBD)
                    {
                        listaImgs.Add(new ImgProdDTO
                        {
                            IdImagen = img.IdImagen,
                            Imagen = img.Imagen,
                            IdProPer = img.IdProPer
                        });
                    }

                    listaProductos.Add(new ProductoDTO
                    {
                        IdProducto = proBD.ProductoId,
                        Nombre = proBD.Nombre,
                        Precio = proBD.Precio,
                        Stock = proBD.Stock,
                        Descontinuado = proBD.Descontinuado,
                        listaImgs = listaImgs
                    });
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listaProductos;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
                error.Error=ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet]
        [Route("ListaProductosConUImg")]
        public async Task<IActionResult> ListaProductosConUImg()
        {
            var responseApi = new ResponseAPI<List<ProductoDTO>>();
            var listaProductos = new List<ProductoDTO>();
            var productosBD = await _dbContext.Productos.Where(p => p.Descontinuado == false).ToArrayAsync();

            try
            {
                foreach (var proBD in productosBD)
                {
                    var listaImgs = new List<ImgProdDTO>();
                    var imgsBD = await _dbContext.ImgsProductos.Where(i => i.IdProPer == proBD.ProductoId).FirstOrDefaultAsync();
                    if (imgsBD!=null)
                    {
                        var img = new ImgProdDTO();
                        img.IdImagen = imgsBD.IdImagen;
                        img.Imagen = imgsBD.Imagen;
                        img.IdProPer = imgsBD.IdProPer;
                        listaImgs.Add(img);
                    }

                    listaProductos.Add(new ProductoDTO
                    {
                        IdProducto = proBD.ProductoId,
                        Nombre = proBD.Nombre,
                        Precio = proBD.Precio,
                        Stock = proBD.Stock,
                        Descontinuado = proBD.Descontinuado,
                        listaImgs = listaImgs
                    });
                }
                responseApi.EsCorrecto = true;
                responseApi.Valor = listaProductos;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
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
        [Route("DevolverProductoxID{idP}")]
        public async Task<IActionResult> DevolverProductoxNombreUImg(int idP)
        {
            var responseApi = new ResponseAPI<ProductoDTO>();
            var productoBD = await _dbContext.Productos.Where(p => p.Descontinuado == false && p.ProductoId == idP).FirstOrDefaultAsync();

            try
            {
                var listaImgs = new List<ImgProdDTO>();
                var imgsBD = await _dbContext.ImgsProductos
                    .Where(i => i.IdProPer == productoBD.ProductoId)
                    .ToListAsync();

                foreach (var imgBD in imgsBD)
                {
                    var img = new ImgProdDTO
                    {
                        IdImagen = imgBD.IdImagen,
                        Imagen = imgBD.Imagen,
                        IdProPer = imgBD.IdProPer
                    };
                    listaImgs.Add(img);
                }

                var producto = new ProductoDTO
                {
                    IdProducto = productoBD.ProductoId,
                    Nombre = productoBD.Nombre,
                    Precio = productoBD.Precio,
                    Stock = productoBD.Stock,
                    Descontinuado = productoBD.Descontinuado,
                    listaImgs = listaImgs
                };

                responseApi.EsCorrecto = true;
                responseApi.Valor = producto;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
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
        [Route("DevolverProductosxNombre{nombre}")]
        public async Task<IActionResult> DevolverProductoxNombreUImg(string nombre)
        {
            var responseApi = new ResponseAPI<List<ProductoDTO>>();
            var productos = new List<ProductoDTO>();

            try
            {
                var productosBD = await _dbContext.Productos
                    .Where(p => p.Descontinuado == false && p.Nombre.Contains(nombre))
                    .ToListAsync();

                foreach (var productoBD in productosBD)
                {
                    var listaImgs = new List<ImgProdDTO>();
                    var imgBD = await _dbContext.ImgsProductos
                        .Where(i => i.IdProPer == productoBD.ProductoId)
                        .FirstOrDefaultAsync();

                    if (imgBD != null)
                    {
                        var img = new ImgProdDTO
                        {
                            IdImagen = imgBD.IdImagen,
                            Imagen = imgBD.Imagen,
                            IdProPer = imgBD.IdProPer
                        };
                        listaImgs.Add(img);
                    }

                    var producto = new ProductoDTO
                    {
                        IdProducto = productoBD.ProductoId,
                        Nombre = productoBD.Nombre,
                        Precio = productoBD.Precio,
                        Stock = productoBD.Stock,
                        Descontinuado = productoBD.Descontinuado,
                        listaImgs = listaImgs
                    };

                    productos.Add(producto);
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = productos;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
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
        [Route("CrearProducto")]
        public async Task<IActionResult> CrearProducto(ProductoDTO producto)
        {
            var responseApi = new ResponseAPI<bool>();
            var buscar = await _dbContext.Productos.FirstOrDefaultAsync(x => x.Nombre==producto.Nombre);
            try
            {
                if (buscar == null) {
                    var newProduct = new Producto
                    {
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Stock = producto.Stock,
                        Descontinuado = false
                    };
                    _dbContext.Productos.Add(newProduct);
                    await _dbContext.SaveChangesAsync();
                    List<ImgProdDTO> listaImgs = producto.listaImgs;
                    foreach (var item in listaImgs)
                    {
                        var newImg = new ImgsProducto
                        {
                            Imagen = item.Imagen,
                            IdProPer = newProduct.ProductoId
                        };
                        _dbContext.ImgsProductos.Add(newImg);
                    }

                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = true;
                    responseApi.Mensaje = "Producto creado exitosamente.";
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Valor = false;
                    responseApi.Mensaje = "El nombre del producto ya fue registrado";
                }
                
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                return StatusCode(500, responseApi);
            }
            return Ok(responseApi);
        }


        [HttpPost]
        [Route("ActualizarProducto")]
        public async Task<IActionResult> ActualizarProducto(ProductoDTO producto)
        {
            var responseApi = new ResponseAPI<bool>();

            try
            {
                var productToUpdate = await _dbContext.Productos.FindAsync(producto.IdProducto);

                if (productToUpdate == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Valor = false;
                    responseApi.Mensaje = "Producto no encontrado.";
                    return NotFound(responseApi);
                }
                productToUpdate.Nombre = producto.Nombre;
                productToUpdate.Precio = producto.Precio;
                productToUpdate.Descontinuado = producto.Descontinuado;
                productToUpdate.Stock = producto.Stock;
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = true;
                responseApi.Valor = true;
            }
            catch (Exception ex)
            {
                var error = new ErrorLog();
                error.Tabla = "Productos";
                error.Error = ex.Message;
                error.FechaHora = DateTime.Now;
                _dbContext.ErrorLogs.Add(error);
                await _dbContext.SaveChangesAsync();
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                return StatusCode(500, responseApi);
            }
            return Ok(responseApi);
        }

    }
}

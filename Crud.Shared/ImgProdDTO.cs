using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Shared
{
    public class ImgProdDTO
    {
        public int IdImagen { get; set; }

        public byte[]? Imagen { get; set; }

        public int? IdProPer { get; set; }
    }
}

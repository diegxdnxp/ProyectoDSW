using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Proyecto_DSWI.Models
{
    public class ProveedorO
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }

        [DisplayName("EMPRESA")]
        public string empresaprov { get; set; }

        [DisplayName("NOMBRE")]
        public string nomprov { get; set; }

        [DisplayName("CARGO")]
        public string cargoprov { get; set; }

        [DisplayName("DISTRITO")]
        public int distrito { get; set; }

        [DisplayName("TELEFONO")]
        public int telefono { get; set; }
    }
}
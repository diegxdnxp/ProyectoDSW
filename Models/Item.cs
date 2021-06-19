using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Proyecto_DSWI.Models
{
    public class Item
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }


        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [DisplayName("DESCRIPCION")]
        public string descripcion { get; set; }



        [DisplayName("PRECIO S/")]
        public double precio { get; set; }

        [DisplayName("CANTIDAD")]
        public int cantidad { get; set; }

        [DisplayName("SUBTOTAL")]
        public double subtotal
        {
            get { return precio * cantidad; }
        }

    

        [DisplayName("FOTO")]
        public string foto { get; set; }
    }
}
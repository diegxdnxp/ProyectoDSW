using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Proyecto_DSWI.Models
{
    public class Usuario
    {

        [DisplayName("CODIGO")]
        public int codigo { get; set; }

        [DisplayName("USUARIO")]

        public string usuario{ get; set; }

        [DisplayName("CORREO")]

        public string correo { get; set; }

        [DisplayName("CONTRASEÑA")]
        public string contraseña { get; set; }
    }
}
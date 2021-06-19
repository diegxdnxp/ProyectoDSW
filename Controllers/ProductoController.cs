using Proyecto_DSWI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_DSWI.Controllers
{
    public class ProductoController : Controller
    {

        //Conexion
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CrudProducto()
        {
            return View();
        }
        public ActionResult CrudProveedor()
        {
            return View();
        }
        public ActionResult IndexProducto()
        {
            return View();
        }


        //LISTADO GENERAL
        List<ProductoO> ListGeneral()
        {
            List<ProductoO> aProductos = new List<ProductoO>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTOS", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new ProductoO()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    descripcion = dr[2].ToString(),
                    precio = double.Parse(dr[3].ToString()),
                    stockActual = int.Parse(dr[4].ToString()),
                    categoria = dr[5].ToString(),
                    proveedor = dr[6].ToString(),
                    foto = dr[7].ToString()
                });
            }
            dr.Close();
            cn.Close();
            return aProductos;
        }

        List<Producto> ListGeneralA()
        {
            List<Producto> aProductos = new List<Producto>();
            SqlCommand cmd = new SqlCommand("SP_PRODUCTO", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new Producto()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    descripcion = dr[2].ToString(),
                    precio = double.Parse(dr[3].ToString()),
                    stockActual = int.Parse(dr[4].ToString()),
                    categoria = int.Parse(dr[5].ToString()),
                    proveedor = int.Parse(dr[6].ToString()),
                    foto = dr[7].ToString()
                });
            }
            dr.Close();
            cn.Close();
            return aProductos;
        }



        public ActionResult ListaProducto()
        {

            List<ProductoO> aProductos = new List<ProductoO>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTOS", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new ProductoO()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    descripcion = dr[2].ToString(),
                    precio = double.Parse(dr[3].ToString()),
                    stockActual = int.Parse(dr[4].ToString()),
                    categoria = dr[5].ToString(),
                    proveedor = dr[6].ToString(),
                    foto = dr[7].ToString()

                });
            }
            dr.Close();
            cn.Close();
            return Json(aProductos, JsonRequestBehavior.AllowGet);
        }

        void CRUD(String proceso, List<SqlParameter> pars)
        {
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(proceso, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars.ToArray());
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            cn.Close();
        }


        public ActionResult nuevoProducto(Producto objE, HttpPostedFileBase f)
        {
            if (f == null)
            {
                ViewBag.mensaje = "Seleccione una imagen";
                return View(objE);
            }
            if (Path.GetExtension(f.FileName) != ".jpg")
            {
                ViewBag.mensaje = "Debe ser .JPG";
                return View(objE);
            }
            List<SqlParameter> lista = new List<SqlParameter>()
             {

               new SqlParameter(){ParameterName="@nom",SqlDbType=SqlDbType.VarChar,
                Value=objE.nombre},
                new SqlParameter(){ParameterName="@cat",SqlDbType=SqlDbType.Int,
                Value=objE.categoria},
                new SqlParameter(){ParameterName="@sto",SqlDbType=SqlDbType.Int,
                Value=objE.stockActual},
                new SqlParameter(){ParameterName="@pre",SqlDbType=SqlDbType.Money,
                Value=objE.precio },
                 new SqlParameter(){ParameterName="@des",SqlDbType=SqlDbType.VarChar,
               Value=objE.descripcion },
                  new SqlParameter(){ParameterName="@prov",SqlDbType=SqlDbType.Int,
               Value=objE.proveedor },
                new SqlParameter(){ParameterName="@fot",SqlDbType=SqlDbType.VarChar,
                Value="~/img/"+Path.GetFileName(f.FileName) },
                
               
            };

           

                CRUD("SP_NUEVOPRODUCTO", lista);
                f.SaveAs(Path.Combine(Server.MapPath("~/img/"),
                Path.GetFileName(f.FileName)));
                return Content("1");
            
               
            

           
        }


        public ActionResult actualizaProducto(int id)
        {
            Producto objE = ListGeneralA().Where(e => e.codigo == id).FirstOrDefault();
           
            return View(objE);
        }

        [HttpPost]
        public ActionResult actualizaProducto(Producto objE)
        {
           // Producto objE = ListGeneralA().Where(e => e.codigo == id).FirstOrDefault();

            List<SqlParameter> lista = new List<SqlParameter>()
        {
              new SqlParameter(){ParameterName="@ide",SqlDbType=SqlDbType.Int,
              Value=objE.codigo},
             new SqlParameter(){ParameterName="@nom",SqlDbType=SqlDbType.VarChar,
                Value=objE.nombre},
                new SqlParameter(){ParameterName="@cat",SqlDbType=SqlDbType.Int,
                Value=objE.categoria},
                new SqlParameter(){ParameterName="@sto",SqlDbType=SqlDbType.Int,
                Value=objE.stockActual},
                new SqlParameter(){ParameterName="@pre",SqlDbType=SqlDbType.Money,
                Value=objE.precio },
                 new SqlParameter(){ParameterName="@des",SqlDbType=SqlDbType.VarChar,
               Value=objE.descripcion },
                  new SqlParameter(){ParameterName="@prov",SqlDbType=SqlDbType.Int,
               Value=objE.proveedor },
              new SqlParameter(){ParameterName="@fot",SqlDbType=SqlDbType.VarChar,
              Value=objE.foto }
};
            CRUD("SP_ACTUALIZAPRODUCTO", lista);
            return Json(lista,JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerProveedor()
        {
            List<Proveedor> data = new List<Proveedor>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPROVEEDOR", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Proveedor obj = new Proveedor()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nomprov = dr[1].ToString()
                };
                data.Add(obj);
            }

            cn.Close();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerCategoria()
        {
            List<Categoria> data = new List<Categoria> ();
            SqlCommand cmd = new SqlCommand("SP_CATEGORIA", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria obj = new Categoria()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString()
                };
                data.Add(obj);
            }

            cn.Close();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminaProductos(int? id = null)
        {
            ProductoO objE = ListGeneral().Where(e => e.codigo == id).FirstOrDefault();

            List<SqlParameter> lista = new List<SqlParameter>() {
            new SqlParameter(){ ParameterName="@ide",SqlDbType=SqlDbType.Int,
            Value=objE.codigo 
            }
            };
            CRUD("SP_ELIMINAPRODUCTO", lista);
            return Json(lista,JsonRequestBehavior.AllowGet);
        }
        

        //Metodo que busca producto segun ID
        public ActionResult BuscarProducto(int? id= null)
        {

            List<Producto> aProductos = new List<Producto>();
            SqlCommand cmd = new SqlCommand("SP_BUSCARPRODUCTOID", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ide", id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new Producto()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    categoria = int.Parse(dr[2].ToString()),
                    stockActual = int.Parse(dr[3].ToString()),
                    precio = double.Parse(dr[4].ToString()),
                    descripcion = dr[5].ToString(),
                    proveedor = int.Parse(dr[6].ToString()),
                    foto = dr[7].ToString()
                });
            }

            dr.Close();
            cn.Close();

            return Json(aProductos, JsonRequestBehavior.AllowGet);
        }

        
        //AGREGAR USUARIO
       
        public ActionResult nuevoUsuario(Usuario objE)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
             {
               new SqlParameter(){ParameterName="@usuario",SqlDbType=SqlDbType.VarChar,
                Value=objE.usuario},
                new SqlParameter(){ParameterName="@correo",SqlDbType=SqlDbType.VarChar,
                Value=objE.correo},
                new SqlParameter(){ParameterName="@contraseña",SqlDbType=SqlDbType.VarChar,
                Value=objE.contraseña},

            };
            CRUD("SP_NUEVOUSUARIO", lista);
            return Content("1");

        }

    }
}

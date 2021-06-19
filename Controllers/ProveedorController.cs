using Proyecto_DSWI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_DSWI.Controllers
{
    public class ProveedorController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        // GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CrudProveedor()
        {
            return View();
        }

        //LISTA PROVEEDOR
        public JsonResult ListaProveedor()
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
                    empresaprov = dr[1].ToString(),
                    nomprov = dr[2].ToString(),
                    cargoprov = dr[3].ToString(),
                    distrito = dr[4].ToString(),
                    telefono = int.Parse(dr[5].ToString())

                };
                data.Add(obj);
            }

            cn.Close();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //LISTA DISTRITO
        public JsonResult ObtenerDistrito()
        {
            List<Distrito> data = new List<Distrito>();
            SqlCommand cmd = new SqlCommand("SP_LISTADISTRITO", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Distrito obj = new Distrito()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString()
                };
                data.Add(obj);
            }

            cn.Close();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        List<ProveedorO> ListadoGeneralA()
        {
            List<ProveedorO> data = new List<ProveedorO>();
            SqlCommand cmd = new SqlCommand("SP_PROVEEDOR", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ProveedorO obj = new ProveedorO()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    empresaprov = dr[1].ToString(),
                    nomprov = dr[2].ToString(),
                    cargoprov = dr[3].ToString(),
                    distrito = int.Parse(dr[4].ToString()),
                    telefono = int.Parse(dr[5].ToString())
                };
                data.Add(obj);
            }

            cn.Close();
            return data;
        }

        //PROCESO DE ACTUALIZAR Y REGISTRAR
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
        //AGREGAR PROVEEDOR
        public ActionResult nuevoProveedor(Proveedor objE)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
             {

               new SqlParameter(){ParameterName="@emp",SqlDbType=SqlDbType.VarChar,
                Value=objE.empresaprov},
                new SqlParameter(){ParameterName="@nom",SqlDbType=SqlDbType.VarChar,
                Value=objE.nomprov},
                new SqlParameter(){ParameterName="@cargo",SqlDbType=SqlDbType.VarChar,
                Value=objE.cargoprov},
                new SqlParameter(){ParameterName="@dis",SqlDbType=SqlDbType.Int,
                Value=objE.distrito },
                 new SqlParameter(){ParameterName="@fon",SqlDbType=SqlDbType.Int,
               Value=objE.telefono },
            };
            CRUD("SP_NUEVOPROVEEDOR", lista);
            
            return Content("1");
        }
        //ACTUALIZAR PROVEEDOR
        public ActionResult actualizaProveedor(int id)
        {
            ProveedorO objE = ListadoGeneralA().Where(e => e.codigo == id).FirstOrDefault();

            return View(objE);
        }

        [HttpPost]
        public ActionResult actualizaProveedor(Proveedor objP)
        {
            // Producto objE = ListGeneralA().Where(e => e.codigo == id).FirstOrDefault();

            List<SqlParameter> lista = new List<SqlParameter>()
        {
                new SqlParameter(){ ParameterName="@IDPROV",SqlDbType=SqlDbType.Int,Value=objP.codigo},
                new SqlParameter(){ ParameterName="@EMPRESA",SqlDbType=SqlDbType.VarChar,Value=objP.empresaprov},
                new SqlParameter(){ ParameterName="@NOMBREPROVEEDOR",SqlDbType=SqlDbType.VarChar,Value=objP.nomprov},
                new SqlParameter(){ ParameterName="@CARGOPROVEEDOR",SqlDbType=SqlDbType.VarChar,Value=objP.cargoprov},
                new SqlParameter(){ ParameterName="@IDDIS",SqlDbType=SqlDbType.Int,Value=objP.distrito},
                new SqlParameter(){ ParameterName="@TELEFONO",SqlDbType=SqlDbType.VarChar,Value=objP.telefono}
        };
            CRUD("SP_ACTUALIZAPROVEEDOR", lista);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //BUSCAR PROVEEEDOR POR CODIGO
        public ActionResult BuscarProveedor(int? id = null)
        {
            List<ProveedorO> aProductos = new List<ProveedorO>();
            SqlCommand cmd = new SqlCommand("SP_PROVEEDOR", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new ProveedorO()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    empresaprov = dr[1].ToString(),
                    nomprov = dr[2].ToString(),
                    cargoprov = dr[3].ToString(),
                    distrito = int.Parse(dr[4].ToString()),
                    telefono = int.Parse(dr[5].ToString()),
                   
                });
            }


            ProveedorO objP = aProductos.Where(a => a.codigo == id).FirstOrDefault();
            return Json(objP, JsonRequestBehavior.AllowGet);
        }
        //ELIMINAR PROVEEDOR
        public ActionResult eliminaProveedor(int? id = null)
        {
            ProveedorO objE = ListadoGeneralA().Where(e => e.codigo == id).FirstOrDefault();

            List<SqlParameter> lista = new List<SqlParameter>() {
            new SqlParameter(){ ParameterName="@ide",SqlDbType=SqlDbType.Int,
            Value=objE.codigo
            }
            };
            CRUD("SP_ELIMINAPROVEEDOR", lista);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }


    }
}
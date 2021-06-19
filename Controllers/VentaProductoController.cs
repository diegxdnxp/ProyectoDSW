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
    public class VentaProductoController : Controller
    {
        //Conexion
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        //Vista Principal de productos
        public ActionResult Principal()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        //Vista de seleccion de Productos
        public ActionResult Seleccion()
        {
            return View();
        }

        //Carrito de Compras
        public ActionResult carritoCompras()
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<Item>();
            }
            return View(ListProductos());
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
                    categoria=dr[5].ToString(),
                    proveedor = dr[6].ToString(),
                    foto = dr[7].ToString()
                   
                });
            }
            dr.Close();
            cn.Close();
            return Json(aProductos, JsonRequestBehavior.AllowGet);
        }

        //Metodo que lista los item en formato Json
        public ActionResult ListaItem()
        {
            
            if (Session["carrito"] == null)
            {
                return RedirectToAction("Principal");
            }
            var carrito = (List<Item>)Session["carrito"];
            var monto = carrito.Sum(p => p.subtotal);

            return Json(carrito, JsonRequestBehavior.AllowGet);


        }

        public ActionResult actualizarTotal()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("Principal");
            }
            var carrito = (List<Item>)Session["carrito"];
            var monto = carrito.Sum(p => p.subtotal);

            return Json(monto, JsonRequestBehavior.AllowGet);

        }
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult buscarProducto(string nom)
        {
            List<ProductoO> aproductos= new List<ProductoO>();
            SqlCommand cmd = new SqlCommand("SP_BUSCAR_PRODUCTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nom", nom);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ProductoO objE = new ProductoO()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    descripcion = dr[2].ToString(),
                    precio=double.Parse(dr[3].ToString()),
                    foto=dr[4].ToString(),
                    categoria=dr[5].ToString(),
                    proveedor=dr[6].ToString()
                };
                aproductos.Add(objE);
            }
            dr.Close();
            cn.Close();
            return Json(aproductos, JsonRequestBehavior.AllowGet);
            
        }

        //LISTADO GENERAL
        List<ProductoO> ListProductos()
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


        public ActionResult seleccionaProductos(int id)
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<Item>();
            }

            ProductoO objP = ListProductos().Where(a => a.codigo == id).FirstOrDefault();
            return View(objP);

        }

        public ActionResult Comprar()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("Principal");
            }
            var carrito = (List<Item>)Session["carrito"];
            ViewBag.monto = carrito.Sum(p => p.subtotal);
            return View(carrito);


        }

        //Metodo para seleccionar un producto Json
        public ActionResult seleccionaProducto(string nombre)
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


            ProductoO objP = aProductos.Where(a => a.nombre == nombre).FirstOrDefault();
            return Json(objP, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminaProducto(int? id = null)
        {
            
            if (id == null) return RedirectToAction("carritoCompras");
            var carrito = (List<Item>)Session["carrito"];
            var item = carrito.Where(i => i.codigo == id).FirstOrDefault();
            carrito.Remove(item);
            Session["carrito"] = carrito;
           
            return Json(carrito, JsonRequestBehavior.AllowGet);
        }

        //Metodo para agregar un producto adquirido desde carrito de compras
        public ActionResult agregarProducto(int id, int cant = 0)
        {
            var miProducto = ListProductos().Where(p => p.codigo == id).FirstOrDefault();
            Item objI = new Item()
            {
                codigo = miProducto.codigo,
                nombre=miProducto.nombre,
  
                descripcion = miProducto.descripcion,
                precio = miProducto.precio,
                cantidad = cant,
                foto = miProducto.foto
            };
            var miCarrito = (List<Item>)Session["carrito"];
            miCarrito.Add(objI);
            Session["carrito"] = miCarrito;
            return RedirectToAction("Principal");
        }
        /*public ActionResult Comprar()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("carritoCompras");
            }
            var carrito = (List<Item>)Session["carrito"];
            ViewBag.monto = carrito.Sum(p => p.subtotal);
            return View(carrito);
        }*/

        //Metodo para eliminar un producto del carrito
       

        //Metodo para pagar
        public ActionResult Pago()
        {
            List<Item> detalle = (List<Item>)Session["carrito"];
            double mt = 0;
            foreach (Item it in detalle)
            {
                mt += it.subtotal;
            }
            ViewBag.mt = mt;
            return View(detalle);
        }

        public ActionResult Final(string dni, string nombres)
        {
            ViewBag.dni = dni;
            ViewBag.nombres = nombres;
            List<Item> detalle = (List<Item>)Session["carrito"];
            double mt = 0;
            foreach (Item it in detalle)
            {
                mt += it.subtotal;
            }
            ViewBag.mt = mt;
            return View();
        }

       



    }
}
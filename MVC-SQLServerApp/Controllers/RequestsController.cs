using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using NHibernate;
using NHibernate.Cfg;
using MVC_SQLServerApp.Models;

namespace MVC_SQLServerApp.Controllers
{
    public class RequestsController : Controller
    {
        /// <summary>
        /// Variables Globales
        /// </summary>
        private string conString = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        private NHibernate.Cfg.Configuration myConfiguration;
        private ISessionFactory myISessionFactory;
        private static ISession myISession;
        private RequestClass myRequest;
        private string emptyRequest;
        /// <summary>
        /// Se crea la configuración de inicio de la aplicación y
        /// muestra, usando la vista Index.cshtml la pagina de iniio
        /// </summary>
        /// <returns>Devuelve Index.cshtml, la vista asociada a la funcion Index()</returns>
        public ActionResult Index()
        {
            myConfiguration = new NHibernate.Cfg.Configuration();
            myConfiguration.Configure();
            myISessionFactory = myConfiguration.BuildSessionFactory();
            myISession = myISessionFactory.OpenSession();
            string msgSalida = "";
            using (SqlConnection con = new SqlConnection(conString))
            {
                try//tratamos de crear la tabla, si existe atrapa la excepcion y lanza el mensaje correspondiente
                {
                    // Abrir conexion sql.
                    con.Open();
                    // Creamos la tabla.
                    using (SqlCommand command = new SqlCommand("CREATE TABLE requests (id int IDENTITY(1,1) PRIMARY KEY, request text NOT NULL);", con))
                    {
                        command.ExecuteNonQuery();//verificamos que el comando afecta la base de datos
                        msgSalida = "La tabla requests no existe, creando tabla en base de datos";
                    }                    
                }
                catch (Exception ex)
                {
                    msgSalida = "La tabla requests ya existe, no es necesario crearla.";
                }
            }
            ViewData["msgSalida"] = msgSalida;
            return View();
        }
        /// <summary>
        /// Cuenta la cantidad de filas de la tabla results.
        /// 
        /// Lee el objeto asociado a la base de datos del tipo RequestClass y 
        /// obtiene el numero de filas.
        /// </summary>
        /// <returns>Devuelve la vista asociada a la función count()</returns>
        public ActionResult count() {
            using (myISession.BeginTransaction())
            {
                var query = myISession.QueryOver<RequestClass>();
                var result = query.List();
                ViewData["countNumber"] = "La cantidad de filas de la tabla \"requests\" es:" + query.RowCount();
            }
            return View();
        }
        /// <summary>
        /// Obtiene el requests con identificador id.
        /// En este caso el id es pasado por GET a traves de la URL.
        /// </summary>
        /// <param name="id">Identificador unico de una fila de la tabla requests.</param>
        /// <returns>Devuelve la cista asociada a la funcion getRequest()</returns>
        [HttpGet]
        public ActionResult getRequest(int id=0) {
            if (id != 0)
            {
                using (myISession.BeginTransaction())
                {
                    RequestClass reqClass=myISession.Get<RequestClass>(id);
                    var query = myISession.QueryOver<RequestClass>();
                    var result = query.List();
                    var rowcount = query.RowCount();
                    myISession.Transaction.Commit();
                    ViewData["getRequest"] = rowcount+" La peticion con id = "+id +" es: \"" + reqClass.request+"\"";
                }
            }
            else
            {
                ViewData["getRequest"] = "";
            }
            return View();     
        }
        /// <summary>
        /// Obtiene el requests con identificador id.
        /// En este caso el id es pasado por POST a traves del campo id en el formulario.
        /// </summary>
        /// <param name="id">Identificador unico de una fila de la tabla requests.</param>
        /// <returns>Devuelve la vista asociada a la funcion getRequest()</returns>
        [HttpPost]
        public ActionResult getRequest(RequestClass r) {
            using (myISession.BeginTransaction())
            {
                try { 
                    RequestClass reqClass = myISession.Get<RequestClass>(r.id);
                    myISession.Transaction.Commit();
                    ViewData["getRequest"] = "La peticion con id = " + r.id + " es: \"" + reqClass.request + "\""; 
                }
                catch(Exception e){
                    ViewData["getRequest"] = "No se puede recuperar la petición: "+r.id;
                }
            }
            return View();
        }
        /// <summary>
        /// Solicita ingresar una petición (request).
        /// </summary>
        /// <returns>Devuelve la vista asociada a la funcion putRequest()</returns>
        [HttpGet]
        public ActionResult putRequest()
        {
            emptyRequest = "Introduzca una peticion (request)";
            ViewData["putRequest"] = emptyRequest;
            return View();
        }
        /// <summary>
        /// Guarda en la base de datos la petición ingresada en el formulario
        /// de la vista asociada.
        /// </summary>
        /// <param name="r">Objeto del tipo RequestClass que contiene la petición 
        /// que se desea guardar.
        /// </param>
        /// <returns>Devuelve la vista asociada a ala funcion putRequest()</returns>
        [HttpPost]
        public ActionResult putRequest(RequestClass r)
        {
            string _request = r.request;
            using (myISession.BeginTransaction())
            {
                if (_request != null)
                {
                    myRequest = new RequestClass() { request = _request };
                    emptyRequest = "Request \"" + _request + "\" guardada!!";
                    myISession.Save(myRequest);
                    myISession.Transaction.Commit();
                }
            }
            ViewData["putRequest"] = emptyRequest;
            return View();
        }

    }
}

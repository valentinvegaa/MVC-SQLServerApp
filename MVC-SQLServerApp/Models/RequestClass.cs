using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_SQLServerApp.Models
{
    /// <summary>
    /// Clase asociada al modelo de la tabla requests
    /// </summary>
    public class RequestClass
    {
        public virtual int id { get; set; }
        public virtual string request { get; set; }
    }
}
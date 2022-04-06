using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTORIAL_ARCHIVOS.Models;

namespace TUTORIAL_ARCHIVOS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult InsertarArchivos(HttpPostedFileBase[] archivos)
        {
            Respuesta_Json respuesta = new Respuesta_Json();
            try
            {
                for (int i = 0; i < archivos.Length; i++)
                {
                    Archivos archivo = new Archivos();

                    archivo.Fecha_Entrada = DateTime.Now;
                    archivo.Nombre_Archivo = Path.GetFileNameWithoutExtension(archivos[i].FileName);
                    archivo.Extension = Path.GetExtension(archivos[i].FileName);
                    archivo.Formato = MimeMapping.GetMimeMapping(archivos[i].FileName);

                    double tamanio = archivos[i].ContentLength;
                    tamanio = tamanio / 1000000.0;
                    archivo.Tamanio = Math.Round(tamanio, 2);

                    Stream fs = archivos[i].InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    archivo.Archivo = br.ReadBytes((Int32)fs.Length);

                    using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Model1"].ConnectionString))
                    {
                        connection.Open();
                        string sql = "insert into Archivos(Nombre_Archivo, Extension, Formato, Fecha_Entrada, Archivo, Tamanio) values " +
                            "(@nombreArchivo, @extension, @formato, @fechaEntrada, @archivo, @tamanio)";
                        using(SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@nombreArchivo", SqlDbType.VarChar, 100).Value = archivo.Nombre_Archivo;
                            cmd.Parameters.Add("@extension", SqlDbType.VarChar, 5).Value = archivo.Extension;
                            cmd.Parameters.Add("@formato", SqlDbType.VarChar, 200).Value = archivo.Formato;
                            cmd.Parameters.Add("@fechaEntrada", SqlDbType.DateTime).Value = archivo.Fecha_Entrada;
                            cmd.Parameters.Add("@archivo", SqlDbType.Image).Value = archivo.Archivo;
                            cmd.Parameters.Add("@tamanio", SqlDbType.Float).Value = archivo.Tamanio;
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }

                }

                respuesta.Codigo = 1;
                respuesta.Mensaje_Respuesta = "Se insertaron correctamente los archivos en la base de datos";

            }catch(Exception ex)
            {
                respuesta.Codigo = 0;
                respuesta.Mensaje_Respuesta = ex.ToString();
            }

            return Json(respuesta);
        }
    }
}
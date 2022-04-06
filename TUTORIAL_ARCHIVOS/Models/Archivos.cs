using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TUTORIAL_ARCHIVOS.Models
{
    public class Archivos
    {
        public int Id { get; set; }
        public string Nombre_Archivo { get; set; }
        public string Extension { get; set; }
        public string Formato { get; set; }
        public DateTime Fecha_Entrada { get; set; }
        public byte[] Archivo { get; set; }
        public double Tamanio { get; set; }

    }
}
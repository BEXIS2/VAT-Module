using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BExIS.Modules.VAT.UI.Models
{
    public class EditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Select a Longitude.")]
        public string Longitude { get; set; }

        [Required(ErrorMessage = "Select a Latitude.")]

        public string Latitude { get; set; }

        [Required(ErrorMessage = "Select a Datatype.")]

        public string  DataType { get; set; }

        public string  Time { get; set; }

        [Required(ErrorMessage = "SpatialReference.")]
        public string  SpatialReference { get; set; }

        public EditModel()
        {
            Longitude = "";
            Latitude = "";
            DataType = "";
            Time = "";
            SpatialReference = "";
        }
    }
}
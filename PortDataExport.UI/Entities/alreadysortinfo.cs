namespace PortDataExport.UI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("db_wcs.alreadysortinfo")]
    public partial class alreadysortinfo
    {
        [Key]
        public int FInterID { get; set; }

        [Required]
        [StringLength(255)]
        public string billCode { get; set; }

        [StringLength(255)]
        public string trayCode { get; set; }

        [Required]
        [StringLength(255)]
        public string pipeline { get; set; }

        [Required]
        [StringLength(255)]
        public string sortTime { get; set; }

        [Required]
        [StringLength(255)]
        public string turnNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string sortPortCode { get; set; }

        [Required]
        [StringLength(255)]
        public string sortSource { get; set; }

        [Required]
        [StringLength(255)]
        public string sendStatus { get; set; }

        [StringLength(255)]
        public string userID { get; set; }
    }
}

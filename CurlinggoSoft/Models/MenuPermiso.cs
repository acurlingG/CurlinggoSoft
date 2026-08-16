using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurlinggoSoft.Models
{
    [Table("MenuPermisos")]
    public class MenuPermiso
    {
        [Display(Name = "Menú")]
        public long MenuID { get; set; }

        [Display(Name = "Permiso")]
        public int PermisoID { get; set; }

        [ForeignKey("MenuID")]
        public virtual Menu? Menu { get; set; }

        [ForeignKey("PermisoID")]
        public virtual Permiso? Permiso { get; set; }
    }
}

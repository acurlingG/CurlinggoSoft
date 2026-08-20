namespace CurlinggoSoft.Models
{
    // Representa un nodo (padre o hijo) del árbol de menú ya filtrado
    // por los permisos del usuario. No es una entidad de base de datos,
    // es solo la forma en que se lo pasamos a la vista.
    public class MenuNodeVM
    {
        public long MenuID { get; set; }
        public string Nombre { get; set; } = "";
        public string? Url { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
        public List<MenuNodeVM> Hijos { get; set; } = new();
    }
}

using CurlinggoSoft.Models;
using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Services
{
    // Helper compartido entre SolicitudServicioController (cliente) y
    // TecnicoController (técnico) para decidir si una cancelación cae
    // dentro de la ventana de gracia (sin penalización) o no.
    //
    // Regla "O" (gracia dura hasta que pase lo que sea MÁS TARDE entre las
    // dos condiciones, igual que Uber/Puls):
    //   1) Han pasado 10 minutos o menos desde que la reserva pasó a ASIGNADA.
    //   2) El técnico todavía NO ha marcado la reserva como EN_CAMINO.
    // Solo hay penalización simulada cuando AMBAS condiciones fallan a la
    // vez: ya pasaron más de 10 minutos Y el técnico ya salió (EN_CAMINO o
    // estado posterior).
    public static class ReservaCancelacionHelper
    {
        private const int MinutosGracia = 10;

        // Códigos de estado de reserva que indican que el técnico "ya salió"
        // o está más adelante en el flujo que EN_CAMINO.
        private static readonly string[] EstadosDesdeEnCaminoEnAdelante =
            { "EN_CAMINO", "EN_PROCESO", "COMPLETADA" };

        public static readonly IReadOnlyDictionary<string, string> MotivosCancelacionPermitidos =
            new Dictionary<string, string>
            {
                ["TECNICO_TARDO"] = "El técnico tardó demasiado",
                ["YA_NO_NECESITO"] = "Ya no necesito el servicio",
                ["OTRA_OPCION"] = "Encontré otra opción",
                ["ERROR_RESERVA"] = "Error al pedir la reserva"
            };

        public static bool EsMotivoValido(string? motivoCodigo) =>
            !string.IsNullOrEmpty(motivoCodigo) && MotivosCancelacionPermitidos.ContainsKey(motivoCodigo);

        // Devuelve true si la cancelación NO debería tener penalización
        // (está dentro de la ventana de gracia); false si aplica
        // penalización simulada.
        public static async Task<bool> EstaDentroDeVentanaDeGraciaAsync(ApplicationDbContext context, long reservaId)
        {
            var estadoActualCodigo = await context.SolicitudesReserva
                .Where(r => r.ReservaID == reservaId)
                .Select(r => r.EstadoReserva!.Codigo)
                .FirstOrDefaultAsync();

            // Condición 2: si el técnico todavía no marcó EN_CAMINO (ni
            // estados posteriores), siempre hay gracia sin importar el
            // tiempo transcurrido.
            var tecnicoYaSalio = estadoActualCodigo != null &&
                EstadosDesdeEnCaminoEnAdelante.Contains(estadoActualCodigo);

            if (!tecnicoYaSalio)
                return true;

            // Condición 1: minutos transcurridos desde que pasó a ASIGNADA
            // (se toma el registro más reciente por si hubo reasignación).
            var fechaAsignada = await context.HistorialEstadosReserva
                .Where(h => h.ReservaID == reservaId &&
                            h.EstadoNuevo!.Codigo == "ASIGNADA")
                .OrderByDescending(h => h.FechaCambio)
                .Select(h => (DateTime?)h.FechaCambio)
                .FirstOrDefaultAsync();

            if (fechaAsignada == null)
                return true; // sin dato de referencia, no penalizamos

            var minutosTranscurridos = (DateTime.Now - fechaAsignada.Value).TotalMinutes;

            return minutosTranscurridos <= MinutosGracia;
        }
    }
}

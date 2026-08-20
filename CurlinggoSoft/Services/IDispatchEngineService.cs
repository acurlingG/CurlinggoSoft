namespace CurlinggoSoft.Services
{
    public interface IDispatchEngineService
    {
        /// <summary>
        /// Ejecuta el algoritmo de Match Predictivo y envía ofertas al primer lote de técnicos.
        /// </summary>
        Task<bool> GenerarOfertasLoteInicialAsync(long reservaId, int tamanoLote = 3);
    }
}
# Copilot Instructions

## Directrices del proyecto
- Al crear SqlParameter para valores nullable (decimal?, int?, etc.) en CurlinggoSoft, siempre usar `(object?)valor ?? DBNull.Value` en vez de pasar el valor null directamente, para evitar SqlException de parámetro no proporcionado.
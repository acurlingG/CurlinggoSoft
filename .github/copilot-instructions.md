# Copilot Instructions

## Project Guidelines
- Al crear SqlParameter para valores nullable (decimal?, int?, etc.) en CurlinggoSoft, siempre usar `(object?)valor ?? DBNull.Value` en vez de pasar el valor null directamente, para evitar SqlException de parámetro no proporcionado.
- Los combos dependientes (provincia→cantón→distrito) en CurlinggoSoft deben implementarse con endpoints AJAX (GET Json) en los controladores del catálogo padre (ej. CantonesController.PorProvincia, DistritosController.PorCanton) y JS con jQuery ($.getJSON) en las vistas Create/Edit, en vez de recargar la página completa.
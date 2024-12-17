using System.Globalization;

namespace CashFlow.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next)
    {
        // Requestdelegate - Permissão para deixar ou não o projeto continuar.
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        // Extrair a cultura da requisição
        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        //if(supportedLanguages.Exists(requestCulture))

        // Esse é o formato default;
        var cultureInfo = new CultureInfo("en");

        if(!string.IsNullOrWhiteSpace(requestCulture) && supportedLanguages.Exists(l => l.Name.Equals(requestCulture)))
        {
            // Sobrescrever o valor
            cultureInfo = new CultureInfo(requestCulture);
        }

        // Aqui é o formato para mudar a cultura da aplicação
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}

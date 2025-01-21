using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor httpContextAcessor)
    {
        _contextAccessor = httpContextAcessor;
    }
    public string TokenOnRequest()
    {
       var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        // Preciso remover o BEARER;
        // ["Bearer ".Length..] - Inicia no lugar tal e aceita o resto dos valores. Similar a um substring
        return authorization["Bearer ".Length..].Trim();
    }
}

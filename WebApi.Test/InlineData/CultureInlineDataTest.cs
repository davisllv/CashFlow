using System.Collections;

namespace WebApi.Test.InlineData;
public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Retorna um valor por vez; e verifica se tem mais valores se preciso, se tem ele faz de novo.
        yield return new Object[]{"en"};
        yield return new Object[]{"pt-BR"};
        yield return new Object[]{"pt-PT"};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


}

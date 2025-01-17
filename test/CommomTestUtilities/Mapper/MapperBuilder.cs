using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommomTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(conf =>
        {
            conf.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}

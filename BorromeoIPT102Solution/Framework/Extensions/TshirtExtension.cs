using Dapper;
using Domain.Models;

namespace Framework.Extensions;

public static class TshirtExtension
{
    public static DynamicParameters ToCreateParameters(this Tshirt tshirt)
    {
        var p = new DynamicParameters();
        p.Add("@Name", tshirt.Name);
        p.Add("@Quantity", tshirt.Quantity);
        p.Add("@Price", tshirt.Price);
        p.Add("@Brand", tshirt.Brand);
        return p;
    }

    public static DynamicParameters ToUpdateParameters(this Tshirt tshirt)
    {
        var p = new DynamicParameters();
        p.Add("@TshirtId", tshirt.TshirtId);
        p.Add("@Name", tshirt.Name);
        p.Add("@Quantity", tshirt.Quantity);
        p.Add("@Price", tshirt.Price);
        p.Add("@Brand", tshirt.Brand);
        return p;
    }

    public static DynamicParameters ToDeleteParameters(this int tshirtId)
    {
        var p = new DynamicParameters();
        p.Add("@TshirtId", tshirtId);
        return p;
    }

    public static DynamicParameters ToReadByIdParameters(this int tshirtId)
    {
        var p = new DynamicParameters();
        p.Add("@TshirtId", tshirtId);
        return p;
    }
}

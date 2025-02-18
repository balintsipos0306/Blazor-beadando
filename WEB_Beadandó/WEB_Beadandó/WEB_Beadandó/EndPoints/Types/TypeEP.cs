using DataModels;
using WEB_Beadandó.Client;
using WEB_Beadandó.DataServices;

namespace WEB_Beadandó.EndPoints.Types;

public static class TypeEP
{
    public static WebApplication AddTypeEndpoints(this WebApplication app)
    {
        app.MapGet(APIUri.TypeGetList, async (IDataServices ds)
            => await GetTypeList(ds))
        .WithName("GetTypeList")
        .WithDescription("Leiras")
        .Produces<List<DataModels.Type>>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest);

        app.MapDelete("/api/types/{typeId}", async (int typeId, IDataServices ds) =>
        {
            var branch = await ds.GetById<DataModels.Type>(typeId);
            if (branch == null)
            {
                return Results.NotFound();
            }

            var query = $@"
                DELETE FROM [Haszaltauto].[User].[Type]
                WHERE typeId = {typeId}";

            var result = await ds.Execute(query);
            if (result > 0)
            {
                return Results.NoContent();
            }

            return Results.BadRequest("Hiba történt a törlés során.");
        });

        app.MapPost("/api/types", async (TypeDto newType, IDataServices ds) =>
        {
            if (newType == null)
            {
                return Results.BadRequest("Érvénytelen adat.");
            }

            var query = $@"
                INSERT INTO [Haszaltauto].[User].[Type] (brandId, typeName)
                VALUES (@BrandId, @TypeName)";

            var parameters = new
            {
                BrandId = newType.brandId,
                TypeName = newType.typeName,
            };

            var result = await ds.Execute(query, parameters);
            if (result > 0)
            {
                return Results.Ok("Telephely sikeresen hozzáadva.");
            }

            return Results.BadRequest("Hiba történt a telephely hozzáadásakor.");
        });

        return app;
    }
    private static async Task<IEnumerable<DataModels.Type>> GetTypeList(IDataServices ds)
    {
        var akarmi = await ds.GetList<DataModels.Type>("SELECT [typeID],[brandId],[typeName] FROM [Haszaltauto].[User].[Type]");
        return akarmi;
    }
}

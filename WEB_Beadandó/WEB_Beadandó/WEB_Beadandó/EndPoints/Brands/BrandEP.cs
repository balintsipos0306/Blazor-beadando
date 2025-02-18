using DataModels;
using WEB_Beadandó.Client;
using WEB_Beadandó.DataServices;

namespace WEB_Beadandó.EndPoints.Brands;

public static class BrandEP
{
    public static WebApplication AddBrandEndpoints(this WebApplication app)
    {
        app.MapGet(APIUri.BrandGetList, async (IDataServices ds)
            => await GetBrandList(ds))
        .WithName("GetBrandList")
        .WithDescription("Leiras")
        .Produces<List<Brand>>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest);

        app.MapPost("/api/brands", async (BrandDto newBrand, IDataServices ds) =>
        {
            if (newBrand == null)
            {
                return Results.BadRequest("Érvénytelen adat.");
            }

            var query = $@"
                INSERT INTO [Haszaltauto].[User].[Brand] (brandName)
                VALUES (@BrandName)";

            var parameters = new
            {
                BrandName = newBrand.brandName,
            };

            var result = await ds.Execute(query, parameters);
            if (result > 0)
            {
                return Results.Ok("Márka sikeresen hozzáadva.");
            }

            return Results.BadRequest("Hiba történt a márka hozzáadásakor.");
        });

        app.MapDelete("/api/brands/{brandId}", async (int brandId, IDataServices ds) =>
        {
            var brand = await ds.GetById<DataModels.Brand>(brandId);
            if (brand == null)
            {
                return Results.NotFound();
            }

            var query = $@"
                DELETE FROM [Haszaltauto].[User].[Brand]
                WHERE brandId = {brandId}";

            var result = await ds.Execute(query);
            if (result > 0)
            {
                return Results.NoContent();
            }

            return Results.BadRequest("Hiba történt a törlés során.");
        });


        return app;
    }

    private static async Task<IEnumerable<Brand>> GetBrandList(IDataServices ds)
    {
        var akarmi = await ds.GetList<Brand>("SELECT [brandId],[brandName] FROM [Haszaltauto]. [User].[Brand]");
        return akarmi;
    }
}

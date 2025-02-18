using DataModels;
using WEB_Beadandó.Client;
using WEB_Beadandó.DataServices;

namespace WEB_Beadandó.EndPoints.Car;

public static class CarEP
{
    public static WebApplication AddCarEndpoints(this WebApplication app)
    {
        app.MapGet(APIUri.CarGetList, async (IDataServices ds)
                    => await GetCarList(ds))
           .WithName("GetCarList")
           .WithDescription("Leiras")
           .Produces<List<DataModels.Car>>(StatusCodes.Status200OK)
           .Produces<string>(StatusCodes.Status400BadRequest);

        app.MapDelete("/api/cars/{carId}", async (int carId, IDataServices ds) =>
        {
            var car = await ds.GetById<DataModels.Car>(carId);
            if (car == null)
            {
                return Results.NotFound();
            }

            var query = $@"
                DELETE FROM [Haszaltauto].[User].[Car]
                WHERE carId = {carId}";

            var result = await ds.Execute(query);
            if (result > 0)
            {
                return Results.NoContent();
            }

            return Results.BadRequest("Hiba történt a törlés során.");
        });

        app.MapPost("/api/cars", async (CarDto newCar, IDataServices ds) =>
        {
            // Ellenőrzés
            if (newCar == null)
            {
                return Results.BadRequest("Érvénytelen adat.");
            }

            // SQL lekérdezés az új autó beszúrásához
            var query = $@"
                INSERT INTO [Haszaltauto].[User].[Car] (branchesId, typeId, year, price)
                VALUES (@BranchId, @TypeId, @Year, @Price)";

            var parameters = new
            {
                BranchId = newCar.branchId,
                TypeId = newCar.typeId,
                Year = newCar.year,
                Price = newCar.price
            };

            var result = await ds.Execute(query, parameters);
            if (result > 0)
            {
                return Results.Ok("Autó sikeresen hozzáadva.");
            }

            return Results.BadRequest("Hiba történt az autó hozzáadásakor.");
        });

        return app;
    }

    private static async Task<IEnumerable<DataModels.Car>> GetCarList(IDataServices ds)
    {
        var akarmi = await ds.GetList<DataModels.Car>(@"
        SELECT 
            c.carId,
            b.branchName AS branchName,
            d.brandName AS brandName,
            t.typeName AS typeName,
            c.year,
            c.price
        FROM [Haszaltauto].[User].[Car] c
        INNER JOIN [Haszaltauto].[User].[Branches] b ON c.branchesId = b.branchesId
        INNER JOIN [Haszaltauto].[User].[Type] t ON c.typeId = t.typeID
        INNER JOIN [HaszaltAuto].[User].[Brand] d ON t.brandId = d.brandId
    ");
        return akarmi;
    }
}

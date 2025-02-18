using DataModels;
using WEB_Beadandó.Client;
using WEB_Beadandó.DataServices;

namespace WEB_Beadandó.EndPoints.Branch;

public static class BranchEP
{
    public static WebApplication AddBranchEndpoints(this WebApplication app)
    {
        app.MapGet(APIUri.BranchesGetList, async (IDataServices ds)
                    => await GetBranchList(ds))
           .WithName("GetBranchList")
           .WithDescription("Leiras")
           .Produces<List<Branches>>(StatusCodes.Status200OK)
           .Produces<string>(StatusCodes.Status400BadRequest);

        app.MapDelete("/api/branches/{branchesId}", async (int branchesId, IDataServices ds) =>
        {
            var branch = await ds.GetById<DataModels.Branches>(branchesId);
            if (branch == null)
            {
                return Results.NotFound();
            }

            var query = $@"
                DELETE FROM [Haszaltauto].[User].[Branches]
                WHERE branchesId = {branchesId}";

            var result = await ds.Execute(query);
            if (result > 0)
            {
                return Results.NoContent();
            }

            return Results.BadRequest("Hiba történt a törlés során.");
        });

        app.MapPost("/api/branches", async (BranchDto newBranch, IDataServices ds) =>
        {
            if (newBranch == null)
            {
                return Results.BadRequest("Érvénytelen adat.");
            }

            var query = $@"
                INSERT INTO [Haszaltauto].[User].[Branches] (branchName, address)
                VALUES (@BranchName, @Address)";

            var parameters = new
            {
                BranchName = newBranch.branchName,
                address = newBranch.address,
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

    private static async Task<IEnumerable<Branches>> GetBranchList(IDataServices ds)
    {
        var akarmi = await ds.GetList<Branches>("SELECT [branchesId],[branchName],[address] FROM [Haszaltauto]. [User].[Branches]");
        return akarmi;
    }
}

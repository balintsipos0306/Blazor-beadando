using FluentMigrator;

[Migration(2025010902)]
public class SeedData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("Branches").InSchema("User").Row(new { branchName = "Blue Car Autókereskedés", address = "Szombathely Külső Söptei út" });
        Insert.IntoTable("Branches").InSchema("User").Row(new { branchName = "AM&AM auto", address = "Veszprém Házgyári út 22" });

        Insert.IntoTable("Brand").InSchema("User").Row(new { brandName = "Toyota" });
        Insert.IntoTable("Brand").InSchema("User").Row(new { brandName = "Ford" });

        Insert.IntoTable("Type").InSchema("User").Row(new { brandId = 1, typeName = "Rav4" });
        Insert.IntoTable("Type").InSchema("User").Row(new { brandId = 1, typeName = "Corolla" });
        Insert.IntoTable("Type").InSchema("User").Row(new { brandId = 2, typeName = "Ranger" });

        Insert.IntoTable("Car").InSchema("User").Row(new { branchesId = 1, typeId = 1, year = 2020, price = 5000000 });
        Insert.IntoTable("Car").InSchema("User").Row(new { branchesId = 2, typeId = 2, year = 2015, price = 3000000 });
        Insert.IntoTable("Car").InSchema("User").Row(new { branchesId = 2, typeId = 3, year = 2019, price = 4500000 });
    }

    public override void Down()
    {
        Delete.FromTable("Car").InSchema("User").AllRows();
        Delete.FromTable("Types").InSchema("User").AllRows();
        Delete.FromTable("Brand").InSchema("User").AllRows();
        Delete.FromTable("Branches").InSchema("User").AllRows();
    }
}

using FluentMigrator;
using System.Data;

[Migration(2025010901)]
public class CreateTables : Migration
{
    public override void Up()
    {
        _ = Create.Schema("User");

        Create.Table("Branches").InSchema("User")
            .WithColumn("branchesId").AsInt32().PrimaryKey().Identity()
            .WithColumn("branchName").AsString(100).NotNullable()
            .WithColumn("address").AsString(200).NotNullable();

        Create.Table("Brand").InSchema("User")
            .WithColumn("brandId").AsInt32().PrimaryKey().Identity()
            .WithColumn("brandName").AsString(100).NotNullable();

        Create.Table("Type").InSchema("User")
            .WithColumn("typeId").AsInt32().PrimaryKey().Identity()
            .WithColumn("brandId").AsInt32()
            .WithColumn("typeName").AsString(100).NotNullable();

        Create.Table("Car").InSchema("User")
            .WithColumn("carId").AsInt32().PrimaryKey().Identity()
            .WithColumn("branchesId").AsInt32()
            .WithColumn("typeId").AsInt32()
            .WithColumn("year").AsInt32().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable();

        Create.ForeignKey()
            .FromTable("Type").InSchema("User").ForeignColumn("brandId")
            .ToTable("Brand").InSchema("User").PrimaryColumn("brandId")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable("Car").InSchema("User").ForeignColumn("branchesId")
            .ToTable("Branches").InSchema("User").PrimaryColumn("branchesId")
            .OnDelete(Rule.Cascade);
        Create.ForeignKey()
            .FromTable("Car").InSchema("User").ForeignColumn("typeId")
            .ToTable("Type").InSchema("User").PrimaryColumn("typeId")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("Car").InSchema("User");
        Delete.Table("Type").InSchema("User");
        Delete.Table("Brand").InSchema("User");
        Delete.Table("Branches").InSchema("User");
    }
}

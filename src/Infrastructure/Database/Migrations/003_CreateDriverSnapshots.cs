using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(3, description: "Creates a driver snapshots table")]
public class CreateDriverSnapshots : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE driver_snapshots
            (
                driver_id    BIGINT PRIMARY KEY,
                latitude     DOUBLE PRECISION NOT NULL,
                longitude    DOUBLE PRECISION NOT NULL,
                availability driver_availability NOT NULL,
                segment      vehicle_segment NULL,
                ts           TIMESTAMP NOT NULL DEFAULT now()
            );
            """);
    }

    public override void Down()
    {
        Execute.Sql(
            """
            
            """);
    }
}
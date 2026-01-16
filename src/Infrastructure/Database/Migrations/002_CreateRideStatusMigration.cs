using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(2, description: "Creates a driver availability enum migration")]
public class CreateRideStatusMigration : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TYPE driver_availability AS ENUM (
                        'idle',
                        'searching',
                        'busy',
                        'offline'
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TYPE IF EXISTS driver_availability;
                    """);
    }
}
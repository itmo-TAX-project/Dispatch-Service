using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(1, description: "Creates a vehicle segment enum migration")]
public class CreateVehicleSegmentEnum : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TYPE vehicle_segment AS ENUM (
                        'basic',
                        'mid',
                        'premium'
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TYPE IF EXISTS vehicle_segment;
                    """);
    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultispatialLogistics.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stargate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentSystemId = table.Column<int>(nullable: false),
                    StargateId = table.Column<int>(nullable: false),
                    DestinationSystemId = table.Column<int>(nullable: false),
                    DestinationStargateId = table.Column<int>(nullable: false),
                    XPos = table.Column<long>(nullable: false),
                    YPos = table.Column<long>(nullable: false),
                    ZPos = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stargate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stargate");
        }
    }
}

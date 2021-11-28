using Microsoft.EntityFrameworkCore.Migrations;

namespace Credit_Dharma.Migrations
{
    public partial class Initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Identification = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    AccountSubType = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    OpeningDate = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<double>(nullable: false),
                    Payments = table.Column<int>(nullable: false),
                    PendingPayments = table.Column<int>(nullable: false),
                    MonthlyPay = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Identification);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Lastname = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}

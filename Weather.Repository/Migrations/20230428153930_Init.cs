using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Weather.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Число = table.Column<DateOnly>(type: "date", nullable: false),
                    Время = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Температура = table.Column<double>(type: "double precision", nullable: true),
                    Влажность = table.Column<double>(type: "double precision", nullable: true),
                    Точкаросы = table.Column<double>(name: "Точка росы", type: "double precision", nullable: true),
                    Давление = table.Column<int>(type: "int", nullable: true),
                    Направлениеветра = table.Column<string>(name: "Направление ветра", type: "text", nullable: true),
                    Скоростьветра = table.Column<int>(name: "Скорость ветра", type: "integer", nullable: true),
                    Облачность = table.Column<int>(type: "integer", nullable: true),
                    Нижняяграницаоблачности = table.Column<int>(name: "Нижняя граница облачности", type: "integer", nullable: true),
                    Видимость = table.Column<int>(type: "integer", nullable: true),
                    Погодныеявления = table.Column<string>(name: "Погодные явления", type: "varchar", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}

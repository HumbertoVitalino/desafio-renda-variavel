using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ativos",
                columns: table => new
                {
                    id_ativo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_ativo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_ativo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ativos", x => x.id_ativo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    senha_hash = table.Column<byte[]>(type: "longblob", nullable: false),
                    senha_salt = table.Column<byte[]>(type: "longblob", nullable: false),
                    taxa_corretagem = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cotacoes",
                columns: table => new
                {
                    id_cotacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_ativo = table.Column<int>(type: "int", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    data_hora_cotacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotacoes", x => x.id_cotacao);
                    table.ForeignKey(
                        name: "FK_cotacoes_ativos_id_ativo",
                        column: x => x.id_ativo,
                        principalTable: "ativos",
                        principalColumn: "id_ativo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operacoes",
                columns: table => new
                {
                    id_operacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_ativo = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    tipo_operacao = table.Column<int>(type: "int", nullable: false),
                    valor_corretagem = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    data_hora_operacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operacoes", x => x.id_operacao);
                    table.ForeignKey(
                        name: "FK_operacoes_ativos_id_ativo",
                        column: x => x.id_ativo,
                        principalTable: "ativos",
                        principalColumn: "id_ativo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operacoes_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "posicoes",
                columns: table => new
                {
                    id_posicao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_ativo = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    preco_medio = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    lucro_prejuizo_atual = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posicoes", x => x.id_posicao);
                    table.ForeignKey(
                        name: "FK_posicoes_ativos_id_ativo",
                        column: x => x.id_ativo,
                        principalTable: "ativos",
                        principalColumn: "id_ativo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_posicoes_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ativos_codigo_ativo",
                table: "ativos",
                column: "codigo_ativo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cotacoes_id_ativo_data_hora_cotacao",
                table: "cotacoes",
                columns: new[] { "id_ativo", "data_hora_cotacao" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operacoes_id_ativo",
                table: "operacoes",
                column: "id_ativo");

            migrationBuilder.CreateIndex(
                name: "ix_operacoes_usuario_ativo_data",
                table: "operacoes",
                columns: new[] { "id_usuario", "id_ativo", "data_hora_operacao" });

            migrationBuilder.CreateIndex(
                name: "IX_posicoes_id_ativo",
                table: "posicoes",
                column: "id_ativo");

            migrationBuilder.CreateIndex(
                name: "IX_posicoes_id_usuario_id_ativo",
                table: "posicoes",
                columns: new[] { "id_usuario", "id_ativo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cotacoes");

            migrationBuilder.DropTable(
                name: "operacoes");

            migrationBuilder.DropTable(
                name: "posicoes");

            migrationBuilder.DropTable(
                name: "ativos");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace redeSocial_WebApplication.Migrations
{
    public partial class redeSocial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    usuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    senha = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.usuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Banimentos",
                columns: table => new
                {
                    banID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioID = table.Column<int>(type: "int", nullable: false),
                    usuarioBanID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banimentos", x => x.banID);
                    table.ForeignKey(
                        name: "FK_Banimentos_Usuario_usuarioBanID",
                        column: x => x.usuarioBanID,
                        principalTable: "Usuario",
                        principalColumn: "usuarioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banimentos_Usuario_usuarioID",
                        column: x => x.usuarioID,
                        principalTable: "Usuario",
                        principalColumn: "usuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postagem",
                columns: table => new
                {
                    postID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    conteudoTxt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postagem", x => x.postID);
                    table.ForeignKey(
                        name: "FK_Postagem_Usuario_usuarioID",
                        column: x => x.usuarioID,
                        principalTable: "Usuario",
                        principalColumn: "usuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArquivoMidia",
                columns: table => new
                {
                    arquivoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nomeArmazenamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivoMidia", x => x.arquivoID);
                    table.ForeignKey(
                        name: "FK_ArquivoMidia_Postagem_postID",
                        column: x => x.postID,
                        principalTable: "Postagem",
                        principalColumn: "postID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    comentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postID = table.Column<int>(type: "int", nullable: false),
                    usuarioID = table.Column<int>(type: "int", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.comentID);
                    table.ForeignKey(
                        name: "FK_Comentario_Postagem_postID",
                        column: x => x.postID,
                        principalTable: "Postagem",
                        principalColumn: "postID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentario_Usuario_usuarioID",
                        column: x => x.usuarioID,
                        principalTable: "Usuario",
                        principalColumn: "usuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArquivoMidia_postID",
                table: "ArquivoMidia",
                column: "postID");

            migrationBuilder.CreateIndex(
                name: "IX_Banimentos_usuarioBanID",
                table: "Banimentos",
                column: "usuarioBanID");

            migrationBuilder.CreateIndex(
                name: "IX_Banimentos_usuarioID",
                table: "Banimentos",
                column: "usuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_postID",
                table: "Comentario",
                column: "postID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_usuarioID",
                table: "Comentario",
                column: "usuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Postagem_usuarioID",
                table: "Postagem",
                column: "usuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivoMidia");

            migrationBuilder.DropTable(
                name: "Banimentos");

            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Postagem");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SetUpProtocolsDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FunctionTypes",
                columns: table => new
                {
                    FunctionTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionDescription = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    IconActionClass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionTypes", x => x.FunctionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LineTypes",
                columns: table => new
                {
                    LineTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineTypeDescription = table.Column<string>(nullable: true),
                    LineTypeParentID = table.Column<int>(nullable: false),
                    LineTypeChildID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineTypes", x => x.LineTypeID);
                    table.ForeignKey(
                        name: "FK_LineTypes_LineTypes_LineTypeChildID",
                        column: x => x.LineTypeChildID,
                        principalTable: "LineTypes",
                        principalColumn: "LineTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineTypes_LineTypes_LineTypeParentID",
                        column: x => x.LineTypeParentID,
                        principalTable: "LineTypes",
                        principalColumn: "LineTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialCategories",
                columns: table => new
                {
                    MaterialCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCategories", x => x.MaterialCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolCategories",
                columns: table => new
                {
                    ProtocolCategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolCategories", x => x.ProtocolCategoryTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolComments",
                columns: table => new
                {
                    ProtocolCommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolCommmentType = table.Column<string>(nullable: true),
                    ProtocolCommentDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolComments", x => x.ProtocolCommentID);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolTypes",
                columns: table => new
                {
                    ProtocolTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolTypes", x => x.ProtocolTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDescription = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                });

            migrationBuilder.CreateTable(
                name: "ResourceTypes",
                columns: table => new
                {
                    ResourceTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceTypeDescription = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTypes", x => x.ResourceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Info = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    MaterialCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialID);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialCategories_MaterialCategoryID",
                        column: x => x.MaterialCategoryID,
                        principalTable: "MaterialCategories",
                        principalColumn: "MaterialCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolSubCategories",
                columns: table => new
                {
                    ProtocolSubCategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolSubCategoryTypeDescription = table.Column<string>(nullable: true),
                    ProtocolCategoryTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolSubCategories", x => x.ProtocolSubCategoryTypeID);
                    table.ForeignKey(
                        name: "FK_ProtocolSubCategories_ProtocolCategories_ProtocolCategoryTypeID",
                        column: x => x.ProtocolCategoryTypeID,
                        principalTable: "ProtocolCategories",
                        principalColumn: "ProtocolCategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    FirstAuthor = table.Column<string>(nullable: true),
                    LastAuthor = table.Column<string>(nullable: true),
                    PubMedID = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Journal = table.Column<string>(nullable: true),
                    ResourceTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleID);
                    table.ForeignKey(
                        name: "FK_Articles_ResourceTypes_ResourceTypeID",
                        column: x => x.ResourceTypeID,
                        principalTable: "ResourceTypes",
                        principalColumn: "ResourceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    ProtocolID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UniqueCode = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    Theory = table.Column<string>(nullable: true),
                    ApplicationUserCreatorID = table.Column<int>(nullable: false),
                    ApplicationUserCreatorId = table.Column<string>(nullable: true),
                    ProtocolSubCategoryID = table.Column<int>(nullable: false),
                    ProtocolTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.ProtocolID);
                    table.ForeignKey(
                        name: "FK_Protocols_AspNetUsers_ApplicationUserCreatorId",
                        column: x => x.ApplicationUserCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Protocols_ProtocolSubCategories_ProtocolSubCategoryID",
                        column: x => x.ProtocolSubCategoryID,
                        principalTable: "ProtocolSubCategories",
                        principalColumn: "ProtocolSubCategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Protocols_ProtocolTypes_ProtocolTypeID",
                        column: x => x.ProtocolTypeID,
                        principalTable: "ProtocolTypes",
                        principalColumn: "ProtocolTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagArticles",
                columns: table => new
                {
                    ArticleID = table.Column<int>(nullable: false),
                    TagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagArticles", x => new { x.ArticleID, x.TagID });
                    table.ForeignKey(
                        name: "FK_TagArticles_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagArticles_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    LineID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(nullable: true),
                    ParentLineID = table.Column<int>(nullable: false),
                    LineTypeID = table.Column<int>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: false),
                    LineNumber = table.Column<int>(nullable: false),
                    Timer = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.LineID);
                    table.ForeignKey(
                        name: "FK_Lines_LineTypes_LineTypeID",
                        column: x => x.LineTypeID,
                        principalTable: "LineTypes",
                        principalColumn: "LineTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lines_Lines_ParentLineID",
                        column: x => x.ParentLineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lines_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    LinkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkDescription = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ProtocolID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.LinkID);
                    table.ForeignKey(
                        name: "FK_Links_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialProtocols",
                columns: table => new
                {
                    ProtocolID = table.Column<int>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialProtocols", x => new { x.MaterialID, x.ProtocolID });
                    table.ForeignKey(
                        name: "FK_MaterialProtocols_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialProtocols_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagProtocols",
                columns: table => new
                {
                    ProtocolID = table.Column<int>(nullable: false),
                    TagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagProtocols", x => new { x.TagID, x.ProtocolID });
                    table.ForeignKey(
                        name: "FK_TagProtocols_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagProtocols_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionLines",
                columns: table => new
                {
                    LineID = table.Column<int>(nullable: false),
                    FunctionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionLines", x => new { x.FunctionTypeID, x.LineID });
                    table.ForeignKey(
                        name: "FK_FunctionLines_FunctionTypes_FunctionTypeID",
                        column: x => x.FunctionTypeID,
                        principalTable: "FunctionTypes",
                        principalColumn: "FunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolInstances",
                columns: table => new
                {
                    ProtocolInstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    ProtocolID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CurrentLineID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolInstances", x => x.ProtocolInstanceID);
                    table.ForeignKey(
                        name: "FK_ProtocolInstances_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtocolInstances_Lines_CurrentLineID",
                        column: x => x.CurrentLineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtocolInstances_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ResourceTypeID",
                table: "Articles",
                column: "ResourceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_LineID",
                table: "FunctionLines",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_LineTypeID",
                table: "Lines",
                column: "LineTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ProtocolID",
                table: "Lines",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_LineTypes_LineTypeChildID",
                table: "LineTypes",
                column: "LineTypeChildID");

            migrationBuilder.CreateIndex(
                name: "IX_LineTypes_LineTypeParentID",
                table: "LineTypes",
                column: "LineTypeParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ProtocolID",
                table: "Links",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProtocols_ProtocolID",
                table: "MaterialProtocols",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialCategoryID",
                table: "Materials",
                column: "MaterialCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProductID",
                table: "Materials",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstances_ApplicationUserId",
                table: "ProtocolInstances",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstances_CurrentLineID",
                table: "ProtocolInstances",
                column: "CurrentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstances_ProtocolID",
                table: "ProtocolInstances",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_ApplicationUserCreatorId",
                table: "Protocols",
                column: "ApplicationUserCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_ProtocolSubCategoryID",
                table: "Protocols",
                column: "ProtocolSubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_ProtocolTypeID",
                table: "Protocols",
                column: "ProtocolTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolSubCategories_ProtocolCategoryTypeID",
                table: "ProtocolSubCategories",
                column: "ProtocolCategoryTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TagArticles_TagID",
                table: "TagArticles",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_TagProtocols_ProtocolID",
                table: "TagProtocols",
                column: "ProtocolID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionLines");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "MaterialProtocols");

            migrationBuilder.DropTable(
                name: "ProtocolComments");

            migrationBuilder.DropTable(
                name: "ProtocolInstances");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "TagArticles");

            migrationBuilder.DropTable(
                name: "TagProtocols");

            migrationBuilder.DropTable(
                name: "FunctionTypes");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "MaterialCategories");

            migrationBuilder.DropTable(
                name: "LineTypes");

            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.DropTable(
                name: "ResourceTypes");

            migrationBuilder.DropTable(
                name: "ProtocolSubCategories");

            migrationBuilder.DropTable(
                name: "ProtocolTypes");

            migrationBuilder.DropTable(
                name: "ProtocolCategories");
        }
    }
}

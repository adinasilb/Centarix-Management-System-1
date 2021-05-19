using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FunctionDescriptionEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 602);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 603);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 604);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 704);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 705);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 706);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 707);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 805);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 806);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 807);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 808);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 809);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 810);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 811);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 812);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 813);

            //migrationBuilder.DeleteData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 814);

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 9, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 10, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 11, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 12, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 13, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 20, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 21, 7 });

            //migrationBuilder.DeleteData(
            //    table: "UnitTypeParentCategory",
            //    keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    keyValues: new object[] { 22, 7 });

            //migrationBuilder.AddColumn<string>(
            //    name: "ImageUrl",
            //    table: "ResourceCategories",
            //    nullable: true);

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IncludeVAT",
            //    table: "Requests",
            //    nullable: false,
            //    defaultValue: true,
            //    oldClrType: typeof(bool),
            //    oldType: "bit",
            //    oldNullable: true,
            //    oldDefaultValue: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DescriptionEnum",
            //    table: "FunctionTypes",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "FavoriteProtocols",
            //    columns: table => new
            //    {
            //        FavoriteProtocolID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProtocolID = table.Column<int>(nullable: false),
            //        ApplicationUserID = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FavoriteProtocols", x => x.FavoriteProtocolID);
            //        table.ForeignKey(
            //            name: "FK_FavoriteProtocols_AspNetUsers_ApplicationUserID",
            //            column: x => x.ApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "FavoriteResources",
            //    columns: table => new
            //    {
            //        FavoriteResourceID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ResourceID = table.Column<int>(nullable: false),
            //        ApplicationUserID = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FavoriteResources", x => x.FavoriteResourceID);
            //        table.ForeignKey(
            //            name: "FK_FavoriteResources_AspNetUsers_ApplicationUserID",
            //            column: x => x.ApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FavoriteResources_Resources_ResourceID",
            //            column: x => x.ResourceID,
            //            principalTable: "Resources",
            //            principalColumn: "ResourceID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ShareResources",
            //    columns: table => new
            //    {
            //        ShareResourceID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ResourceID = table.Column<int>(nullable: false),
            //        FromApplicationUserID = table.Column<string>(nullable: true),
            //        ToApplicationUserID = table.Column<string>(nullable: true),
            //        TimeStamp = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ShareResources", x => x.ShareResourceID);
            //        table.ForeignKey(
            //            name: "FK_ShareResources_AspNetUsers_FromApplicationUserID",
            //            column: x => x.FromApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ShareResources_Resources_ResourceID",
            //            column: x => x.ResourceID,
            //            principalTable: "Resources",
            //            principalColumn: "ResourceID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ShareResources_AspNetUsers_ToApplicationUserID",
            //            column: x => x.ToApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "DescriptionEnum",
                value: "AddImage");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 2,
                column: "DescriptionEnum",
                value: "AddTimer");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 3,
                column: "DescriptionEnum",
                value: "AddComment");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 4,
                column: "DescriptionEnum",
                value: "AddWarning");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 5,
                column: "DescriptionEnum",
                value: "AddTip");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "DescriptionEnum",
                value: "AddTable");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 7,
                column: "DescriptionEnum",
                value: "AddTemplate");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 8,
                column: "DescriptionEnum",
                value: "AddStop");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 9,
                column: "DescriptionEnum",
                value: "AddLinkToProduct");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 10,
                column: "DescriptionEnum",
                value: "AddLinkToProtocol");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 11,
                column: "DescriptionEnum",
                value: "AddFile");

            //migrationBuilder.InsertData(
            //    table: "OffDayTypes",
            //    columns: new[] { "OffDayTypeID", "Description" },
            //    values: new object[] { 5, "Unpaid Leave" });

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 1,
            //    column: "ParentCategoryDescription",
            //    value: "Consumables");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 3,
            //    column: "ParentCategoryDescription",
            //    value: "Biological");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 6,
            //    columns: new[] { "CategoryTypeID", "ParentCategoryDescription" },
            //    values: new object[] { 1, "General" });

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 7,
            //    columns: new[] { "IsProprietary", "ParentCategoryDescription" },
            //    values: new object[] { false, "Clinical" });

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 8,
            //    column: "ParentCategoryDescription",
            //    value: "IT");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 9,
            //    column: "ParentCategoryDescription",
            //    value: "Day To Day");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 10,
            //    column: "ParentCategoryDescription",
            //    value: "Travel");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 11,
            //    column: "ParentCategoryDescription",
            //    value: "Advice");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 12,
            //    column: "ParentCategoryDescription",
            //    value: "Regulations");

            //migrationBuilder.UpdateData(
            //    table: "ParentCategories",
            //    keyColumn: "ParentCategoryID",
            //    keyValue: 13,
            //    column: "ParentCategoryDescription",
            //    value: "Government");

            //migrationBuilder.InsertData(
            //    table: "ParentCategories",
            //    columns: new[] { "ParentCategoryID", "CategoryTypeID", "IsProprietary", "ParentCategoryDescription" },
            //    values: new object[,]
            //    {
            //        { 5, 1, false, "Safety" },
            //        { 14, 1, true, "Samples" }
            //    });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 102,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/culture_plates.png", "Cell Culture Plates" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 104,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/tips2.png", "Tips" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 105,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/pipettes.png", "Pipets" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 106,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/tubes.png", "Tubes" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 107,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/robot_consumables_tips.png", "Robot Consumables(Tips,Microplates, Reservoirs)" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 108,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/ddpcr_consumables.png", "DdPCR Consumables (Gaskets, Cartridges, Microplates, Foil seal)" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 109,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/consumables/rtpcr_consumables.png", "RT-PCR" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 201,
            //    column: "ImageURL",
            //    value: "/images/css/CategoryImages/reagents/chemical_powder.png");

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 202,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/antibody.png", "Antibodies" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 203,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/cell_media.png", "Cell Media" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 204,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/chemical_solution2.png", "Solution" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 205,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/kit.png", "Kit" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 206,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/PCR_reagent.png", "PCR" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 207,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/ddPCR_reagent2.png", "RT-PCR" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 208,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/dna_probes2.png", "Probes" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 209,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/primer.png", "Primers" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 210,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/media_supplement.png", "Media Supplement" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 211,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/antibiotics.png", "Antibiotics" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 212,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reagents/restriction_enzyme.png", "Enzyme Restriction" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 301,
            //    column: "ImageURL",
            //    value: "/images/css/CategoryImages/biological/cell1.png");

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 401,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/reusable/all_reusables.png", "Reusable" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 601,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/general/general.png", "General" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 701,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/clinical/edta_tubes3.png", "EDTA Tubes" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 702,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/clinical/serum_tubes.png", "Serum Tubes" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 703,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/clinical/sample_collection.png", "Sample Collection and Processing (Blood, Saliva Collection, Swabsticks, Sepmate, and Ficoll for PBMC, Extraction kits for DNA/RNA)" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 801,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/it/communications.png", "Communications" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 802,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/it/cybersecurity.png", "Cybersecurity" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 803,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/it/hardware3.png", "Hardware" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 804,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/it/general.png", "General" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 901,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/daytoday/taxes2.png", "Bookeeping" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 902,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/daytoday/books.png", "Books" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 903,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/daytoday/branding.png", "Branding" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 904,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/daytoday/company_events.png", "Company Events" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 905,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/daytoday/electricity2.png", "Electricity" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1001,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/travel/conference3.png", "Conference" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1002,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/travel/flight_tickets.png", "Flight Tickets" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1003,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/travel/food.png", "Food" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1004,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/travel/hotels3.png", "Hotels" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1005,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/travel/travel.png", "Travel" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1101,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/advice/business_advice.png", "Business Advice" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1102,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/advice/clinical_regulation2.png", "Clinical Regulations" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1201,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/regulations/regulations.png", "Regulations" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1202,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/regulations/safety.png", "Safety" });

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1203,
            //    column: "ImageURL",
            //    value: "/images/css/CategoryImages/regulations/general.png");

            //migrationBuilder.UpdateData(
            //    table: "ProductSubcategories",
            //    keyColumn: "ProductSubcategoryID",
            //    keyValue: 1301,
            //    columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
            //    values: new object[] { "/images/css/CategoryImages/government/taxes4.png", "Taxes" });

            //migrationBuilder.InsertData(
            //    table: "ProductSubcategories",
            //    columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
            //    values: new object[,]
            //    {
            //        { 118, "/images/css/CategoryImages/consumables/tapestation_consumables.png", 1, "Tapestation Consumables (Screentapes: gDNA/HS/RNA; Markers, Loading Buffers, Loading Tips)" },
            //        { 119, "/images/css/CategoryImages/consumables/sequencing.png", 1, "Sequencing" },
            //        { 101, "/images/css/CategoryImages/consumables/pcr_consumables.png", 1, "PCR" },
            //        { 103, "/images/css/CategoryImages/consumables/petri_dish.png", 1, "Petri Dish" },
            //        { 110, "/images/css/CategoryImages/consumables/fplc_consumables.png", 1, "FPLC Consumables" },
            //        { 111, "/images/css/CategoryImages/consumables/tff_consumables.png", 1, "TFF Consumables" },
            //        { 112, "/images/css/CategoryImages/consumables/column.png", 1, "Column" },
            //        { 113, "/images/css/CategoryImages/consumables/filteration_system.png", 1, "Filtration system" },
            //        { 216, "/images/css/CategoryImages/reagents/nucleic_acid_quantitation.png", 2, "Nucleic Acid Quantitation (DNA/RNA qubit assay, Picogreen assay)" },
            //        { 215, "/images/css/CategoryImages/reagents/TFF_reagent.png", 2, "TFF Reagent" },
            //        { 114, "/images/css/CategoryImages/consumables/flasks.png", 1, "Flasks" },
            //        { 217, "/images/css/CategoryImages/reagents/general_reagents.png", 2, "General" },
            //        { 302, "/images/css/CategoryImages/biological/virus.png", 3, "Virus" },
            //        { 303, "/images/css/CategoryImages/biological/plasmid2.png", 3, "Plasmid" },
            //        { 304, "/images/css/CategoryImages/biological/bacteria.png", 3, "Bacteria" },
            //        { 305, "/images/css/CategoryImages/biological/general.png", 3, "General" },
            //        { 120, "/images/css/CategoryImages/consumables/general.png", 1, "General" },
            //        { 214, "/images/css/CategoryImages/reagents/fplc_reagent.png", 2, "FPLC Reagent" },
            //        { 213, "/images/css/CategoryImages/reagents/rna_enzyme.png", 2, "Enzyme RNA" },
            //        { 913, "/images/css/CategoryImages/daytoday/renovation.png", 9, "Renovation" },
            //        { 116, "/images/css/CategoryImages/consumables/syringes.png", 1, "Syringes" },
            //        { 914, "/images/css/CategoryImages/daytoday/rent2.png", 9, "Rent" },
            //        { 915, "/images/css/CategoryImages/daytoday/shippment.png", 9, "Shipment" },
            //        { 1006, "/images/css/CategoryImages/travel/general.png", 10, "General" },
            //        { 1103, "/images/css/CategoryImages/advice/general.png", 11, "General" },
            //        { 1104, "/images/css/CategoryImages/advice/legal.png", 11, "Legal" },
            //        { 1105, "/images/css/CategoryImages/advice/scientific_advice3.png", 11, "Scientific Advice" },
            //        { 1302, "/images/css/CategoryImages/government/general.png", 13, "General" },
            //        { 912, "/images/css/CategoryImages/daytoday/parking2.png", 9, "Parking" },
            //        { 117, "/images/css/CategoryImages/consumables/covaris_consumables.png", 1, "Covaris Consumables" },
            //        { 906, "/images/css/CategoryImages/daytoday/fees.png", 9, "Fees" },
            //        { 907, "/images/css/CategoryImages/daytoday/food.png", 9, "Food" },
            //        { 908, "/images/css/CategoryImages/daytoday/furniture2.png", 9, "Furniture" },
            //        { 909, "/images/css/CategoryImages/daytoday/general.png", 9, "General" },
            //        { 910, "/images/css/CategoryImages/daytoday/graphics.png", 9, "Graphic" },
            //        { 115, "/images/css/CategoryImages/consumables/bags.png", 1, "Bags" },
            //        { 911, "/images/css/CategoryImages/daytoday/insurance.png", 9, "Insurance" }
            //    });

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 1,
            //    column: "ImageUrl",
            //    value: "rejuvenation_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 2,
            //    column: "ImageUrl",
            //    value: "biomarkers_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 3,
            //    column: "ImageUrl",
            //    value: "delivery_systems_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 4,
            //    column: "ImageUrl",
            //    value: "clinical_trials_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 14,
            //    columns: new[] { "ImageUrl", "ResourceCategoryDescription" },
            //    values: new object[] { "software_image.svg", "Software" });

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 15,
            //    column: "ImageUrl",
            //    value: "learning_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 16,
            //    column: "ImageUrl",
            //    value: "companies_image.svg");

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 17,
            //    column: "ImageUrl",
            //    value: "news_image.svg");

            //migrationBuilder.InsertData(
            //    table: "UnitTypeParentCategory",
            //    columns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    values: new object[,]
            //    {
            //        { 19, 7 },
            //        { 3, 7 },
            //        { 23, 6 },
            //        { 2, 7 },
            //        { 1, 7 },
            //        { 24, 7 },
            //        { 1, 6 },
            //        { 2, 6 },
            //        { 3, 6 },
            //        { 22, 6 },
            //        { 21, 6 },
            //        { 20, 6 },
            //        { 19, 6 },
            //        { 18, 6 },
            //        { 17, 6 },
            //        { 16, 6 },
            //        { 15, 6 },
            //        { 24, 6 },
            //        { 14, 6 },
            //        { 12, 6 },
            //        { 11, 6 },
            //        { 10, 6 },
            //        { 9, 6 },
            //        { 8, 6 },
            //        { 6, 6 },
            //        { 5, 6 },
            //        { 4, 6 },
            //        { 13, 6 },
            //        { 7, 6 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "ProductSubcategories",
            //    columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
            //    values: new object[,]
            //    {
            //        { 501, "/images/css/CategoryImages/safety/protective_wear.png", 5, "PPE (Personal Protective Equipment)" },
            //        { 1409, "/images/css/CategoryImages/samples/media2.png", 14, "Media" },
            //        { 1408, "/images/css/CategoryImages/samples/buffer2.png", 14, "Buffer" },
            //        { 1407, "/images/css/CategoryImages/samples/serum.png", 14, "Serum" },
            //        { 1406, "/images/css/CategoryImages/samples/blood.png", 14, "Blood" },
            //        { 1404, "/images/css/CategoryImages/samples/cell1.png", 14, "Cells" },
            //        { 1403, "/images/css/CategoryImages/samples/dna_probes2.png", 14, "Probes" },
            //        { 1402, "/images/css/CategoryImages/samples/plasmid.png", 14, "Plasmid" },
            //        { 1405, "/images/css/CategoryImages/samples/bacteria2.png", 14, "Bacteria with Plasmids" },
            //        { 502, "/images/css/CategoryImages/safety/safety.png", 5, "Lab Safety" },
            //        { 1401, "/images/css/CategoryImages/samples/virus.png", 14, "Virus" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "UnitTypeParentCategory",
            //    columns: new[] { "UnitTypeID", "ParentCategoryID" },
            //    values: new object[,]
            //    {
            //        { 24, 5 },
            //        { 13, 14 },
            //        { 22, 14 },
            //        { 21, 14 },
            //        { 20, 14 },
            //        { 9, 14 },
            //        { 10, 14 },
            //        { 5, 14 },
            //        { 2, 5 },
            //        { 19, 5 },
            //        { 12, 14 },
            //        { 3, 5 },
            //        { 5, 5 },
            //        { 1, 5 },
            //        { 11, 14 }
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteProtocols_ApplicationUserID",
            //    table: "FavoriteProtocols",
            //    column: "ApplicationUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteResources_ApplicationUserID",
            //    table: "FavoriteResources",
            //    column: "ApplicationUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteResources_ResourceID",
            //    table: "FavoriteResources",
            //    column: "ResourceID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShareResources_FromApplicationUserID",
            //    table: "ShareResources",
            //    column: "FromApplicationUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShareResources_ResourceID",
            //    table: "ShareResources",
            //    column: "ResourceID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShareResources_ToApplicationUserID",
            //    table: "ShareResources",
            //    column: "ToApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProtocols");

            migrationBuilder.DropTable(
                name: "FavoriteResources");

            migrationBuilder.DropTable(
                name: "ShareResources");

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 906);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 907);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 908);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 910);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 911);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 912);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 913);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 914);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 915);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1103);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1104);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1105);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1302);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1401);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1402);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1403);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1404);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1405);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1406);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1407);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1408);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1409);

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 7, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 14, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 15, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 16, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 17, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 18, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 23, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 7 });

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 14);

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ResourceCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionEnum",
                table: "FunctionTypes");

            migrationBuilder.AlterColumn<bool>(
                name: "IncludeVAT",
                table: "Requests",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 1,
                column: "ParentCategoryDescription",
                value: "Plastics");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "ParentCategoryDescription",
                value: "Cells");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                columns: new[] { "CategoryTypeID", "ParentCategoryDescription" },
                values: new object[] { 2, "IT" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                columns: new[] { "IsProprietary", "ParentCategoryDescription" },
                values: new object[] { true, "Samples" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 8,
                column: "ParentCategoryDescription",
                value: "Day To Day");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 9,
                column: "ParentCategoryDescription",
                value: "Travel");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 10,
                column: "ParentCategoryDescription",
                value: "Advisment");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 11,
                column: "ParentCategoryDescription",
                value: "Regulations");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 12,
                column: "ParentCategoryDescription",
                value: "Governments");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 13,
                column: "ParentCategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/PCR.png", "PCR" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 104,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/culture_plates.png", "Cell Culture Plates" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 105,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Petri Dishes" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 106,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Tips" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 107,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/pipettes.png", "Pipets" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/tubes.png", "Tubes" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Robot Tips" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 201,
                column: "ImageURL",
                value: "/images/css/CategoryImages/chemical_powder.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/dna_enzyme.png", "Enzyme" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 203,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/antibody.png", "Antibodies" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 204,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/cell_media.png", "Cell Media" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 205,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/chemical_solution.png", "Chemicals Solution" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 206,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/kit.png", "Kit" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/PCR.png", "PCR" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 208,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/ddPCR.png", "ddPCR" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 209,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "RT-PCR" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 210,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/dna_probes.png", "Probes" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 211,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/oligo.png", "Oligo" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 212,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/media_supplement.png", "Media Supplement" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 301,
                column: "ImageURL",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 401,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Reusables" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/software.png", "Hardware" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 701,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/virus.png", "Virus" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 702,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/plasmid.png", "Plasmid" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 703,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Probes" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/rent.png", "Rent" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 802,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Electricity" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 803,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Parking" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 804,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/renovation.png", "Renovation" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 901,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Conference" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 902,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/flight_tickets.png", "Flight Tickets" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 903,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/hotels.png", "Hotels" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 904,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/food.png", "Food" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 905,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/general.png", "General" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1001,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/legal.png", "Law" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1002,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/sciemtific_advice.png", "Scientific" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1003,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/business_advice.png", "Business" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1004,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/clinical_regulation.png", "Clinical Experiments" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/general.png", "General" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1101,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/safety.png", "Safety" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1102,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/general.png", "General" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1201,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/taxes.png", "Tax" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1202,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/furniture.png", "Fees" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1203,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1301,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/general.png", "General" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 809, "/images/css/CategoryImages/shippment.png", 8, "Shipment" },
                    { 811, "/images/css/CategoryImages/books.png", 8, "Books And Journal" },
                    { 807, "/images/css/CategoryImages/company_events.png", 8, "Company Events" },
                    { 805, "/images/css/CategoryImages/insurance.png", 8, "Insurance" },
                    { 604, "/images/css/CategoryImages/general.png", 6, "General" },
                    { 814, "/images/css/CategoryImages/general.png", 8, "General" },
                    { 808, "/images/css/CategoryImages/branding.png", 8, "Branding" },
                    { 812, "/images/css/CategoryImages/bookeeping.png", 8, "Bookkeeping" },
                    { 602, "/images/css/CategoryImages/communications.png", 6, "Communication" },
                    { 603, "/images/css/CategoryImages/software.png", 6, "Cybersecurity" },
                    { 810, "/images/css/CategoryImages/food.png", 8, "Food" },
                    { 806, "/images/css/CategoryImages/furniture.png", 8, "Furniture" },
                    { 813, "/images/css/CategoryImages/furniture.png", 8, "Graphics" },
                    { 704, null, 7, "Cells" },
                    { 705, null, 7, "Bacteria with Plasmids" },
                    { 706, null, 7, "Blood" },
                    { 707, null, 7, "Serum" }
                });

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 14,
                column: "ResourceCategoryDescription",
                value: "Softwares");

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[,]
                {
                    { 9, 7 },
                    { 20, 7 },
                    { 21, 7 },
                    { 22, 7 },
                    { 13, 7 },
                    { 12, 7 },
                    { 11, 7 },
                    { 10, 7 }
                });
        }
    }
}

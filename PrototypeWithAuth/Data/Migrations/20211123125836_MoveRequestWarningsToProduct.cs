using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveRequestWarningsToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO ProductComments(ObjectID, ApplicationUserID, CommentText, CommentTimeStamp, IsDeleted, CommentTypeID) (SELECT r.ProductID, ApplicationUserID, CommentText, CommentTimeStamp, rc.IsDeleted, CommentTypeID FROM RequestComments rc INNER JOIN Requests r ON r.RequestID = rc.ObjectID WHERE rc.CommentTypeID=2)");
            migrationBuilder.Sql("DELETE FROM RequestComments WHERE CommentTypeID=2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.Sql("INSERT INTO RequestComments(ObjectID, ApplicationUserID, CommentText, CommentTimeStamp, IsDeleted, CommentTypeID) (SELECT r.RequestID, ApplicationUserID, CommentText, CommentTimeStamp, pc.IsDeleted, CommentTypeID FROM ProductComments pc INNER JOIN Requests r ON r.ProductID = pc.ObjectID WHERE pc.CommentTypeID=2)");
            migrationBuilder.Sql("DELETE FROM ProductComments WHERE CommentTypeID=2");
        }
    }
}

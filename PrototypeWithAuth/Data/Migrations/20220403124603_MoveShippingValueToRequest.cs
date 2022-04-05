using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveShippingValueToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update Requests set Requests.Shipping =ISNULL((select pr.shipping from ParentRequests pr join requests r on pr.ParentRequestID = r.ParentRequestID join payments pa on pa.RequestID = r.RequestID where pa.ShippingPaidHere = 'true' and Requests.ParentRequestID = pr.ParentRequestID and pa.RequestID = Requests.RequestID), 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update Payments set shippingpaidhere = 'true' from Payments p inner join requests r on r.RequestID = p.RequestID where r.Shipping > 0 and p.InstallmentNumber = 1 ");
            migrationBuilder.Sql("update ParentRequests set ParentRequests.Shipping = isnull((select r.shipping from Requests r join payments pa on r.requestid = pa.RequestID where pa.ShippingPaidHere = 'true' and r.ParentRequestID = ParentRequests.ParentRequestID and pa.requestid = r.RequestID), 0)");
        
        }
    }
}

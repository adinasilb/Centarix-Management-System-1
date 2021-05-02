
$(".addMaterial").click(function(){
	 var url="/Protocols/AddMaterialModal?materialTypeID="+$(this).val()+"&ProtocolID="+$(".createProtocolMasterProtocolID").val();
	 $.fn.CallPageRequest(url, "addMaterial");
});
$(".open-material-info").click(function(e){
	 e.preventDefault();
	 var url="/Protocols/MaterialInfoModal?materialID="+$(this).val();
	 $.fn.CallPageRequest(url, "materialInfo");
});
$(".open-material-product").click(function(e){
	 e.preventDefault();
	 var url="/Protocols/ProtocolsProductDetails?productID="+$(this).val();
	 $.fn.CallPageRequest(url, "summary");
});
$(".link-material-to-product").click(function(e){
	 e.preventDefault();
		 var url="/Protocols/LinkMaterialToProductModal?materialID="+$(this).attr("materialID");
	 $.fn.CallPageRequest(url, "materialInfo");
});


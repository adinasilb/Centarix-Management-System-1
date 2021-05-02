
$(".addMaterial").click(function(){
	 var url="/Protocols/AddMaterialModal?materialTypeID="+$(this).val()+"&ProtocolID="+$(".createProtocolMasterProtocolID").val();
	 $.fn.CallPageRequest(url, "addMaterial");
});
$(".open-material-info").click(function(e){
	 e.preventDefault();
	 var url="/Protocols/MaterialInfoModal?materialID="+$(this).val();
	 $.fn.CallPageRequest(url, "materialInfo");
});

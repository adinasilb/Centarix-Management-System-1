$('.materialForm').validate({

		 normalizer: function( value ) {
			 return $.trim( value );
		},
		rules:{

		   'Material.Name': {
                eitherSerialNumberOrName:true
            },
			"Material.Product.SerialNumber": {
				remote:{
				url: '/Protocols/CheckIfSerialNumberExists',
				type: 'POST',
				data: { "SerialNumber":function(){ return $("#Material_Product_SerialNumber").val()}},
			},
				 eitherSerialNumberOrName:true
			},
	},
	messages:{
	   "Material.Product.SerialNumber": {
            remote: "This serial number does not exist."
        },
		}
});

$.validator.addMethod("eitherSerialNumberOrName", function (value, element) {
	return ( $("#Material_Name").val()!="" ||  $("#Material_Product_SerialNumber").val()!="")
}, 'Must fill out name or serial number.');


$("body, .modal").off("change", '#Material_Name, #Material_Product_SerialNumber ').on("change", '#Material_Name, #Material_Product_SerialNumber ' , function(){
	//alert("in change vendor")
	//$('#Request_0__Product_CatalogNumber').valid();
		$('.error').addClass("beforeCallValid");
		$("#Material_Name").valid();
		$("#Material_Product_SerialNumber").valid();
		$(".error:not(.beforeCallValid)").addClass("afterCallValid")
		$(".error:not(.beforeCallValid)").removeClass("error")
		$("label.afterCallValid").remove()
		$(".error").removeClass('beforeCallValid')
		$(".afterCallValid").removeClass('error')
		$(".afterCallValid").removeClass('afterCallValid')

});
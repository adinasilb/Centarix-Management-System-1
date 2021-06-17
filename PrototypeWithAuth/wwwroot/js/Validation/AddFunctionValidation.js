$(function () {
	$('.addFunctionForm').validate({

		 normalizer: function( value ) {
			 return $.trim( value );
		},
		rules:{

			"FunctionLine.Product.SerialNumber": {
				remote:{
					url: '/Protocols/CheckIfSerialNumberExists',
					type: 'POST',
					async: false,
					data: { "SerialNumber":function(){ return $("#FunctionLine_Product_SerialNumber").val()}},
				}
			},				
			"FunctionLine.Protocol.UniqueCode": {
				remote:{
					url: '/Protocols/CheckIfProtocolUniqueNumberExists',
					type: 'POST',
					async: false,
					data: { "UniqueNumber":function(){ return $("#FunctionLine_Protocol_UniqueCode").val()}},
				}
			}
	},
	messages:{
	   "FunctionLine.Product.SerialNumber": {
            remote: "This serial number does not exist."
        },
		"FunctionLine.Protocol.UniqueCode": {
            remote: "This unique code does not exist."
        },
	}
});
})
$(function () {
	$('.addFunctionForm').validate({

		 normalizer: function( value ) {
			 return $.trim( value );
		},
		rules:{
			"FunctionLine.ProductID" : {
				selectRequired: true	
			},	
			"FunctionLine.ProtocolID" : {
				selectRequired: true	
			},
			"FunctionLine.Product.SerialNumber": {
				remote:{
					url: '/Protocols/CheckIfSerialNumberExists',
					type: 'POST',
					async: false,
					data: { "SerialNumber":function(){ return $("#FunctionLine_Product_SerialNumber").val()}},
				}
			},	
			"Function.Timer":{
				required:true,
				validTimeWithSeconds:true
			},
			"uniqueCode": {
				remote:{
					url: '/Protocols/CheckIfProtocolUniqueNumberExists',
					type: 'POST',
					async: false,
					data: { "UniqueNumber":function(){ return $("#uniqueCode").val()}}
				}
			},
				PicturesInput :{
			fileRequired : true			
		},
					FilesInput :{
			fileRequired : true			
		},
					},
	messages:{
	   "FunctionLine.Product.SerialNumber": {
            remote: "This serial number does not exist."
        },
		"uniqueCode": {
            remote: "This unique code does not exist."
        },
	},
	 ignore: "input[type='file']"
});
})
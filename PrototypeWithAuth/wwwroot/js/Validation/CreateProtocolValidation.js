$(function () {
	$('.createProtocolForm').validate({
		normalizer: function (value) {
			return $.trim(value);
		},
		rules: {

			"Protocol.Name": {
				required: true
			},
			"Protocol.ProtocolSubCategory.ProtocolCategoryTypeID": {
				selectRequired: true
			},
			"Protocol.ProtocolSubCategoryID": {
				selectRequired: true
			},
			"Protocol.UniqueCode": {
			    required: true,
				remote:{
					url: '/Protocols/ValidateUniqueProtocolNumber',
					type: 'POST',
					async: false,
					data: { "UniqueNumber":function(){ return $("#Protocol_UniqueCode").val()}, "ProtocolID":function(){ return $("#Protocol_ProtocolID").val()}}
				}
			}
		},
		messages:{
			"Protocol.UniqueCode": {
				remote: "This unique code already exists."
			},
	},
	});
});


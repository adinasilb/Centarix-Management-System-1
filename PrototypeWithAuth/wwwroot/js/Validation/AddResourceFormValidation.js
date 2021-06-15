$.validator.addMethod("IsPubMedOrHasTitle", function (value, element) {
	return $("#Resource_Title").val() != "" || $("#Resource_PubMedID").val() != "";
}, 'Must Have Title');

$('.add-resource-form').validate({
	normalizer: function (value) {
		return $.trim(value);
	},
	rules: {
		"Resource.Title": {
			IsPubMedOrHasTitle : true
		}
	}
});
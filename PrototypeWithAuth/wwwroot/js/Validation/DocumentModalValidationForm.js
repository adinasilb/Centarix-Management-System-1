$('.documentModalForm').validate({
     normalizer: function( value ) {
    return $.trim( value );
  },
    rules :{

        "FilesToSave": { required: true, extension: "jpg|jpeg|png|pdf|ppt|pptx" }
    }
})
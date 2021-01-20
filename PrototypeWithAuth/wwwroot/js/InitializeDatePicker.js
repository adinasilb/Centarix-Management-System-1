 $('.datepicker').pickadate({format:'dd/mm/yyyy', formatSubmit:'yyyy-mm-dd',
                onSet : function(context )
                {   console.log("in on set")
                    var date = moment(context.select).format("yyyy-mm-dd")
                    console.log(context)
                    $('.picker__input--target').attr("data-val",date);
                    $('.picker__input--target').attr("value", moment(context.select));
                },
              });
(function($)
{
	  if (window.File && window.FileReader)
	  {
	    $("#recoveryrequest input[name=configfile]").on("change", function (e)
	    {
	      if (e.target.files && e.target.files.length != 0)
	      {
	        var file = e.target.files[0];
	        var reader = new FileReader();
	        reader.onload = (function (f)
	        {
	          return function (e)
	          {
	            $("#recoveryrequest input[name=config]").val(e.target.result);
	          };
	        })(file);
	        reader.readAsDataURL(file);
	      }
	    });
	  }

	$("#recoveryrequest").on("submit", function(e)
	{
		var $f = $(this);
		$.ajax({
			method:"POST",
			url:"/node",
			dataType:"json",
			data:$(this).serialize(),
			success:function()
			{
				$(".fields", $f).hide();
				$(".success", $f).show();
			},
			error:function(xhr, status, err)
			{
				$("div.error", $f).html(err || status);
			}
		});
		return false;
	});

})($j);


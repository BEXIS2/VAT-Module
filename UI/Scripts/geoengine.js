$(document).ready(function (e) {
    console.log("vat ready");
});

$('#Time').on('change', function () {

    console.log("time changed");

    var variable = $(this).val();
    var id = $("#Id").val();
    console.log($(this));
    console.log(Id);

    $.get("/vat/config/DisplayPattern/", { id, variable }, function (response) {
        //alert(parentId);

        var tf = $('#TimeFormat');
        tf.val(response);

        if (response === "") {
            tf.prop("disabled", false);
        }
        else {
            tf.prop("disabled", true);
        }
    })
})


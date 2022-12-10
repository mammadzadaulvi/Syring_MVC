$(function ($) {
    $(document).on('click', '#category', function () {
        var id = $(this).data('id');
        console.log("test");

        $.ajax({
            method: "GET",
            url: "/faq/QuestionFilter",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result)
                $('.Newquestion').empty().append("");
                $('.Newquestion').append(result);
            }
        })
    })
});


$(function ($) {
    $(document).on('click', '#categoryproduct', function () {
        var id = $(this).data('id');
        console.log("test");

        $.ajax({
            method: "GET",
            url: "/shop/ProductFilter",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result)
                $('#Newproduct').empty().append("");
                $('#Newproduct').append(result);
            }
        })
    })
});







$(function ($) {
    $(document).on('click', '#addToCart', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/add",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result); 
                //$('#cart-id').append('<ol><li>html ${} data</li></ol>')
          
            }
        })
    })




    $(document).on('click', '#deleteButton', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/delete",
            data: {
                id: id
            },
            success: function (result) {
                $(`.basketProduct[id=${id}]`).remove();

            }
        })
    })

    //$(document).on('click', '#dltbasket', function () {

    //    ajax({
    //        $(document).getElementById('#deletebasket').insertAdjacentHTML += ''
    //    })
        
    //})


    //$(document).on('click', '#clear', function () {
    //    var id = $(this).data('id');
    //    //$(document).getElementById('#spe').insertAdjacentHTML("beforebegin", '<ol><li>html data</li></ol>')

    //    $.ajax({
    //        method: "POST",
    //        url: "/basket",
    //        data: {
    //            id: id
    //        },
    //        success: function (result) {
    //            $(document).getElementById('#spe').insertAdjacentHTML("beforebegin", '<ol><li>html data</li></ol>')

    //        }
    //    })
    //})



    /*$(document).getElementById('tag-id').insertAdjacentHTML("beforebegin", '<ol><li>html data</li></ol>')*/
    


    $(document).on('click', '#upcount', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/upcount",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }
        })
    })


    $(document).on('click', '#downcount', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/downcount",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }
        })
    })
})






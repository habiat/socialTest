
"use strict";
var SocialApp = (function () {
    var addAntiForgeryToken = function (data) {
        //if the object is undefined, create a new one.
        if (!data) {
            data = {};
        }
        //add token
        var tokenInput = $('input[name=__RequestVerificationToken]').val();
        if (tokenInput.length) {
            data.__RequestVerificationToken = tokenInput.value;
        }
        return data;
    };
    var ajaxCall = function (element, url, postData) {
        $.ajax({
            cache: false,
            type: "POST",
            url: url,
            data: postData,
            success: function (data, textStatus, jqXHR) {
                console.log(data);
                if (data.Result) {
                    //reload grid
                    //updateTable('#resources-grid');

                    //clear input value
                    $("#ResourceName").val('');
                    $("#ResourceValue").val('');

                } else {
                    //display errors if returned
                    displayError(data);

                }
            },
            complete: function (jqXHR, textStatus) {
                $('#' + element).attr('disabled', false);
            }
        });
    };

    var displayError = function (e) {
        if (e.error) {
            if ((typeof e.error) == 'string') {
                //single error
                //display the message
                alert(e.error);
            } else {
                //array of errors
                var message = "The following errors have occurred:";
                //create a message containing all errors.
                $.each(e.error,
                    function (key, value) {
                        if (value.errors) {
                            message += "\n";
                            message += value.errors.join("\n");
                        }
                    });
                //display the message
                alert(message);
            }
            //ignore empty error
        } else if (e.errorThrown) {
            alert('Error happened');
        }
    }

    return {
        init: function () {
            $('#requestFriend').click(function () {
                $('#requestFriend').attr('disabled', true);

                var postData = {
                    uId: window.location.href.split('/')[4],
                };
                addAntiForgeryToken(postData);
                ajaxCall("requestFriend", requestFriendurl, postData);
              
            });
            $('#cancelRequest').click(function () {
                $('#cancelRequest').attr('disabled', true);

                var postData = {
                    uId: window.location.href.split('/')[4],
                };
                addAntiForgeryToken(postData);
                ajaxCall("cancelRequest", cancelRequesturl, postData);
               
            });
        },
        getParameterByName: function (name, url = window.location.href) {
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        },
         

    }
})();




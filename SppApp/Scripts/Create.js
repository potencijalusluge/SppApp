$(document).ready(function () {
    AddClickEvents();
});


function deleteErrorMessage(item) {
    $(item).closest('div').next().text('');
};

function checkFile(item) {
    $(item).closest('div').next().text('');
    var extension = $(item).val().split('.').pop().toLowerCase();
    // Create array with the files extensions that we wish to upload
    var validExtensions = ['jpeg', 'jpg', 'zip', 'pdf', 'docx', 'doc', 'xls', 'xlsx', 'odt', 'ods', 'fodt', 'fods'];
    //Check file extension in the array.if -1 that means the file extension is not in the list.
    if ($('#loginLink').length) {
        $(item).closest('div').next().addClass("text-danger").text("Greška! Samo prijavljeni korisnici mogu dodavati datoteke.").show();
        $(item).replaceWith($(item).val('').clone(true));
    } else {
        if ($.inArray(extension, validExtensions) == -1 && item.files.length > 0) {
            $(item).closest('div').next().addClass("text-danger").text("Greška! Nedopušten format datoteke.").show();
            // Clear fileuload control selected file
            $(item).replaceWith($(item).val('').clone(true));
            //Disable Submit Button
            //$('#submit').prop('disabled', true);
        }
        else {
            // Check and restrict the file size to 32 KB.
            if ($(item).get(0).files[0].size > (20971520)) {
                $(item).closest('div').next().addClass("text-danger").text("Greška!! Najveæa dopuštena velièina datoteke je 30 MB.").show();
                // Clear fileuload control selected file
                $(item).replaceWith($(item).val('').clone(true));
                //Disable Submit Button
                //$('#submit').prop('disabled', true);
            }
            else {
                //Clear and Hide message span
                $(item).closest('div').next().text('');
                //Enable Submit Button
                //$('#submit').prop('disabled', false);
            }
        }
    }
};
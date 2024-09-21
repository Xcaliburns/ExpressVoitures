// wwwroot/js/UpDateFileName.js
document.addEventListener('DOMContentLoaded', function() {
    var input = document.getElementById('file-upload');
    if (input) {
        input.addEventListener('change', function() {
            if (input.files.length > 0) {
                updateFileName();
            }
        });
    } else {
        console.error("File input element not found.");
    }
});

function updateFileName() {
    var input = document.getElementById('file-upload');
    var fileName = input.files.length > 0 ? input.files[0].name : '';
    var fileNameElement = document.getElementById('file-name');
    if (fileNameElement) {
        // Display the file name safely
        fileNameElement.textContent = fileName;
        console.log("File name updated successfully.");
    } else {
        console.error("File name element not found.");
    }
}

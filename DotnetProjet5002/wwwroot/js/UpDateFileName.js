// wwwroot/js/UpDateFileName.js
document.addEventListener('DOMContentLoaded', function() {
    var input = document.getElementById('file-upload');
    if (input) {
        input.addEventListener('change', function() {
            if (input.files.length > 0) {
                UpDateFileName();
            }
        });
    } else {
        console.error("File input element not found.");
    }
});

function UpDateFileName() {
    var input = document.getElementById('file-upload');
    var file = input.files.length > 0 ? input.files[0] : null;
    var thumbnailElement = document.getElementById('file-thumbnail');
    
    if (file && thumbnailElement) {
        var reader = new FileReader();
        reader.onload = function(e) {
            thumbnailElement.src = e.target.result;
            thumbnailElement.style.display = 'block';
        };
        reader.readAsDataURL(file);
        console.log("UpDateFileName.js loaded successfully.");
    } else {
        console.error("File or thumbnail element not found.");
    }
}

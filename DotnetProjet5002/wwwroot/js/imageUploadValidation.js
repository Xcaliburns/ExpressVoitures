
document.addEventListener('DOMContentLoaded', function () {
    var input = document.getElementById('file-upload');
    var fileNameElement = document.getElementById('file-name');
    var imageFileError = document.getElementById('imageFileError');
    var form = document.querySelector('form');

    if (input) {
        input.addEventListener('change', function () {
            if (input.files.length > 0) {
                var file = input.files[0];
                var validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];

                if (validImageTypes.includes(file.type)) {
                    fileNameElement.textContent = file.name;
                    imageFileError.textContent = ''; // Clear any previous error message
                } else {
                    fileNameElement.textContent = '';
                    imageFileError.textContent = 'Veuillez sélectionner un fichier image valide (JPEG, PNG, GIF).';
                }
            }
        });
    } else {
        console.error("File input element not found.");
    }

    form.addEventListener('submit', function (event) {
        if (input.files.length > 0) {
            var file = input.files[0];
            var validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];

            if (!validImageTypes.includes(file.type)) {
                event.preventDefault();
                imageFileError.textContent = 'Veuillez sélectionner un fichier image valide (JPEG, PNG, GIF).';
            }
        }
    });
});

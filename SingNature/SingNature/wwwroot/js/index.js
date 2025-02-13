document.addEventListener("DOMContentLoaded", function () {
    const uploadImageBtn = document.getElementById("uploadImageBtn");
    const searchByImageBtn = document.getElementById("searchByImageBtn");
    const imageUpload = document.getElementById("imageUpload");
    const imagePreviewContainer = document.getElementById("imagePreviewContainer");
    const imagePreview = document.getElementById("imagePreview");
    const processingMessage = document.getElementById("processingMessage");

    if (uploadImageBtn && imageUpload) {
        // Click "Upload Image" button to open file picker
        uploadImageBtn.addEventListener("click", function () {
            imageUpload.click();
        });

        // When an image is selected
        imageUpload.addEventListener("change", function () {
            if (this.files.length > 0) {
                const file = this.files[0];

                // Show the image preview
                const reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    imagePreviewContainer.style.display = "block";
                    searchByImageBtn.disabled = false;
                };
                reader.readAsDataURL(file);
            }
        });

        // When "Search by Image" button is clicked
        searchByImageBtn.addEventListener("click", function () {
            if (imageUpload.files.length > 0) {
                const file = imageUpload.files[0];
                const formData = new FormData();
                formData.append("image", file);

                // Show "Processing" message
                processingMessage.style.display = "block";

                fetch('/api/ImageApi/predict', {
                    method: 'POST',
                    body: formData
                })
                .then(response => response.json())
                .then(data => {
                    processingMessage.style.display = "none"; // Hide processing message
                    if (data.species && data.species.length > 0) {
                        const speciesString = data.species.join(",");
                        window.location.href = `/Search/Results?keyword=${encodeURIComponent(speciesString)}`;
                    } else {
                        alert("No species detected.");
                    }
                })
                .catch(error => {
                    processingMessage.style.display = "none"; // Hide processing message
                    console.error("Error:", error);
                    alert("Error processing the image.");
                });
            }
        });
    }
});


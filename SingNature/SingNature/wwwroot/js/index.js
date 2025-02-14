document.addEventListener("DOMContentLoaded", function () {
    const uploadImageBtn = document.getElementById("uploadImageBtn");
    const searchByImageBtn = document.getElementById("searchByImageBtn");
    const imageUpload = document.getElementById("imageUpload");
    const imagePreviewContainer = document.getElementById("imagePreviewContainer");
    const imagePreview = document.getElementById("imagePreview");
    const processingMessage = document.getElementById("processingMessage");

    if (uploadImageBtn && imageUpload) {
        // Open file picker
        uploadImageBtn.addEventListener("click", function () {
            imageUpload.click();
        });

        // Handle image upload
        imageUpload.addEventListener("change", function () {
            if (this.files.length > 0) {
                const file = this.files[0];

                // Show image preview
                const reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    imagePreviewContainer.classList.remove("d-none");
                };
                reader.readAsDataURL(file);

                // Enable "Search By Image" button
                searchByImageBtn.disabled = false;
                searchByImageBtn.classList.remove("btn-outline-light", "disabled");
                searchByImageBtn.classList.add("btn-success");
                searchByImageBtn.style.cursor = "pointer"; 
                searchByImageBtn.setAttribute("aria-disabled", "false"); 
            }
        });

        // Handle "Search By Image" button click
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
                        alert("No species detected. Try uploading another image.", "warning");
                    }
                })
                .catch(error => {
                    processingMessage.style.display = "none"; // Hide processing message
                    console.error("Error:", error);
                    alert("Error processing the image. Please try again later.", "warning");
                });
            } else {
                showAlert("Please upload an image before searching.", "warning");
            }
        });
    }

    function showAlert(message, type) {
        const alertBox = document.createElement("div");
        alertBox.className = `alert alert-${type} alert-dismissible fade show`;
        alertBox.role = "alert";
        alertBox.innerHTML = `${message} <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`;
        document.body.prepend(alertBox);
        setTimeout(() => alertBox.remove(), 5000);
    }
});


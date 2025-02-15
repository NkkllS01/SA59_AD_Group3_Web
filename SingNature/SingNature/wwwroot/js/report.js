let map, marker;

document.addEventListener("DOMContentLoaded", function () {
    console.log("report.js loaded!");


    initMap();


    document.getElementById("image").addEventListener("change", function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                const preview = document.getElementById("imagePreview");
                preview.src = e.target.result;
                preview.style.display = "block";
            };
            reader.readAsDataURL(file);
        }
    });


    document.getElementById("sightingForm").addEventListener("submit", function (event) {
        event.preventDefault();

        const fileInput = document.getElementById("image").files[0];
        const specieId = document.getElementById("species").value;
        const details = document.getElementById("description").value;
        const latitude = parseFloat(document.getElementById("latitude").value);
        const longitude = parseFloat(document.getElementById("longitude").value);

        if (!specieId || !details.trim() || !fileInput || isNaN(latitude) || isNaN(longitude)) {
            alert(" Missing required fields.");
            return;
        }

        const formData = new FormData();
        formData.append("specieId", specieId);
        formData.append("details", details);
        formData.append("file", fileInput);
        formData.append("latitude", latitude);
        formData.append("longitude", longitude);

        console.log(" Sending FormData:", [...formData.entries()]);

        fetch("/Sightings/UploadSighting", {
            method: "POST",
            body: formData,
            credentials: "include"
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Server Error: ${response.status} ${response.statusText}`);
                }
                return response.text();
            })
            .then(text => {
                try {
                    return text ? JSON.parse(text) : {};
                } catch (error) {
                    console.warn("Server returned non-JSON response:", text);
                    throw new Error("Invalid JSON response from server.");
                }
            })
            .then(data => {
                console.log(" API Response:", data);
                document.getElementById("success-message").style.display = "block";
                setTimeout(() => location.reload(), 1500);
            })
            .catch(error => {
                console.error(" Upload failed:", error);
                document.getElementById("error-message").innerText = "Upload failed: " + (error.message || "Unknown error.");
                document.getElementById("error-message").style.display = "block";
            });
    });
});


function initMap() {
    console.log(" Initializing Google Map...");

    const defaultLocation = { lat: 1.291896, lng: 103.776642 };

    map = new google.maps.Map(document.getElementById("map"), {
        center: defaultLocation,
        zoom: 15
    });

    marker = new google.maps.Marker({
        position: defaultLocation,
        map: map,
        draggable: true
    });

    updateLocation(defaultLocation.lat, defaultLocation.lng);

    google.maps.event.addListener(marker, "dragend", function (event) {
        updateLocation(event.latLng.lat(), event.latLng.lng());
    });

    google.maps.event.addListener(map, "click", function (event) {
        marker.setPosition(event.latLng);
        updateLocation(event.latLng.lat(), event.latLng.lng());
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                const userLocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map.setCenter(userLocation);
                marker.setPosition(userLocation);
                updateLocation(userLocation.lat, userLocation.lng);
            },
            function () {
                console.warn(" Geolocation failed. Using default location.");
            }
        );
    }
}


function updateLocation(lat, lng) {
    document.getElementById("latitude").value = lat;
    document.getElementById("longitude").value = lng;
    document.getElementById("timestamp").value = new Date().toISOString();
}

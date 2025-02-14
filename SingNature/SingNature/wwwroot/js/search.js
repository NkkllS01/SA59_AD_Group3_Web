document.addEventListener("DOMContentLoaded", function () {
    const searchForm = document.getElementById("searchForm");
    const searchInput = document.getElementById("searchSpecieInput");

    if (searchForm) {
        searchForm.addEventListener("submit", async function(event) {
            event.preventDefault();

            let keyword = searchInput.value.trim();
            if (!keyword) {
                showAlert("Please enter a search term before searching.", "warning");
                return
            }

            try {
                // Show loading spinner
                showLoadingSpinner(true);

                let sightingsResponse = fetch(`/api/sightings/search/${encodeURIComponent(keyword)}`);
                let speciesResponse = fetch(`/api/species/search/${encodeURIComponent(keyword)}`);

                let [sightingsData, speciesData] = await Promise.all([
                    sightingsResponse.then(res => res.ok ? res.json() : []),
                    speciesResponse.then(res => res.ok ? res.json() : [])
                ]);

                sessionStorage.setItem("searchResults", JSON.stringify({ keyword, sightings: sightingsData, species: speciesData }));

                window.location.href = `/Search/Results?keyword=${encodeURIComponent(keyword)}`;
            } catch (error) {
                console.error("Error fetching search results:", error);
                showAlert("An error occurred while searching. Please try again.", "danger");
            } finally {
                // Hide loading spinner
                showLoadingSpinner(false);
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

    function showLoadingSpinner(show) {
        const spinner = document.getElementById("loadingSpinner");
        if (spinner) {
            spinner.style.display = show ? "block" : "none";
        }
    }
});


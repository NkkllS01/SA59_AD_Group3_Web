document.getElementById("searchForm").addEventListener("submit", async function(event) {
    event.preventDefault();

    let keyword = document.getElementById("searchSpecieInput").value.trim();

    if (!keyword) {
        alert("Please enter a search term.");
        return;
    }

    try {
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
        alert("An error occurred while searching. Please try again.");
    }
});
function initMap() {
    console.log("Initializing Google Map...");

    const mapElement = document.getElementById("map");
    if (!mapElement) {
        console.error("ERROR: Map container #map not found in the DOM.");
        return;
    }

    const map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 1.29245, lng: 103.77657 },
        zoom: 12,
        mapId: "53f04509b9ddcd70",
    });

    const input = document.getElementById("searchBox");
    const searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });

    let markers = [];

    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();
        if (places.length === 0) {
            console.warn("No places found.");
            return;
        }

        markers.forEach(marker => marker.setMap(null));
        markers = [];

        const bounds = new google.maps.LatLngBounds();

        places.forEach(place => {
            if (!place.geometry || !place.geometry.location) {
                console.warn("Returned place contains no geometry.");
                return;
            }

            if (place.geometry.viewport) {
                map.fitBounds(place.geometry.viewport);
            } else {
                map.setCenter(place.geometry.location);
                map.setZoom(14);
            }

            const marker = new google.maps.Marker({
                map,
                position: place.geometry.location,
                title: place.name,
                visible: true
            });

            markers.push(marker);
            bounds.extend(place.geometry.location);
        });
        map.fitBounds(bounds);
    });
    fetchSightings(map);
}

function fetchSightings(map) {
fetch("/api/sightings") 
    .then(response => response.json())
    .then(data => {
        console.log("Fetched sightings:", data);
        addSightingsToMap(map, data);
    })
    .catch(error => console.error("ERROR: Failed to fetch sightings data", error));
}

function addSightingsToMap(map, sightings) {
    const markers = []; 
    sightings.forEach(sighting => {
        const position = { lat: sighting.latitude, lng: sighting.longitude };

        const marker = new google.maps.marker.AdvancedMarkerElement({
            map: map,
            position: position,
            title: sighting.specieName,
        });

        const infoWindow = new google.maps.InfoWindow({
            content: `<h4>${sighting.specieName}</h4>
                      <p><b>Reported by:</b> ${sighting.userName}</p>`,
        });

        marker.addListener("gmp-click", (event) => {
            infoWindow.open(map, marker);
        });

        markers.push(marker);
    });

    new markerClusterer.MarkerClusterer({
        map,
        markers: markers,
        algorithm: new markerClusterer.GridAlgorithm({ gridSize: 50 }),
        renderer: new markerClusterer.DefaultRenderer(),
    });
}       

window.initMap = initMap;
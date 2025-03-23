window.initializeMap = () => {
    var map = L.map('map').setView([51.505, -0.09], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    var geocoder = L.Control.Geocoder.nominatim();
    L.Control.Geocoder({
        geocoder: geocoder
    }).addTo(map);

    geocoder.on('markgeocode', function (e) {
        var latlng = e.geocode.center;
        L.marker(latlng).addTo(map).bindPopup('Seçilen adres: ' + e.geocode.name).openPopup();
    });
};

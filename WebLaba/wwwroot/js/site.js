const uri = 'api/Corporations1';
let corporations = [];
let c = [];

mapboxgl.accessToken = 'pk.eyJ1Ijoic2FzaGFtYXBib3h1c2VyIiwiYSI6ImNraDJsajV1ZTBnMm4ycm14dm45dWQ5ZDgifQ.0Oeq4nzOsQcqbU4uvvxDvw';
var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v11',
    center: [33, 50],
    zoom: 4
});

map.on('load', function () {
    getCategories();
});

map.addControl(new mapboxgl.FullscreenControl());

function randomInt(min, max) {
    return min + Math.floor((max - min) * Math.random());
}

function getCategories() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayCategories(data))
        .catch(error => console.error('Unable to get categories.', error));
}

function _displayCategories(data) {
    data.forEach(category => {
        var el = document.createElement('div');
        el.className = 'marker';
        el.style.backgroundImage = `url("Image/${category.emblem}")`;
        el.style.backgroundSize = "contain";
        el.style.backgroundRepeat = "no-repeat";
        //el.style.backgroundColor = "black";
        el.style.width = 70 + 'px';
        el.style.height = 70 + 'px';

        // add marker to map
        new mapboxgl.Marker(el)
            .setLngLat([randomInt(0, 60), randomInt(0, 60)])
            .addTo(map);
    });
    corporations = data;
    corporations.forEach(cat => {
        c.push(cat.name);
    });
    $(function () {

        var langs = c;
        $('input#lang').autocomplete({
            source: langs
        }, { delay: 300, minLength:2 });

    });
}

$(window).load(function(){
    $('#map').gmap3({
        map: {
            address: mapAddress,
            options: {
                zoom: 18,
                mapTypeControl: true,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
                },
                navigationControl: false,
                scrollwheel: true,
                streetViewControl: true,
                panControl: false
            }
        },
        marker: {
            address: mapAddress
        },
    });
});
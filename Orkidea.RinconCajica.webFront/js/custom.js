/**********************
 * Configuration
 *********************/

    /* Background Music */
var songs = ['assets/danosongs.com-magicghost.mp3','assets/danosongs.com-magicghost.ogg'], // First: mp3, second: ogg; Music by Dan-O at DanoSongs.com
    autoplay = false,           // Boolean, set to false if you don'T want to autostart it
    startingVolume = 0.4,       // A number between 0 and 1
    
    /* Background Slider */
    sliderSettings = {
        auto: true,             // Boolean: Animate automatically, true or false
        speed: 1000,            // Integer: Speed of the transition, in milliseconds
        timeout: 4000,          // Integer: Time between slides, in milliseconds
        random: false,          // Boolean: Randomize the order of the slides, true or false
    },
    
    /* Sticky Menu */
    stickyMenu = false,         // Use sticky menu? Set to true to turn on
    
    /* Index text slideshow interval */
   	indexInterval = 4000,
   	
   	/* Maps */
   	mapAddress = 'Downtown Dubai';

/**********************
 * Don't edit below
 *********************/

var player, portfolio, sliderDir = false;

$(window).load(function(){
    
    
    ///* Background music */
    //player = new Player;
    
    //$('#music_toggle').click(function(){
    //    if($(this).hasClass('play')){
    //        player.player.pause();
    //        $(this).removeClass('play');
    //        $.cookie('autoplay', 'false');
    //    }
    //    else{
    //        player.play();
    //        $(this).addClass('play');
    //        $.cookie('autoplay', 'true');
    //    }
    //});
    
    //autoplay = $.cookie('autoplay') === 'false' ? false : true;
    
    //if(autoplay){
    //    $('#music_toggle')[0].click();
    //}
    
    /* Init tolltips */
    $('.tt').tooltip();
    
    /* Background slider */
   	function sliderTimer(){
   		do_animation();
   		next_slide(sliderDir);
   	}
   	function do_animation(){
   		if($('#bg_timer').length <= 0){
   			$('body').append('<div id="bg_timer"></div>');
   		}
   		$('#bg_timer').html('<div></div>');
   		$('#bg_timer > div').animate({width: '100%'}, sliderSettings.timeout);
   	}
   	do_animation();

    sliderSettings.nav = true;
    sliderSettings.before = sliderTimer;
    $("#bg_slide ul").responsiveSlides(sliderSettings);
    
    
    /* Portfolio Filter */
    $('.filter-portfolio .always-visible').click(function(){
        $(this).parent().find('li').toggleClass('show');
    });
    
    /* Init portfolio */
    portfolio = new Portfolio();
    portfolio.init();
    
    /* Portfolio pagination */
    $('.portfolio .pagination a').click(function(e){
        portfolio[$(this).data('direction')]();
        //$("html, body").animate({ scrollTop: $('page-title h3').offset().top-20 }, 1000);
        e.preventDefault();
        return false;
    });
    
    /* Search Toggle */
    $('#search_toggle').click(function(){
        $('#search_form').toggleClass('show');
        $(this).toggleClass('active');
    });
    
    /* Sticky menu */
    if(stickyMenu){
        $('body').addClass('padding');
        $(window).scroll(function(){
                if( $(window).scrollTop() > 50 ) {
                        $('.navbar').addClass('small');
                } else {
                        $('.navbar').removeClass('small');
                }
        }).resize();
    }
    
    /* Portfolio popup */
    $('body').delegate('#portfolio_holder .project-item', 'click', function(){
        $(this).find('a')[0].click();
    });
    $('#portfolio_holder').magnificPopup({
        delegate: 'a',
        type: 'ajax'
        // other options
    });

    /* Arrows for titles */
    $('.page-title').each( function() {
        $(this).append('<span class="arrow"></span>');
    });
    
    /* Members hover */
    $('.member').hover(function(){
        var t = 0;
        $(this).find('.links a').each(function(){
            var that = $(this);
            setTimeout(function(){
                that.css({top: 0});
            }, t);
            t = t + 100;
        });
    },function(){
        var t = 0;
        $(this).find('.links a').each(function(){
            var that = $(this);
            setTimeout(function(){
                that.css({top: '100%'});
            }, t);
            t = t + 100;
        });
    });
    
    /* Member loading */
   var t = 0;
   $('.member').each(function(){
   	var that = $(this);
   	setTimeout(function(){
   		that.addClass('on');
   	}, t);
   	t = t + 300;
   });
   
   /* Blog Hover */
   $('.post-item').hover(function(){
	var t = 0;
	$(this).find('li').each(function(){
	    var that = $(this);
	    setTimeout(function(){
		that.addClass('on');
	    },t);
	    t = t + 100;
	});
    },function(){
	var t = 0;
	$(this).find('li').each(function(){
	    var that = $(this);
	    setTimeout(function(){
		that.removeClass('on');
	    },t);
	    t = t + 100;
	});
    });
    
    /* Contact Form AJAX */
    $('.contact-us form').submit(function(){
        
        var d = $(this).serialize(),
            that = $(this);
        
        $(this).find('button').prepend('<i class="icon-spinner icon-spin"></i> ');
        
        $.ajax({
            type: "POST",
            dataType : 'json',
            data: d,
            dataType: 'json',
            cache: false,
            url: 'sendContactMessage',
            success: function(data){
                that.find('button i').remove();
                if(data == 'OK'){
                    that.find('button').text("Su mensaje, ha sido enviado!").attr('disabled','disabled');
                }
                else{
                    alert('Ocurrio un error al intentar enviar su mensaje. Verifique los campos e intentelo de nuevo.');
                }
            }
        });
    
        return false;
    });
    
    /* Index animations */
   
   	var h = (($(window).height() - 145) / 2) - 200;
   	h = h > 20 ? h : 20;
   	$('.index h1').css({'top': h + 'px'});
   	setTimeout(function(){
	   	$('.index h1 span').addClass('op');
	  }, 800);
	  
		  
		var t = 0;
		$('.slider-nav li, .social-media li').each(function() {
			var that = $(this);
			setTimeout(function() {
				that.addClass('on');
			}, t);
			t = t + 300;
		});
		
		var slides = 0;
		
		$(".index h1 mark").each(function() {
	
			$(this).attr('data-slide', slides);
	
			//show first slide
			if (slides === 0) {
				var h = $(this).addClass('active').height();
				$('.index h1').css({
					'height' : h + 'px'
				});
			}
	
			slides++;
	
		});
		
		function next_slide(prev) {
			
			var current = $('.index h1 mark.active').data('slide'),
					h,
					func = prev ? 'prev' : 'next',
					sel = prev ? 'last-of-type' : 'first-of-type';
			
			if($('.index h1 mark.active')[func]('mark').length > 0){
				h = $('.index h1 mark.active').removeClass('active')[func]('mark').addClass('active').height();
			}
			else{
				$('.index h1 mark.active').removeClass('active');
				h = $('.index h1 mark:' + sel).addClass('active').height();
			}

			$('.index h1').css({
				'height' : h + 'px'
			});
	
		}
		
		/* BG Slider events */
		$('.slider-nav .next').click(function(){sliderDir = false;});
		$('.slider-nav .prev').click(function(){sliderDir = true;});
    
    
    /* Init map */
   	//$('#map').gmap3({
   	//	map:{
	//      address: mapAddress,
	//      options:{
	//        zoom:18,
	//        mapTypeControl: true,
	//        mapTypeControlOptions: {
	//          style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
	//        },
	//        navigationControl: false,
	//        scrollwheel: true,
	//        streetViewControl: true,
	//        panControl: false
	//      }
	//    },
	//    marker:{
	//      address: mapAddress
	//    },
  	//});
   	
    /* Responsive YouTube videos */
    // Find all YouTube videos
    var $allVideos = $("iframe[src^='http://www.youtube.com']");
    
    // Figure out and save aspect ratio for each video
    $allVideos.each(function() {
    
      $(this)
        .data('aspectRatio', this.height / this.width)
    
        // and remove the hard coded width/height
        .removeAttr('height')
        .removeAttr('width');
    
    });
    
    // When the window is resized
    $(window).resize(function() {
    	
    	var h = (($(window).height() - 145) / 2) - 200;
    	h = h > 20 ? h : 20;
   		$('.index h1').css({'top': h + 'px'});
        
      // Resize all videos according to their own aspect ratio
      $allVideos.each(function() {
        
        var $el = $(this),
            newWidth = $el.parent().width();

        $el
          .width(newWidth)
          .height(newWidth * $el.data('aspectRatio'));
    
      });
      
    // Kick off one resize to fix all videos on page load
    }).resize();

});


function Player(){
    
    this.player;

    this.play = function() {
     
        if (window.HTMLAudioElement) {
          this.player = new Audio('');
           
          if(this.player.canPlayType('audio/ogg')) {
            this.player = new Audio(songs[1]);
          }
          else if(this.player.canPlayType('audio/mp3')) {
            this.player = new Audio(songs[0]);
          }
            
            this.player.volume = 0;
            this.player.play();
            $(this.player).animate({volume: startingVolume}, 5000);
            
          
        }
        else {
          alert('HTML5 Audio is not supported by your browser!');
        }
        
    }

}

function Portfolio(){
    
    var that = this;
    
    this.init = function(page){
        page = typeof(page) == 'undefined' ? 0 : page;
        $('#portfolio_pages .page').eq(page).addClass('active').clone().appendTo('#portfolio_holder');
        that.initIsotope();
        that.attachEvents();
        
        $('#portfolio_holder .project-item .thumb-project').each( function() {
		       $(this).append('<div><span><i class="icon-reorder"></i></span></div>');
		       $(this).hoverdir();
		    });
    }
    
    this.initIsotope = function(){
        /* Isotope */
        that.container = $('#portfolio_holder > .page');
        that.container.isotope({
		resizable: false
	});

	$('.filter-portfolio li:not(.always-visible) a').off().click(function(){
	  var selector = $(this).attr('data-filter');
	    that.container.isotope({ 
		filter: selector,
                animationOptions: {
	     duration: 750,
	     easing: 'linear',
	     queue: false
                
	   }
	  });
	  return false;
	});
    }
    
    this.attachEvents = function(){
        $(window).resize(function(){
            that.container.isotope('reLayout');
        });
        /* Portfolio load */
		   var t = 0;
		   $('.project-item').each(function(){
		   	var that = $(this);
		   	setTimeout(function(){
		   		that.addClass('on');
		   	}, t);
		   	t = t + 300;
		   });
    }
    
    this.prev = function(){
        var p = $('#portfolio_pages .page.active').prev('.page');
        if(p.length > 0){
            that.showPage(p.index());
        }
    }
    
    this.next = function(){
        var p = $('#portfolio_pages .page.active').next('.page');
        if(p.length > 0){
            that.showPage(p.index());
        }
    }
    
    this.showPage = function(index){
    		/* Portfolio load */
		   var t = 0;
		   $('.project-item').each(function(){
		   	$(this).removeClass('on');
		   	var that = $(this);
		   	setTimeout(function(){
		   		that.addClass('on');
		   	}, t);
		   	t = t + 300;
		   });
        $('#portfolio_pages .page.active').removeClass('active');
        that.container.isotope('destroy');
        $('#portfolio_holder').empty();
        that.init(index);
    }
    
}